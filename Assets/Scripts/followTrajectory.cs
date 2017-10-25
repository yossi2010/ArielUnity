using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class followTrajectory : MonoBehaviour {

private bool runVehicle = false;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	 void FixedUpdate()
    {
		print("follow trajectory script is on\n");
        // // Debug.Log(float.Parse(data[2,i])+" " + Time.time);
        // if (Time.time > Delay && i < data.GetLength(1) - 3-Sampledist)
        // {
        //     Vector3 loc = myref.InverseTransformPoint(TargetWP);
        //     float dist = loc.magnitude;
        //     float ang = Vector3.Angle(Vector3.forward, new Vector3(loc.x, 0, loc.z));
        //     float Throttle = float.Parse(data[1, i]) - driver.xVel;
        //     float Steering = -float.Parse(data[2, i]);
        //     Steering = loc.x > 0 ? Steering + SteeringFixFactor * ang : Steering - SteeringFixFactor * ang;
        //     Throttle = loc.y > 0 ? Throttle - loc.y * ThrottleFixFactor : Throttle + loc.y * ThrottleFixFactor;
        //     driver.Drive(Throttle, Steering);
        //     Sampledist = (int)(float.Parse(data[1, i]) / 2);
        //     if (Time.time - Delay > float.Parse(data[0, i]))
        //     {
        //         i++;
        //         TargetWP = new Vector3(float.Parse(data[3, i + Sampledist]), 0, float.Parse(data[4, i + Sampledist]))+StartingPoint;
        //     }
        // }
        // else driver.Drive(-driver.xVel * BreakForce, 0); //breaks on
    }
}
