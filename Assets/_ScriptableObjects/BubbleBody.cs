using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Body Card", menuName = "Body Card")]
public class BubbleBody : ScriptableObject
{
    //Body Information
    public new string name;
    public string description;

    //Meshes & Materials
    public Mesh bubbleShellMesh;
    public Material bubbleShellMaterial;
    public Mesh bubbleBrainMesh;
    public Material bubbleBrainMaterial;

    //Weapon
    public Weapon weapon;

    //Stats
    public float moveSpeed;

    
    

}
