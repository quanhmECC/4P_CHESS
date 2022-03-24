using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class ChessScript : MonoBehaviour
{
    //===================Interactable=======================
    [System.Serializable]
    public class SelectMenu
    {
        public Color color;
        public Sprite sprite;
        public string title;
    }
    public string title;
    [SerializeField]
    private SelectMenu[] options;
    //===================================================
    //Var
    private bool isHover = false, isSelected = false;
    private static Transform trSelect = null;
    private ActionList currentAction;
    //private static bool inAction = false;
    private Vector3 posBe4Move = Vector3.zero;
    //private static bool globalHover = false;
    private static GameObject currentObj = null;



    //public float travelTime = .5f;        //time to travel(unused)
    public float speed = 0.5f;              // move speed
    private bool calSpeed;                  //Calculate speed

    //State var
    private bool isMove, isAttack, isSucide, isSwap;

    //Select sprites
    public GameObject move, swap, attack, hover, suicide;

    //Array
    private GameObject[] SelectObj;
    private GameObject[] HoverObj;
    private int[] surroundType;
    public GameObject[] player = new GameObject[4];
    public GameObject basic,red, green, brown, blue;
    private GameObject[] chess;
    public GameObject[] Chess { get => chess; set => chess = value; }
    static ElementData[] eData = {new ElementData(TypeList.Base,"Base"), new ElementData(TypeList.red,"Red"),new ElementData(TypeList.green,"Green"),
            new ElementData(TypeList.brown,"Brown"),new ElementData(TypeList.blue,"Blue"),
            new ElementData(TypeList.player1,"Player1"),new ElementData(TypeList.player2,"Player2"),
            new ElementData(TypeList.player3,"Player3"), new ElementData(TypeList.player4,"Player4")};

    //Global data
    public static SurroundData[] surroundData;
    //===================================================

    //====================Main Functions===================
    private void Start()
    {
        isHover = false;
        isSelected = false;

        SelectObj = new GameObject[5];
        HoverObj = new GameObject[5];
        surroundData = new SurroundData[5];
        //Debug.Log("Chess name: " + this.transform.parent.name);
        basic.SetActive(true);
        Chess = new GameObject[] { basic, red, green, brown, blue };

        if(title == "" || title == null)
        {
            title = gameObject.name;
        }

        UpdatePos();

    }
    private void Update()
    {
        if(isSelected == false)
        {
            //UpdatePos();
            for (int i = 0; i < SelectObj.Length; i++)
            {
                Destroy(SelectObj[i]);
            }
        }
        if (isSelected && transform != trSelect)
        {
            isSelected = false;
            currentObj = null;
            for (int i = 0; i < SelectObj.Length; i++)
            {
                Destroy(SelectObj[i]);
            }
        }
    }
    private void LateUpdate()
    {
        //if (isMove) { UpdatePos(); isMove = false; }
        
    }
    //===================================================

    //====================Hover==========================    
    private void OnMouseOver()
    {
        
        if (currentObj == null||currentObj.GetHashCode() == gameObject.GetHashCode())
        {
            //Debug.Log("HOVER CALLED");
            if (ChessScript.GetTypeByObj(gameObject)[0] == -1)
            {

                return;
            }
            if (!isHover && !isSelected)
            {
                //==========Input surround data on hover=====================================

                surroundType = StageController.GetSurroundDataByObj(gameObject);
                //Debug.Log("here2");
                //Debug.Log("surroundType:" +surroundType.Length);
                GameObject objToCreate = null;
                Vector3 posToCreate = new Vector3(0, 0, 0);
                SelectMenu opToCreate = null;

                for (int i = 0; i < surroundType.Length; i++)
                {
                    //Get obj to create
                    switch (surroundType[i])
                    {
                        case (int)Relation.Self:
                            objToCreate = hover; opToCreate = null;
                            break;
                        case (int)Relation.Sucidable:
                            //Debug.Log("Sucide is called!");
                            objToCreate = suicide; opToCreate = GetOptionByTitle("Suicide");
                            //if (opToCreate != null) Debug.Log("Suicide: " + opToCreate.title);
                            break;
                        case (int)Relation.Moveable:
                            objToCreate = move; opToCreate = GetOptionByTitle("Move");
                            break;
                        case (int)Relation.Attackable:
                            objToCreate = attack; opToCreate = GetOptionByTitle("Attack");
                            break;
                        case (int)Relation.Swapable:
                            objToCreate = swap; opToCreate = GetOptionByTitle("Swap");
                            break;
                        case (int)Relation.NoEffect:
                            objToCreate = null; opToCreate = null;
                            break;
                    }
                    //Get position to create 
                    switch (i)
                    {
                        case (int)SurroundDirection.C:
                            posToCreate = new Vector3(transform.position.x, transform.position.y, 5);
                            break;
                        case (int)SurroundDirection.N:
                            posToCreate = new Vector3(transform.position.x, transform.position.y + GridManager.GridSizeY, 5);
                            break;
                        case (int)SurroundDirection.S:
                            posToCreate = new Vector3(transform.position.x, transform.position.y - GridManager.GridSizeY, 5);
                            break;
                        case (int)SurroundDirection.W:
                            posToCreate = new Vector3(transform.position.x - GridManager.GridSizeX, transform.position.y, 5);
                            break;
                        case (int)SurroundDirection.E:
                            posToCreate = new Vector3(transform.position.x + GridManager.GridSizeX, transform.position.y, 5);
                            break;
                    }
                    //Input surround data
                    surroundData[i] = new SurroundData((SurroundDirection)i, surroundType[i], posToCreate, objToCreate, opToCreate);
                }
                //=======================================================================
                //==========Instantiate hover objects==========================================
                for (int i = 0; i < HoverObj.Length; i++)
                {
                    if (surroundData[i].SelectObj1 != null)
                        HoverObj[i] = Instantiate(surroundData[i].SelectObj1, surroundData[i].Pos, transform.rotation);
                }
                //=======================================================================

            }
        }
        isHover = true;
    }
    private void OnMouseExit()
    {
        if (!isSelected)
        {
            //Debug.Log("EXIT CALLED!");            
            isHover = false;
            if (HoverObj.Length > 0)
            {
                for (int i = 0; i < HoverObj.Length; i++)
                {
                    Destroy(HoverObj[i]);
                }
            }
        }
    }
    //===================================================

    //====================Click=========================== 
    void OnMouseDown()
    {
        isSelected = true;
        RadialMenuSpawner.mSpawner.SpawnMenu(this);
        currentObj = gameObject;    
    }
    //=================================================

    //====================Actions========================
    //====================Act select======================
    public void Act(ActionList action, int ID)
    {
        //Debug.Log("ACT FUNCTION");
        switch (action)
        {
            case ActionList.Move:
                //Debug.Log("MOVE OBJ");
                //isMove = true;
                MoveChess(ID); break;
            case ActionList.Attack:
                //Debug.Log("ATTACK");
                AttackChess(ID); break;
            case ActionList.Swap:
                //Debug.Log("SWAP");
                SwapChess(ID); break;
            case ActionList.Suicide:
                //Debug.Log("SUICIDE");
                SuicideChess();break;

            default:
                break;
        }


    }     
    //=================================================

    //====================Attack========================
    public void AttackChess(int ID) {
        //For animation
        //StartCoroutine(Attack(ID));
        GridData data = GridManager.GetGridByID(ID);
        int[] typeData = GetTypeByObj(gameObject);
        //Debug.Log("Target: " + typeData[0]+ " "+typeData[1]);
        SetTypeByObj(data.Chess, (TypeList)typeData[0]);
        StageController.actionCount++;
    }
    IEnumerator Attack(int ID)
    {
        yield return null;
    }
    //=================================================

    //====================Swap=========================
    private void SwapChess(int ID) {
        //For animation
        //StartCoroutine(Swap(ID));
        int[] currentObj = GetTypeByObj(gameObject);
        int[] targetObj = GetTypeByObj(GridManager.GetGridByID(ID).Chess);
        SetTypeByObj(gameObject, (TypeList) targetObj[0]);
        SetTypeByObj(GridManager.GetGridByID(ID).Chess, (TypeList)currentObj[0]);
        UpdatePos();
        StageController.actionCount++;
    }
    IEnumerator Swap(int ID)
    {
        yield return null;
    }
    //=================================================

    //====================Suicide========================
    private void SuicideChess() {
        //For animation
        //StartCoroutine(Suicide());
        int[] elementData = GetTypeByObj(gameObject);
        SetTypeByObj(gameObject, Extensions.GetStrongerType((TypeList)elementData[0]));
        UpdatePos();
        StageController.actionCount++;
    }
    IEnumerator Suicide()
    {

        //GridData data = GridManager.GetGridByID(GridManager.GetIDbyPos(transform.position));
        //Debug.Log("Suicide ID: " + GridManager.GetIDbyPos(transform.position));
        int[] elementData = GetTypeByObj(gameObject);
        SetTypeByObj(gameObject, Extensions.GetStrongerType((TypeList)elementData[0]));
        yield return null;
    }
    //===================================================


    //===================Move============================
    public void MoveChess(int ID)
    {
        StartCoroutine(Move(ID));
        StageController.actionCount++;
        Debug.Log("Current count: " + StageController.actionCount);
    }
    IEnumerator Move(int ID)
    {
        float timer = 0f;
        Vector3 dest = GridManager.GetPosbyID(ID);
        //Debug.Log("DEST: " + dest);
        posBe4Move = gameObject.transform.position;
        dest.z = 1;
        //transform.position = new Vector3(GridManager.GetPosbyID(ID).x, GridManager.GetPosbyID(ID).y, 1);
        while (transform.position != dest)
            while (transform.position != dest)
        {
            timer += Time.deltaTime;
            float step = speed*timer;
            //Debug.Log("step: " + step);
            transform.position = Vector3.MoveTowards(transform.position, dest, step);
            yield return null;
        }
        transform.position = dest;
        OnMouseExit();
        isMove = false;
        UpdatePos();
        //isMove = true;
    }
    //=====================================================

    //=================Variables Calculate=====================
    public int[] CheckPos()
    {
        int[] result = { 0 };
        
        return result;
    }
    private  void UpdatePos()
    {
        GridManager.SetChessByID(null, GridManager.GetIDbyPos(posBe4Move));
        for (int i = 0; i < GridManager.map.Length; i++)
        {
            if (transform.position == GridManager.GetGridByID(i).Pos)
            {
                GridManager.SetChessByID(gameObject, i);
                //Debug.Log(i + ":" + GridManager.GetGridByID(i).Chess.name);
            }
        }
        //Debug
        //for (int i = 0; i < GridManager.map.Length; i++)
        //{
        //    //Debug.Log(GridManager.GetGridByID(i).Chess.name);
        //    if (GridManager.GetGridByID(i).Chess.name == null)
        //    {
        //        GridManager.GetGridByID(i).Chess.name = "none";
        //    }
        //    Debug.Log(i + ":" + GridManager.GetGridByID(i).Chess.name);
        //}

    }
    /*GetTypeByObj
     * @param   obj:    target object
     * @desc      return the appropriate int value depends on object's element type and owner
     *                 red:        0
     *                 green:   1
     *                 brown:  2
     *                 blue:     3
     *                 player1 10
     *                 player2 11
     *                 player3 12
     *                 player4 13
     * @return  (player,type)
     */
    public static int[] GetTypeByObj(GameObject obj)
    {
        int[] result = new int[2];
        foreach (Transform t in obj.transform)
        {
            if (t.gameObject.activeInHierarchy)
            {
                //Debug.Log("Active obj: "+t.name);
                GameObject dummy = t.gameObject;
                for (int i = 0; i < eData.Length; i++)
                {
                    if (eData[i].Name == dummy.tag)
                    {
                        if ((int)eData[i].Etype < 10)
                        {
                            result[0] = (int)eData[i].Etype;
                            //Debug.Log("Found Element: " + eData[i].Etype);
                        }
                        else
                        {
                            result[1] = (int)eData[i].Etype;
                            //Debug.Log("Found Owner: " + eData[i].Etype);
                        }
                    }
                }
            }
        }
        //Debug.Log("result: " + result[0]);
        //Debug.Log("result: " + result[1]);
        return result;
    }
    public static void SetTypeByObj(GameObject obj, TypeList element)
    {
        string eTag = "";
        for (int i = 0; i < eData.Length; i++)
        {
            if (eData[i].Etype == element)
            {
                eTag = eData[i].Name;
            }
        }
        //Debug.Log("Tag to activate: " + eTag);
        foreach (Transform t in obj.transform)
        {
            if (t.gameObject.activeInHierarchy)
            {
                GameObject dummy = t.gameObject;
                for (int i = 0; i < eData.Length; i++)
                {
                    if (eData[i].Name == dummy.tag)
                    {
                        if ((int)eData[i].Etype < 10)
                        {
                            t.gameObject.SetActive(false);
                            //Debug.Log("Deactivate " + t.gameObject.tag);
                            //Debug.Log("Found Element: " + eData[i].Etype);
                        }
                    }
                }
            }
            else
            {
                if(t.gameObject.tag == eTag)
                {
                    t.gameObject.SetActive(true);
                    //Debug.Log("Activate" + t.gameObject.tag);
                }
            }
        }
    }
    /*GetOptionByTitle
     * 
     */
     public SelectMenu GetOptionByTitle(string title)
    {
        for(int i = 0; i < options.Length; i++)
        {
            if (options[i].title == title) return options[i];
        }
        return null;
    }
    //===================================================
}
