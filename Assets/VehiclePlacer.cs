using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehiclePlacer : MonoBehaviour {
    private bool isReplacing;
	public GameObject GameObjectToPlace;
	Rigidbody rb;
 public LayerMask PlacebleLayers;
    public float Offset;

    // Use this for initialization
    void Start () {
		rb=GameObjectToPlace.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		if (isReplacing)
		{
			RaycastHit hit;
            //Create a Ray on the tapped / clicked position
            Ray ray;

            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //Check if the ray hits any collider
            if (Physics.Raycast(ray, out hit,10000,PlacebleLayers))
            {
                //set a flag to indicate to move the gameobject
                //flag = true;
                //save the click / tap position
                Vector3 endPoint = hit.point;
                //as we do not want to change the y axis value based on touch position, reset it to original y axis value
                
                GameObjectToPlace.transform.position=endPoint+Offset*Vector3.up;
				GameObjectToPlace.transform.rotation=Quaternion.LookRotation(Vector3.forward, hit.normal);
				rb.velocity=Vector3.zero;
			}
			if(Input.GetMouseButtonDown(1))isReplacing=false;
		}
	}
	public void Place()
	{isReplacing=true;}
}
