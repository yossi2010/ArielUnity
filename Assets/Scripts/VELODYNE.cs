using UnityEngine;
using System.Collections;
using System.IO;
public class VELODYNE : MonoBehaviour
{
    Transform myref;
    public float Timescale = 1;
    public Transform emitter;
    Vector3 shootLaserDir = Vector3.zero;
    public float Range = 10, StartVerticalAngle = 2, VertAng = 26.8f, HorAng = 360, hz = 10, HorRes = 0.08f, NoOfScansPerFrame;
    public int lasercount = 64;
    public bool DebugDraw = true, DebugDrawDots = false,DebugTime=false;
    float[] datacolumn;

    float ang = 0;
    // Use this for initialization
    void Start()
    {
        writer = new StreamWriter(Application.dataPath+"/Data.csv");
        myref = transform;
        NoOfScansPerFrame = (HorAng / HorRes) * hz * Time.fixedDeltaTime;
        currentangle = -HorAng / 2;
       writer.WriteLine("This is a data file");
        datacolumn=new float[lasercount];
    }
    float currentangle;
    private StreamWriter writer;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentangle > HorAng / 2) { currentangle = -HorAng / 2; myref.localEulerAngles = new Vector3(0, -HorAng / 2, 0); }
        for (int i = 0; i < NoOfScansPerFrame; i++)
        { // multiple horizontal scans in 1 physics step in order to achieve the full circle
            if (currentangle > HorAng / 2) { currentangle = -HorAng / 2; myref.localEulerAngles = new Vector3(0, -HorAng / 2, 0); }
            emitter.localEulerAngles = new Vector3(-StartVerticalAngle, 0, 0);
            for (int j = 0; j < lasercount; j++) //the lazer column
            {
                ang = (StartVerticalAngle + j * VertAng / (lasercount - 1));
                emitter.localEulerAngles = new Vector3(ang, 0, 0);
                shootLaserDir = (emitter.forward);
                RaycastHit hit;
                // Physics.Raycast(myref.position,shootLaserDir,out hit,Range);
                if (Physics.Raycast(myref.position, shootLaserDir, out hit, Range))
                {

                    if (DebugDraw) Debug.DrawLine(hit.point, myref.position, Color.red, 0.3f, true);
                    else if (DebugDrawDots) Debug.DrawLine(hit.point, hit.point + 0.1f * Vector3.up, Color.red, 0.3f, true);
                }
                else
                {

                    if (DebugDraw) Debug.DrawLine(myref.position, myref.position + shootLaserDir * Range, Color.blue, 0.3f, true);
                }
                datacolumn[j]=hit.distance;

            }

            WriteData(datacolumn);
            currentangle += HorRes;
            myref.localEulerAngles = new Vector3(0, currentangle, 0);

        }

        if(DebugTime){Timescale=Timescale<=0?0.01f:Timescale;Time.timeScale = Timescale;}
        //debugging timescale	
    }
        void WriteData(float[] toWrite){
            for (int n = 0; n < toWrite.Length; n++)
            {
                writer.Write(string.Format("{0}, ",
                             toWrite[n])); 
            }
            
        writer.WriteLine();

    }
}
