using UnityEngine;
using System.Collections;

//for tcp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using UnityEngine.UI;

using System.Net.Sockets;

public class GetTrajectoryFromServer : MonoBehaviour
{
    //flag to check if the user has tapped / clicked. 
    //Set to true on click. Reset to false on reaching destination

    private bool flag = false;
    public GameObject Vehicle;
    public Text debugText;
    //destination point
    public Vector3 endPoint;
    //alter this to change the speed of the movement of player / gameobject
    public float duration = 10.0f;
    //vertical position of the gameobject
    private float yAxis;

    public int compMode = 0;
    public float[] start = { 0, 0, 0 };
    public float[] final = { 0, 0, 0 };
    public int pointsNum;
    public float[] timeVec;
    public float[] velVec;
    public float[] steerVec;
    public float[] xVec;
    public float[] yVec;
    public Material newMat;
    public LayerMask PathDrawingLayer;
    void Start()
    {
        LineRenderer pathLine = gameObject.AddComponent<LineRenderer>();
        pathLine.positionCount = 3;

        //save the y axis value of gameobject
        yAxis = Vehicle.transform.position.y;
        connectToServer("127.0.0.1", 5000);
    }

    // Update is called once per frame
    void Update()
    {

        //check if the screen is touched / clicked   
        if (Input.GetMouseButtonDown(0))
        {
            //declare a variable of RaycastHit struct
            RaycastHit hit;
            //Create a Ray on the tapped / clicked position
            Ray ray;

            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //Check if the ray hits any collider
            if (Physics.Raycast(ray, out hit))
            {
                //set a flag to indicate to move the gameobject
                //flag = true;
                //save the click / tap position
                endPoint = hit.point;
                //as we do not want to change the y axis value based on touch position, reset it to original y axis value
                endPoint.y = yAxis;
                // Debug.Log(endPoint);

                //send start and final points to server
                float[] start = { Vehicle.transform.position.x, Vehicle.transform.position.y, Vehicle.transform.position.z };
                float[] start_angle = { Vehicle.transform.eulerAngles.x, Vehicle.transform.eulerAngles.y, Vehicle.transform.eulerAngles.z };
                float[] final = { endPoint.x, endPoint.y, endPoint.z };

                serialize(compMode);//copute mode. 0 regular, 1 - increase velocity
                serialize(start, 3);
                serialize(start_angle, 3);
                serialize(final, 3);

                sendData();

                readData();//wait for trajectory
                int errorNum = 0;
                deserialize(ref errorNum);

                if (errorNum == 0)//if trajectory computation was ok - read trajectory
                {
                    pointsNum = 0;
                    deserialize(ref pointsNum);//number of points along the trajectory
                    timeVec = new float[pointsNum];
                    deserialize(timeVec, pointsNum);
                    velVec = new float[pointsNum];
                    deserialize(velVec, pointsNum);
                    steerVec = new float[pointsNum];
                    deserialize(steerVec, pointsNum);
                    xVec = new float[pointsNum];
                    deserialize(xVec, pointsNum);
                    yVec = new float[pointsNum];
                    deserialize(yVec, pointsNum);

                    debugText.text = "points num: " + pointsNum.ToString();
                    //deserialize(yVec, pointsNum);

                    LineRenderer pathLine = GetComponent<LineRenderer>();
                    pathLine.positionCount = pointsNum;
                    pathLine.startWidth = 0.5f;
                    pathLine.endWidth = 0.5f;
                    
                    pathLine.material = newMat;
                    for (int i = 0; i < pointsNum; i++)
                    {
                        Vector3 Point = new Vector3(xVec[i], 0, yVec[i]);//point on path x-z
                        Physics.Raycast(Point, Vector3.up, out hit);//compute y
                        Point.y = hit.distance + 0.4f;//add high
                                                      //print("Found an object - distance: " + hit.distance);
                        pathLine.SetPosition(i, Point);
                    }
                }
                else//if trajectory error
                {
                    Debug.Log("error: compute trajectory");
                }


            }

        }

    }

    //////////////////////////////socket functions//////////////////////////
    static NetworkStream stream;
    static void connectToServer(String IP, int port)
    {
        try
        {
            // Create a TcpClient.
            // Note, for this client to work you need to have a TcpServer 
            // connected to the same address as specified by the server, port
            // combination.
            TcpClient client = new TcpClient(IP, port);
            // Get a client stream for reading and writing.
            stream = client.GetStream();
        }
        catch (ArgumentNullException e)
        {
            Console.WriteLine("ArgumentNullException: {0}", e);
        }
        catch (SocketException e)
        {
            Console.WriteLine("SocketException: {0}", e);
        }

    }
    static void sendData()
    {
        try
        {
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(output_data_str);
            stream.Write(data, 0, data.Length);
            //output_data_str.Remove(0, output_data_str.Length);//erase string
            output_data_str = string.Empty;
        }
        catch (ArgumentNullException e)
        {
            Console.WriteLine("ArgumentNullException: {0}", e);
        }
        catch (SocketException e)
        {
            Console.WriteLine("SocketException: {0}", e);
        }

    }
    static void readData()
    {
        try
        {
            Byte[] data1 = new Byte[1000000];//1Mb 

            // Read the first batch of the TcpServer response bytes.
            Int32 bytes = stream.Read(data1, 0, data1.Length);
            input_data_str = System.Text.Encoding.ASCII.GetString(data1, 0, bytes);
        }
        catch (ArgumentNullException e)
        {
            Console.WriteLine("ArgumentNullException: {0}", e);
        }
        catch (SocketException e)
        {
            Console.WriteLine("SocketException: {0}", e);
        }
    }

    private static string input_data_str;
    private static string output_data_str;
    static void serialize(int data)
    {
        output_data_str += (data.ToString());
        output_data_str += ",";
    }
    static void serialize(int[] data, int lenght)
    {
        for (int i = 0; i < lenght; i++)
        {
            output_data_str += (data[i].ToString());
            output_data_str += ",";
        }
    }
    static void serialize(float data)
    {
        output_data_str += (data.ToString());
        output_data_str += ",";
    }
    static void serialize(float[] data, int lenght)
    {
        for (int i = 0; i < lenght; i++)
        {
            output_data_str += (data[i].ToString());
            output_data_str += ",";
        }
    }

    static void deserialize(ref int data)
    {
        string tmp;
        int next = input_data_str.IndexOf(",");
        tmp = input_data_str.Substring(0, next);
        input_data_str = input_data_str.Remove(0, next + 1);
        data = Int32.Parse(tmp);
    }
    static void deserialize(int[] data, int lenght)
    {

        for (int i = 0; i < lenght; i++)
        {
            int next = input_data_str.IndexOf(",");
            string tmp = input_data_str.Substring(0, next);
            input_data_str = input_data_str.Remove(0, next + 1);
            data[i] = Int32.Parse(tmp);
        }
    }
    static void deserialize(ref float data)
    {
        string tmp;
        int next = input_data_str.IndexOf(",");
        tmp = input_data_str.Substring(0, next);
        input_data_str = input_data_str.Remove(0, next + 1);
        data = float.Parse(tmp);
    }
    static void deserialize(float[] data, int lenght)
    {

        for (int i = 0; i < lenght; i++)
        {
            int next = input_data_str.IndexOf(",");
            string tmp = input_data_str.Substring(0, next);
            input_data_str = input_data_str.Remove(0, next + 1);
            data[i] = float.Parse(tmp);
        }
    }
    ////////////////////////////////////////
}
