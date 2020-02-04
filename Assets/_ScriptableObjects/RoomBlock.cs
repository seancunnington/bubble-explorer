using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Room Block", menuName = "Room Block")]
public class RoomBlock : ScriptableObject
{
    // Room Block Information
    public new string name;
    public string description;
    public int id;

    // Mesh
    public Mesh roomMesh;

    // Material
    public Material roomMat;

    // Door Booleans: Manual input of which cube faces are open or closed
    public bool topDoor = false;
    public bool bottomDoor = false;
    public bool leftDoor = false;
    public bool rightDoor = false;
    public bool frontDoor = false;
    public bool backDoor = false;

}
