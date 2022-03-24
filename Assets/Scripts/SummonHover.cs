using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummonHover : MonoBehaviour
{
    private Color currentColor;
    SpriteRenderer sp;    

    private void Start()
    {
         sp = GetComponent<SpriteRenderer>();
    }

    private void OnMouseEnter()
    {
        
        currentColor = sp.color;
        sp.color = new Color(0.9207f, 0.9339f, 0.9333f, 0.4901f);
        //Debug.Log("hovered?");
    }
    private void OnMouseExit()
    {
        sp.color = currentColor;
        //Debug.Log("Exit!");
    }
    private void OnMouseDown()
    {
        Debug.Log("Summon!");
        //Summon();
        ChooseElement();

        //isSelected = true;
        //RadialMenuSpawner.mSpawner.SpawnMenu(this);
        //currentObj = gameObject;

    }
    private void Summon()
    {
        Debug.Log("Checking...");
        for (int i = 0; i < StageController.UnsummonChessList.Length; i++)
        {
            GameObject cChess = StageController.UnsummonChessList[i];
            int[] type = ChessScript.GetTypeByObj(cChess);
            //Get current player unsummoned chess piece

            if (type[1] == (int)UIController.currentPlayer)
            {                
                cChess.transform.position = transform.position;
                cChess.GetComponent<BoxCollider2D>().enabled = true;
            }
        }
    }
    private void ChooseElement()
    {

    }
}
