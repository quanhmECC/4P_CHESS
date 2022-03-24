using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//===========Enum/Struct Section====================
//============Grid================
public struct GridData
{
    string Grid_name; int GridID;
    private Vector3 pos;
    GameObject chess;
    public string Grid_name1 { get => Grid_name; set => Grid_name = value; }
    public int GridID1 { get => GridID; set => GridID = value; }
    public GameObject Chess { get => chess; set => chess = value; }
    public Vector3 Pos { get => pos; set => pos = value; }
}
//================================
//===========Direction==============
public enum SurroundDirection
{
    N = 0, S, E, W, C
}
public struct SurroundData
{
    SurroundDirection dir;
    int surroundType;
    Vector3 pos;
    GameObject SelectObj;
    ChessScript.SelectMenu sMenu;

    public SurroundData(SurroundDirection dir, int surroundType, Vector3 pos, GameObject selectObj, ChessScript.SelectMenu sMenu)
    {
        this.dir = dir;
        this.surroundType = surroundType;
        this.pos = pos;
        this.sMenu = sMenu;
        SelectObj = selectObj;
    }

    public SurroundDirection Dir { get => dir; set => dir = value; }
    public int SurroundType { get => surroundType; set => surroundType = value; }
    public Vector3 Pos { get => pos; set => pos = value; }
    public GameObject SelectObj1 { get => SelectObj; set => SelectObj = value; }
    public ChessScript.SelectMenu SMenu { get => sMenu; set => sMenu = value; }
}
//================================
//======Chess type and relation========
public enum Relation
{
    Self = -3, NoEffect, Sucidable,
    Moveable, Attackable, Swapable,
}
public enum TypeList
{
    Base = -1, red = 0, green, brown, blue, player1 = 10, player2, player3, player4,
}
public struct ElementData
{
    TypeList etype;
    string name;

    public string Name { get => name; set => name = value; }
    public TypeList Etype { get => etype; set => etype = value; }

    public ElementData(TypeList etype, string name)
    {
        this.etype = etype;
        this.name = name;
    }
}
//================================
//===========Action================
public enum ActionList
{
    Move = 0, Attack, Swap, Suicide, Summon,Idle
}
//================================
//===========Player territories========
public struct Territory
{
    //ID(col,row)
    Vector2 minID;
    Vector2 maxID;
    
    public Territory(Vector2 minID, Vector2 maxID)
    {
        this.minID = minID;
        this.maxID = maxID;
    }

    public Vector2 MinID { get => minID; set => minID = value; }
    public Vector2 MaxID { get => maxID; set => maxID = value; }
}
//================================
//=====================================================
//====================Utility class========================
public static class Extensions
{

    public static T FindComponentInChildWithTag<T>(this GameObject parent, string tag) where T:Object
    {
        Transform t = parent.transform;
        foreach (Transform tr in t)
        {
            if (tr.tag == tag)
            {
                return tr.GetComponent<T>();
            }
        }
        return default;
    }
    //public static Vector3 Movetowards()
    public static TypeList GetStrongerType(TypeList e)
    {
        Debug.Log("Weak type: " + e);
        TypeList strongerType = e;
        if (e == TypeList.red) strongerType = TypeList.blue;
        else strongerType = (TypeList)((int)e - 1);
        Debug.Log("Stronger type: " + strongerType);
        return strongerType;
    }
}
//=====================================================
//====================Accounts==========================
public struct Account
{
    private static int _ID = 0;
    string name;
    string password;
    
    public Account(string name, string password)
    {        
        ++_ID;
        this.name = name;
        this.password = password;
    }

    public int ID { get => _ID; }
    public string Name { get => name; set => name = value; }
    public string Password { get => password; set => password = value; }
}
//=====================================================