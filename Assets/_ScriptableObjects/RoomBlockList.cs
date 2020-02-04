using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Room Block List", menuName = "Room Block List")]
public class RoomBlockList : ScriptableObject
{
   // This is just a public array of Room Blocks
   // It's only purpose is to be imported into a Level Generator

   public RoomBlock[] blockList;

}
