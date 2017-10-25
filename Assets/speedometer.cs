using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DebugTools;
public class speedometer : MonoBehaviour {

 public float Speed=0,InitialNeedleAngle,VelToAngleFactor=1;
  float Angle = 0;
 public GameObject Vehicle;
 public TextMeshProUGUI GuiSpeed;
 public Vector2 size = new Vector2(128, 128);
 public RectTransform Needle;
	public DebugTools.GraphCanvas graph;
 
ColoradoDrive VehicleDriver;
 void Start() {
	 graph=DebugTools.Grapher.CreateGraph("Speed",GraphCanvasType.LINE_PLOT,true,false,0,50);
	VehicleDriver=Vehicle.GetComponent<ColoradoDrive>();
 }
 void Update() {
	Speed=VehicleDriver.xVel;
	Angle=VelToAngleFactor*Speed;
	GuiSpeed.text=Speed.ToString("F2")+" m/s";
	Needle.eulerAngles=new Vector3(0,0,Angle-InitialNeedleAngle);
 }

}
