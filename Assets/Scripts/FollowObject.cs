using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    //the camera for this object to follow
    public GameObject followObject;

    //the speed at which to move this object
    public float moveSpeed;

    public float vertOffset;

    //vector for camera position plus the distance to keep this object from
    Vector3 objVec;


    //initialize camVec
    void Awake()
    {
        objVec = new Vector3(0,0,0);
    }

    // Update is called once per frame
    void Update()
    {
        //set camVec to the camera position vector, but with a vertical offset via Y axis
        objVec = new Vector3(   followObject.GetComponent<Transform>().position.x,
                                followObject.GetComponent<Transform>().position.y + vertOffset,
                                followObject.GetComponent<Transform>().position.z);

        transform.localPosition = Vector3.MoveTowards(  transform.localPosition, 
                                                        objVec,
                                                        Time.deltaTime * moveSpeed);
    }
}
