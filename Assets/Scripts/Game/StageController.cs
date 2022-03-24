using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageController : MonoBehaviour
{
   
    public static int actionCount = 0;

    //Unsummoned chess
    public static GameObject[] UnsummonChessList;
    [SerializeField]
    private GameObject[] unsummonChess;
    //RaycastHit2D hit;
    //public static Vector3 mousePos;
    // Start is called before the first frame update
    void Start()
    {
        GL.Clear(true,true,Color.black);
        GridManager.GenerateMap();
        //Debug.Log("Picked Element: "+PickMenu.selectedFaction[1]);
        UnsummonChessList = unsummonChess;
    }
    
    // Update is called once per frame
    void Update()
    {
    }
    /*GetObjByID()
     * @param   int id
     * @return  return any object lying at location id on map 
     */
    private static GameObject GetObjByID(int id)
    {
        return GridManager.map[id / 8, id % 8].Chess;
    }

    //OK - 0627
    /*GetSurroundDataByID
 * @param   id:  target grid ID
 * @desc       check North, South, East, West grid of the target id grid
 *                    return the appropriate value depends on relationship of target object and surrounding objects
 * @return   surround data of target id
 *                  attackable         +1
 *                  suicidable         -1
 *                  swapable            2
 *                  Moveable            0
 *                  NoEffect           -2     
 *                  Self               -3
 */
    public static int[] GetSurroundDataByID(int id)
    {
        //Debug.Log("ID (getsurround): " + id);
        GameObject targetObj = GetObjByID(id);
        GameObject[] surroundObj = new GameObject[5];
        int[] surround = new int[5];
        int N = -2, S = -2, E = -2, W = -2, C = -3;

        if (id >= 8)
        {
            GameObject north = GetObjByID(id - 8);
            if (north != null)
            {
                surroundObj[0] = north;
                //Debug.Log("North: " + north.name);
                N = CheckRelation(targetObj, north);
                //Debug.Log("North relation: "+N);
            }
            else
            {
                //Debug.Log("North is null!");
                N = 0;
            }
        }
        if (id < GridManager.map.Length - 8)
        {
            GameObject south = GetObjByID(id + 8);
            if (south != null)
            {
                //Debug.Log("South: " + south.name);
                surroundObj[1] = south;
                S = CheckRelation(targetObj, south);
            }
            else
            {
                //Debug.Log("South is null!");
                S = 0;
            }
        }
        if (id % 8 != 7)
        {
            GameObject east = GetObjByID(id + 1);
            if (east != null)
            {
                //Debug.Log("East: " + east.name);
                surroundObj[2] = east;
                E = CheckRelation(targetObj, east);
            }
            else
            {
                //Debug.Log("East is null!");
                E = 0;
            }
        }
        if (id % 8 != 0)
        {
            GameObject west = GetObjByID(id - 1);
            if (west != null)
            {
                //Debug.Log("W");
                //Debug.Log("West: " + west.name);
                surroundObj[3] = west;
                W = CheckRelation(targetObj, west);
            }
            else
            {
                W = 0;
                //Debug.Log("West is null!");
            }
        }

        surround[(int)SurroundDirection.N] = N;
        surround[(int)SurroundDirection.S] = S;
        surround[(int)SurroundDirection.E] = E;
        surround[(int)SurroundDirection.W] = W;
        surround[(int)SurroundDirection.C] = C;
        
        //for (int i = 0; i < surround.Length; i++)
        //{
        //    Debug.Log("Surround(" + i + "): " + surround[i]);
        //}

        //Debug.Log("N value: " + N);
        //Debug.Log("S value: " + S);
        //Debug.Log("E value: " + E);
        //Debug.Log("W value: " + W);
        return surround;
    }

    //OK - 0627
    /*GetSurroundDataByObj
     * @param   obj: target object
     * @desc       GetSurroundDataByID() 
     *                  in which ID is the current position ID of obj
     *@return    surround data of target id
     */
    public static int[] GetSurroundDataByObj(GameObject obj)
    {

        for (int i = 0; i < GridManager.row; i++)
        {

            for (int j = 0; j < GridManager.col; j++)
            {
                if (GridManager.map[i, j].Chess == obj)
                {
        
                    //Debug.Log("chess: " + GridManager.map[i, j].Chess.name);
                    return GetSurroundDataByID(GridManager.map[i, j].GridID1);
                }
            }
        }
        return null;
    }
    
    //OK - 0627
    /*CheckRelation()
     * @param:  GameObject  obj1
     *                  GameObject  obj2
     * @desc:       return relation between 2 chesses
     * @return:     -2      if none
     *                    -1      if obj1 weaker than obj2
     *                     1      if obj1 stronger than obj2
     *                     2      if swapable  
     * (return value match the surround data in GetSurroundDataByID())
    */
    public static int CheckRelation(GameObject obj, GameObject target)
    {
        int[] t1 = new int[2] { -2, -2 };
        t1 = ChessScript.GetTypeByObj(obj);
        int[] t2 = new int[2] { -2, -2 };
        t2 = ChessScript.GetTypeByObj(target);

        int relation = -2;
        int player = t2[1]-t1[1];
        int element = t1[0] - t2[0];
        //Blue(3) and red(0) case
        if (element > 2 && player != 0) return 1;
        if (element < -2 && player != 0) return -1;
        //If same player
        if(player == 0)
        {
            /* unused
            //If different element->swap
            if (element != 0) return 2;
            */
            //Debug.Log("Same player");
            return -2;
        }
        else
        {
            //Debug.Log("Different player");
            if (Mathf.Abs(element) % 2 == 0)
            {
                //Debug.Log("Not related");
                //If not related element and not the same->swapable
                if (element != 0) { relation = 2;/* Debug.Log("SWAP!");*/ }
                //Else return none
                else relation = -2;
                
            }
            else
            {
                //Debug.Log("Related!");
                if (element > 0)
                {

                    if (element > 1)
                    {
                        relation = 1;
                    }
                    else relation = -1;
                }
                else relation = 1;                
            }            
        }
        //Debug.Log("im here!");
        return relation;
    }

 
}

