using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Class GridManager
 * @desc:   Create map grid, input grid data
 */
public class GridManager : MonoBehaviour
{
    //Field Data

    //Grid Size
    public static float GridSizeX = 1.0f;
    public static float GridSizeY = 1.0f;

    //Grid Pixel Size
    public static float GridPixelSizeX = 64.0f;
    public static float GridPixelSizeY = 64.0f;

    //Number of rows and collumn
    public static int row = 8;
    public static int col = 8;

    //Corners Position
    public static Vector3 topLeft = new Vector3(-GridSizeX / 2, -GridSizeY / 2, 0);
    public static Vector3 botRight = new Vector3(GridSizeX / 2, GridSizeY / 2, 0);

    //Map data
    public static GridData[,] map = new GridData[row, col];

    //Player Territory
    public static Territory[] pTerritories = new Territory[4];
    
    private void Start()
    {
        //Reset
        for(int i= 0; i < row; i++)
        {
            for(int j = 0; j < col; j++)
            {
                map[i, j].Chess = null;
            }
        }
        //Initiate territories
        pTerritories = new Territory[4]{
            //Player 1
            new Territory(new Vector2(0,0), new Vector2(3,3)),
            //Player 2
            new Territory(new Vector2(0,4), new Vector2(3,7)),
            //Player 3
            new Territory(new Vector2(4,0), new Vector2(7,3)),
            //Player 4
            new Territory(new Vector2(4,4), new Vector2(7,7))
        };
    }
    public static void GenerateMap()
    {

        float x = GridSizeX;
        float y = GridSizeY;
        string[] colstr = { "A", "B", "C", "D", "E", "F", "G", "H" };
        ////Grid
        Vector2 grid = new Vector2(x, y);


        //top left corner pos
        Vector2 tl = new Vector2(-row / 2 * x + grid.x / 2, col / 2 * y - grid.y / 2);
        //ID
        int id = 0;
        for(int i = 0; i < row; i++)
        {
            for(int j = 0; j < col; j++)
            {
                Vector2 dist = new Vector2(j*GridSizeX, i * GridSizeY);
                map[i, j].Pos = new Vector3( tl.x+dist.x,tl.y - dist.y,1.0f);
                map[i, j].GridID1 = id++;
                map[i, j].Grid_name1 = colstr[j] + "-" + (i+1);
                //Debug.Log("ID: " + map[i, j].GridID1 + " [" + i + "," + j + "]: " + map[i, j].Pos);
            }
        }
    }
    public static GridData GetGridByID(int id)
    {
        return map[id / 8, id % 8];
    }
    public static Vector3 GetPosbyID(int id)
    {
        return GetGridByID(id).Pos;
    }
    public static int GetIDbyPos(Vector3 pos)
    {
        Vector3 conv = new Vector3(-4f, 4f, 0);
        Vector3 convertToTopLeft = pos - conv;
        //Debug.Log("Before convert: "+convertToTopLeft);
        convertToTopLeft = new Vector3((int)convertToTopLeft.x, (int)convertToTopLeft.y, convertToTopLeft.z);
        int ID = (int)convertToTopLeft.x + (Mathf.Abs((int)convertToTopLeft.y)) * col;
        //Debug.Log("ID after convert: " + ID);
        return ID;
    }
    public static void SetChessByID(GameObject obj, int id)
    {
        map[id / 8, id % 8].Chess = obj;
    }
    //public static GameObject GetObjectByID(int ID)
    //{
    //    GridData data = GetGridByID(ID);
    //    if(!data.Equals(null))
    //        return data.Chess;
    //    return null;        
    //}

}
