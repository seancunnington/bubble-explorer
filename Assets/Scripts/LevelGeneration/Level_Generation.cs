using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Generation : MonoBehaviour
{

    // The array containing all the roomBlocks to be imported to this level generator
    public RoomBlockList listImport;

    // The array the level is generated in  
    GameObject[,,] levelArray;


    // Sizes for how many cubes to draw in the level.
    // Separated out into x, y, z for readability.
    public int maxCube;
    private int xMax;
    private int yMax;
    private int zMax;

    // Sizes of room blocks
    public int blockSize;

    // Whether to draw the debug boxes or not
    public bool drawBoxes;




    // Generate the level
    void Start()
    {
        xMax = yMax = zMax = maxCube;
        GenerateLevel(blockSize);
    }


    void Update()
    {
        DrawLevel(blockSize);
    }


    // This initializes the space for the level
    public void InitRoomArray(int spacing)
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
                    // Create the GameObject for each field
                    levelArray[xGrid, zGrid, yGrid] = null;

                    // Draw lines for that cube
                    //DrawCube(xGrid*spacing, yGrid*spacing, zGrid*spacing, spacing/2);
                }
            }
        }
        Debug.Log("Level Array Initialized");
    }


    // This initializes layers of 2D Array that holds the level blocks
    void GenerateLevel(int spacing){

        // Initialize the Room Array
        InitRoomArray(spacing);

        int roomCounter = 0;

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
                    levelArray[xGrid, zGrid, yGrid] = CreateRoom(xGrid, yGrid, zGrid, roomCounter, spacing);

                    roomCounter++;

                }

            }

        }

    }


    // Generates each individual room and their necessary components
    GameObject CreateRoom(int xGrid, int yGrid, int zGrid, int roomCounter, int spacing)
    {
        //Create a temporary name to handle the new GameObject
        GameObject Temporary_Handler;
        Temporary_Handler = new GameObject("RoomBlock_" + roomCounter);

        // Set the Position
        Temporary_Handler.GetComponent<Transform>().position = new Vector3(xGrid*spacing, yGrid*spacing, zGrid*spacing);

        // Set the Scale
        Temporary_Handler.GetComponent<Transform>().localScale *= spacing;

        // Set the Mesh
        Temporary_Handler.AddComponent<MeshFilter>().mesh = listImport.blockList[1].roomMesh;

        // Set the Material and its variables
        Temporary_Handler.AddComponent<MeshRenderer>().material = listImport.blockList[1].roomMat;
        Temporary_Handler.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        // Set the Collider
        Temporary_Handler.AddComponent<MeshCollider>().sharedMesh = listImport.blockList[1].roomMesh;

        // Set the Name
        Temporary_Handler.name = "RoomBlock_" + roomCounter;

        // Set the Parent
        Temporary_Handler.GetComponent<Transform>().parent = this.gameObject.GetComponent<Transform>();

        return Temporary_Handler;
    }


    // Draw a cube out of lines -- used for visual debugging
    void DrawCube(int xCenter, int yCenter, int zCenter, int spacing)
    {
        // A cube only has 8 points
        // 'tp' means 'top point'
        // 'bt' means 'bottom point'
            Vector3 tp1 = new Vector3(xCenter - spacing, yCenter + spacing, zCenter - spacing);
            Vector3 tp2 = new Vector3(xCenter + spacing, yCenter + spacing, zCenter - spacing);
            Vector3 tp3 = new Vector3(xCenter + spacing, yCenter + spacing, zCenter + spacing);
            Vector3 tp4 = new Vector3(xCenter - spacing, yCenter + spacing, zCenter + spacing);

            Vector3 bp1 = new Vector3(xCenter - spacing, yCenter - spacing, zCenter - spacing);
            Vector3 bp2 = new Vector3(xCenter + spacing, yCenter - spacing, zCenter - spacing);
            Vector3 bp3 = new Vector3(xCenter + spacing, yCenter - spacing, zCenter + spacing);
            Vector3 bp4 = new Vector3(xCenter - spacing, yCenter - spacing, zCenter + spacing);

        // Vertical Lines
        Debug.DrawLine(bp1, tp1, Color.white, 0);       // Vert Line 1
        Debug.DrawLine(bp2, tp2, Color.white, 0);       // Vert Line 2
        Debug.DrawLine(bp3, tp3, Color.white, 0);       // Vert Line 3
        Debug.DrawLine(bp4, tp4, Color.white, 0);       // Vert Line 4

        // Top Lines
        Debug.DrawLine(tp1, tp2, Color.white, 0);
        Debug.DrawLine(tp2, tp3, Color.white, 0);
        Debug.DrawLine(tp3, tp4, Color.white, 0);
        Debug.DrawLine(tp4, tp1, Color.white, 0);

        // Bottom Lines
        Debug.DrawLine(bp1, bp2, Color.white, 0);
        Debug.DrawLine(bp2, bp3, Color.white, 0);
        Debug.DrawLine(bp3, bp4, Color.white, 0);
        Debug.DrawLine(bp4, bp1, Color.white, 0);

    }

    // Continuously draws lines for each cube section.
    // Used for debugging.
    void DrawLevel(int spacing)
    {
        if( drawBoxes )
        {
            // Reset the loop iterators
            var yGrid = 0;
            var xGrid = 0;
            var zGrid = 0;
    
            // This loop is the y size of the array
            for(yGrid = yMax-1; yGrid >= 0; yGrid--)
            {
                // This loop is the z size of the array
                for(zGrid = zMax-1; zGrid >= 0; zGrid--)
                {
                    // This loop is for the x size of the array
                    for(xGrid = xMax-1; xGrid >= 0; xGrid--)
                    {
                        // Draw lines for that cube
                        DrawCube(xGrid*spacing, yGrid*spacing, zGrid*spacing, spacing/2);
                    }
                }
            }
        }
    }
}
