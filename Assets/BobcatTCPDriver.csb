using System;
using UnityEngine;

public class BobcatTCPDriver : MonoBehaviour
{
    UDPReceive web;
    float[] nums = new float[2] { 0, 0 };
    string[] pieces = new string[2] { "", "" };
		 string data="initialized";
    BobcatDriving driver;
    // Use this for initialization
    void Start()
    {
        web = GetComponent<UDPReceive>();

        driver = GetComponent<BobcatDriving>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        data = web.getLatestUDPPacket();
        pieces = data.Split(new string[] { " " },StringSplitOptions.None);
        nums[0] = float.Parse(pieces[0]);
        nums[1] = float.Parse(pieces[1]);
        driver.DriveBobcat(nums[0], nums[1]);

    }

}
