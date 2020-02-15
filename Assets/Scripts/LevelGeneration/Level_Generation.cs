using UnityEngine;



/// <summary>
/// Creates a level of RoomBlocks that the player moves around in while during a drone mission.
/// The main process happens in Start()
/// Initialize the 3D array with null,
/// *Pick the main path and assign rooms for each RoomBlock along the path
/// *-- need to finish later --
/// </summary>
/// 
public class Level_Generation : MonoBehaviour
{

     // The array containing all the RoomBlocks.
     [SerializeField]
     private RoomBlockList listImport = null;

     // The 3D array the level is generated in.
     //  --TO DO: This will need to have a delete function when ending the scene for a build. --
     private GameObject[,,] levelArray = null;


     // Sizes for how many cubes to draw in the level.
     // Separated out into x, y, z for two reason:
     //   Future posibility of having asymmetrical levels in terms of x, y, z.
     //   Readability.
     [Range(3, 15)]
     public int maxCube;

     private int xMax;
     private int yMax;
     private int zMax;

     // The scale of the RoomBlocks.
     [Range(10, 50)]
     public int blockScale;

     // This will always be even.
     private int blockSize;

     // The total number of rooms created.
     private int roomCounter = 0;

     // Whether to draw the debug boxes or not
     [SerializeField]
     private bool drawBoxes = false;




     /// <summary>
     /// Initializes the maximum size of the level and begins level generation.
     /// Currently within Start() for sake of prototyping. Will move outside of this in the future.
     /// </summary>
     /// 
     void Start()
     {
          // Lock in the amount of cubes for the level.
          xMax = yMax = zMax = maxCube;

          // If blockScale is not EVEN, then subtract by 1.
          if ((blockScale % 2) != 0)
               blockScale -= 1;

          // Lock in the scale for each cube.
          blockSize = blockScale;

          GenerateLevel(blockSize);
     }



     /// <summary>
     /// Only purpose of this update is continually draw the Debug Boxes for the level.
     /// </summary>
     void Update()
     {
          DrawLevel(drawBoxes);
     }



     /// <summary>
     /// This initializes the space for each RoomBlock in the levelArray[].
     /// </summary>
     /// 
     public void InitRoomArray()
     {
          levelArray = new GameObject[xMax,yMax,zMax];

          // This loop is the y size of the array
          for(var yGrid = yMax-1; yGrid >= 0; yGrid--)
          {
               // This loop is the z size of the array
               for(var zGrid = zMax-1; zGrid >= 0; zGrid--)
               {
                    // This loop is for the x size of the array
                    for(var xGrid = xMax-1; xGrid >= 0; xGrid--)
                    {
                         // Save the space for each RoomBlock via null.
                         levelArray[xGrid, zGrid, yGrid] = null;
                    }
               }
          }
          Debug.Log("Level Array Initialized");
     }



     /// <summary>
     /// Creates a RoomBlock for every space in the levelArray[]
     /// </summary>
     /// <param name="blockSize"></param>
     /// 
     void GenerateLevel(int blockSize){

          InitRoomArray();  // Initialize the Room Array

          // This loop is the y size of the array
          for(int yGrid = 0; yGrid < yMax; yGrid++)
          {
               // This loop is the z size of the array
               for(int zGrid = 0; zGrid < zMax; zGrid++)
               {
                    // This loop is for the x size of the array
                    for(int xGrid = 0; xGrid < xMax; xGrid++)
                    {
                         // Set the GameObject for each field
                         levelArray[xGrid, zGrid, yGrid] = CreateRoom(xGrid, yGrid, zGrid);

                         
                    }
               }
          }
     }



     /// <summary>
     /// Generates each individual room and their necessary components
     /// </summary>
     /// <param name="xGrid"></param>
     /// <param name="yGrid"></param>
     /// <param name="zGrid"></param>
     /// <returns></returns>
     /// 
     GameObject CreateRoom(int xGrid, int yGrid, int zGrid)
     {
          //Create a temporary name to handle the new GameObject
          GameObject Temporary_Handler;
          Temporary_Handler = new GameObject("RoomBlock_" + roomCounter);

          // Set the Position
          Temporary_Handler.GetComponent<Transform>().position = new Vector3(xGrid*blockSize, yGrid*blockSize, zGrid*blockSize);

          // Set the Scale
          Temporary_Handler.GetComponent<Transform>().localScale *= blockSize;

          // Set the Mesh
          Temporary_Handler.AddComponent<MeshFilter>().mesh = listImport.blockList[1].roomMesh;

          // Set the Material and its variables
          Temporary_Handler.AddComponent<MeshRenderer>().material = listImport.blockList[1].roomMat;
          Temporary_Handler.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

          // Set the Collider
          Temporary_Handler.AddComponent<MeshCollider>().sharedMesh = listImport.blockList[1].roomMesh;

          // Set the Parent
          Temporary_Handler.GetComponent<Transform>().parent = this.gameObject.GetComponent<Transform>();

          // Set the Name
          Temporary_Handler.name = "RoomBlock_" + roomCounter;

          // Increase the room counter for every room created.
          // Incremented after setting the name so that the names start at 0.
          roomCounter++;

          return Temporary_Handler;
     }



     /// <summary>
     /// Draw a single cube out of lines.
     /// Used for visual debugging.
     /// </summary>
     /// <param name="xCenter"></param>
     /// <param name="yCenter"></param>
     /// <param name="zCenter"></param>
     /// 
     void DrawCube(int xCenter, int yCenter, int zCenter)
     {
          // Calculating each line from the center point of each cube,
          // so it's necessary to use half of the blockSize.
          int halfBlockSize = blockSize / 2;

          // Each cube has 8 points.
          // 'tp' means 'top point'
          // 'bt' means 'bottom point'
          Vector3 tp1 = new Vector3(xCenter - halfBlockSize, yCenter + halfBlockSize, zCenter - halfBlockSize);
          Vector3 tp2 = new Vector3(xCenter + halfBlockSize, yCenter + halfBlockSize, zCenter - halfBlockSize);
          Vector3 tp3 = new Vector3(xCenter + halfBlockSize, yCenter + halfBlockSize, zCenter + halfBlockSize);
          Vector3 tp4 = new Vector3(xCenter - halfBlockSize, yCenter + halfBlockSize, zCenter + halfBlockSize);
                                              
          Vector3 bp1 = new Vector3(xCenter - halfBlockSize, yCenter - halfBlockSize, zCenter - halfBlockSize);
          Vector3 bp2 = new Vector3(xCenter + halfBlockSize, yCenter - halfBlockSize, zCenter - halfBlockSize);
          Vector3 bp3 = new Vector3(xCenter + halfBlockSize, yCenter - halfBlockSize, zCenter + halfBlockSize);
          Vector3 bp4 = new Vector3(xCenter - halfBlockSize, yCenter - halfBlockSize, zCenter + halfBlockSize);

          // Vertical Lines
          Debug.DrawLine(bp1, tp1, Color.white, 0);       // Vert Line 1
          Debug.DrawLine(bp2, tp2, Color.white, 0);       // Vert Line 2
          Debug.DrawLine(bp3, tp3, Color.white, 0);       // Vert Line 3
          Debug.DrawLine(bp4, tp4, Color.white, 0);       // Vert Line 4

          // Top Face Lines
          Debug.DrawLine(tp1, tp2, Color.white, 0);
          Debug.DrawLine(tp2, tp3, Color.white, 0);
          Debug.DrawLine(tp3, tp4, Color.white, 0);
          Debug.DrawLine(tp4, tp1, Color.white, 0);

          // Bottom Face Lines Lines
          Debug.DrawLine(bp1, bp2, Color.white, 0);
          Debug.DrawLine(bp2, bp3, Color.white, 0);
          Debug.DrawLine(bp3, bp4, Color.white, 0);
          Debug.DrawLine(bp4, bp1, Color.white, 0);

     }



     /// <summary>
     /// Continuously draws lines for each cube section.
     /// Used for visual debugging.
     /// </summary>
     /// <param name="drawBoxes"></param>
     /// 
     private void DrawLevel(bool drawBoxes)
     {
          if (!drawBoxes) return;  // This toggles whether to draw the lines or not. 
          
          // Reset the loop iterators
          var yGrid = 0;
          var xGrid = 0;
          var zGrid = 0;
    
          // This loop starts at the Top and iterates down. 
          for(yGrid = yMax-1; yGrid >= 0; yGrid--)
          {
               // This loop starts at the Forward and iterates back.
               for(zGrid = zMax-1; zGrid >= 0; zGrid--)
               {
                    // This loop starts at the Left and iterates right.
                    for(xGrid = xMax-1; xGrid >= 0; xGrid--)
                    {
                         // Draw the lines for this individual cube.
                         DrawCube(xGrid*blockSize, yGrid*blockSize, zGrid*blockSize);
                    }
               }
          }
     }



}
