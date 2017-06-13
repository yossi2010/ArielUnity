using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M113Controller : MonoBehaviour
{

    public float LeftTrackSpeedCommand, RightTrackSpeedCommand;
    public Rigidbody[] jointsR, jointsL;
    Transform myref;
    public float Torque = 4000;
    public float xVel, RightXVel, LeftXVel;
    public float LeftTrackSpeed, RightTrackSpeed;
    Rigidbody rb;
    public bool velcontrol = false;
    public float AngCommand = 0, LinCommand = 0, p = 0.001f;
    public Vector3 AngularVel;
    private float Angfactor, LinFactor;

    // Use this for initialization
    void Start()
    {
        myref = transform;

        rb = GetComponent<Rigidbody>();

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad9)) RightTrackSpeedCommand += 0.1f;
        if (Input.GetKeyDown(KeyCode.Keypad6)) RightTrackSpeedCommand -= 0.1f;
        if (Input.GetKeyDown(KeyCode.Keypad7)) LeftTrackSpeedCommand += 0.1f;
        if (Input.GetKeyDown(KeyCode.Keypad4)) LeftTrackSpeedCommand -= 0.1f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        xVel = myref.InverseTransformDirection(rb.velocity).z;
        RightXVel = myref.InverseTransformDirection(jointsR[0].velocity).z;
        LeftXVel = myref.InverseTransformDirection(jointsL[0].velocity).z;

        AngularVel = rb.angularVelocity;
        if (velcontrol)
        {
            LinFactor += p * (LinCommand - xVel);
            Angfactor += p * (AngCommand - AngularVel.y);
            LeftTrackSpeedCommand = LinFactor + Angfactor;
            RightTrackSpeedCommand = LinFactor - Angfactor;

            for (int i = 0; i < jointsL.Length; i++)
            {
                // var tempmotor=joints[i].motor;
                // tempmotor.targetVelocity=Speed;
                // joints[i].motor=tempmotor;
                jointsL[i].AddRelativeTorque(new Vector3(0, -Torque * Mathf.Sign(LeftTrackSpeedCommand), 0), ForceMode.Force);
                jointsL[i].maxAngularVelocity = LeftTrackSpeedCommand / 0.38f;
            }
            for (int i = 0; i < jointsR.Length; i++)
            {
                jointsR[i].AddRelativeTorque(new Vector3(0, Torque * Mathf.Sign(RightTrackSpeedCommand), 0), ForceMode.Force);
                jointsR[i].maxAngularVelocity = RightTrackSpeedCommand / 0.38f;
            }
        }
        else
        {
            LeftTrackSpeed +=LeftTrackSpeedCommand>1?(p/LeftTrackSpeedCommand)*(LeftTrackSpeedCommand - LeftXVel):(p)*(LeftTrackSpeedCommand - LeftXVel);
            RightTrackSpeed +=RightTrackSpeedCommand>1?(p/RightTrackSpeedCommand)*(RightTrackSpeedCommand - RightXVel):(p)*(RightTrackSpeedCommand - RightXVel);
            {
                for (int i = 0; i < jointsL.Length; i++)
                {
                    // var tempmotor=joints[i].motor;
                    // tempmotor.targetVelocity=Speed;
                    // joints[i].motor=tempmotor;
                    jointsL[i].AddRelativeTorque(new Vector3(0, -Torque * Mathf.Sign(LeftTrackSpeed), 0), ForceMode.Force);
                    jointsL[i].maxAngularVelocity = LeftTrackSpeed / 0.38f;
                }
                for (int i = 0; i < jointsR.Length; i++)
                {
                    jointsR[i].AddRelativeTorque(new Vector3(0, Torque * Mathf.Sign(RightTrackSpeed), 0), ForceMode.Force);
                    jointsR[i].maxAngularVelocity = RightTrackSpeed / 0.38f;
                }
            }
        }
    }
    public void Drive(float velR, float velL)
    {
        RightTrackSpeedCommand = velR;
        LeftTrackSpeedCommand = velL;
    }
}
