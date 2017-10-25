using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class followPath : MonoBehaviour {


    public Text debugText;
	private bool enable = false;
    public float Delay = 2, BreakForce = 10;
    public float SteeringFixFactor = 0, ThrottleFixFactor = 0,Timescale=1;
    public int Sampledist = 0;
    private float startTime;
    private bool firstFlag = true;
     int i = 0;
     Vector3 TargetWP;
    Transform myref;
    GetTrajectoryFromServer dataFromServer;
    public ColoradoDrive driver;
    
	public void test()
	{
		enable = true;
	}
	// Use this for initialization
	void Start () 
	{
        myref = transform;
        // print("start follow!!!!");
        dataFromServer  =FindObjectOfType<GetTrajectoryFromServer>();
	}

    void Update()
    {
        Timescale = Timescale < 0 ? 0 : Timescale;
        Time.timeScale = Timescale;
    }
    void FixedUpdate()
    {
        if (enable)//enable - drive along the path
        {

            //  print("enable!!!!");


            
            if (i < dataFromServer.pointsNum - 2 - Sampledist)
            {
                // print("i: "+i+"Sampledist: "+Sampledist);

                if (firstFlag)//just the first time
                {

                    TargetWP = new Vector3(dataFromServer.yVec[10], 0, dataFromServer.xVec[10]);
                    startTime = Time.time;
                    firstFlag = false;
                }
                // if(i + Sampledist >= getTrajectoryFromServer.pointsNum-2)
                // {
                //     Sampledist = getTrajectoryFromServer.pointsNum - i-3;//last point
                //     print("Sampledist: "+ Sampledist);
                // }
                Vector3 loc = myref.InverseTransformPoint(TargetWP);//-new Vector3(FollowScript.xVec[0], 0,FollowScript.yVec[0])
                float dist = loc.magnitude;
                float ang = Vector3.Angle(Vector3.forward, new Vector3(loc.x, 0, loc.z));
                float Throttle = dataFromServer.velVec[i]- driver.xVel;
                float Steering = -dataFromServer.steerVec[i];
                
                Steering = loc.x > 0 ? Steering + SteeringFixFactor * ang : Steering - SteeringFixFactor * ang;
                Throttle = loc.y > 0 ? Throttle - loc.y * ThrottleFixFactor : Throttle + loc.y * ThrottleFixFactor;
                driver.Drive(Throttle, Steering);
                
                Sampledist = (int)(dataFromServer.velVec[i]/ 2);
                // debugText.text = "i: " + i.ToString() +"from: "+dataFromServer.pointsNum.ToString()+"time: "+ Time.time.ToString() +"start time: "+startTime.ToString();
            
                if (Time.time - startTime> dataFromServer.timeVec[i])
                {
                    i++; 
                    TargetWP = new Vector3(dataFromServer.xVec[i+Sampledist] , 0,dataFromServer.yVec[i+Sampledist]);
                    // if(i==getTrajectoryFromServer.pointsNum - 1)//last point
                    //     enable = false;
                }
            }
            else 
            {
                 driver.Drive(-driver.xVel * BreakForce, 0); //breaks on
                enable = false;
                // print("break");
            }
            

        }
        else
        {
            driver.Drive(-driver.xVel * BreakForce, 0); //breaks on
            // print("break i = 0");
            i = 0;
            firstFlag = true;
        }

    }
}
