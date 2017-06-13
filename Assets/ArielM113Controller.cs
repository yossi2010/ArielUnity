using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ArielM113Controller : MonoBehaviour
{
    Transform myref;
    public TextAsset csv;
    public M113Controller driver;
    string[,] data;
    public float Delay = 2;
    int i = 1;
    Vector3 TargetWP;
    public int Sampledist;
    public float SteeringFixFactor = 0, ThrottleFixFactor = 0;
    public int LineSegmentResolution = 10;
    static Material lineMaterial;

    // Use this for initialization
    void Start()
    {
        myref = transform;
        data = CSVReader.SplitCsvGrid(csv.text);
        // CSVReader.DebugOutputGrid( CSVReader.SplitCsvGrid(csv.text) ); 
        // for (int j = 1; j < data.GetLength(1) / LineSegmentResolution; j++)
        // {
        //     Debug.DrawLine(new Vector3(float.Parse(data[3, j * LineSegmentResolution]), 0, float.Parse(data[4, j * LineSegmentResolution])), new Vector3(float.Parse(data[3, (j + 1) * LineSegmentResolution]), 0, float.Parse(data[4, (j + 1) * LineSegmentResolution])), Color.red, 50);
        // }
        // TargetWP = new Vector3(float.Parse(data[4, 10]), 0, float.Parse(data[3, 10]));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Debug.Log(float.Parse(data[2,i])+" " + Time.time);
        if (Time.time > Delay && i < data.GetLength(0))
        {
            // Vector3 loc = myref.InverseTransformPoint(TargetWP);
            // float dist = loc.magnitude;
            // float ang = Vector3.Angle(Vector3.forward, new Vector3(loc.x, 0, loc.z));
            float vell = float.Parse(data[i, 0]);
            float velr = float.Parse(data[i, 1]);
						// Steering = loc.x > 0 ? Steering+SteeringFixFactor*ang : Steering-SteeringFixFactor*ang ;
						// Throttle = loc.y > 0 ? Throttle-loc.y*ThrottleFixFactor : Throttle+loc.y*ThrottleFixFactor;
            driver.Drive(velr, vell);
						// Sampledist=(int)(float.Parse(data[1, i])/2);
            // if (Time.time - Delay > float.Parse(data[0, i]))
            // {
                i++;
            //     TargetWP = new Vector3(float.Parse(data[3, i + Sampledist]), 0, float.Parse(data[4, i + Sampledist]));
            // }
        }
    }
    // void OnDrawGizmosSelected() {
    //     Gizmos.color = Color.yellow;
        // Gizmos.DrawSphere(TargetWP, 0.1f);
    // } 	
}
