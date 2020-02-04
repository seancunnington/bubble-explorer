using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClippingPlane : MonoBehaviour
{
    // Material this script is passing values to
    public Material[] mat;

    void Update(){
        // Create the plane
        Plane plane = new Plane(transform.up, transform.position);

        // Transfer values from plane to vector4
        Vector4 planeRepresentation = new Vector4(  plane.normal.x, 
                                                    plane.normal.y,
                                                    plane.normal.z,
                                                    plane.distance);
        
        // Pass the vector of both Top and Bottom Planes to the shader,
        // For each material
        for (int i = 0; i < mat.Length; i++){
            mat[i].SetVector("_PlaneTop", planeRepresentation);
            mat[i].SetVector("_PlaneBottom", planeRepresentation);
        }
    }
}
