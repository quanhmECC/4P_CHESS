using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialMenuScript : MonoBehaviour
{
    public RadialButton buttonPrefab;
    public RadialButton selected;
    public Text label;

    private ActionList selectedAction;
    private int[] surroundType;
    private ChessScript currentChess;

    [SerializeField]
    private ChessScript.SelectMenu[] ChessSprite;



    // Start is called before the first frame update
    public void SpawnButtons(ChessScript obj)
    {
        currentChess = obj;
        StartCoroutine(AnimateButtons(obj));

        //Debug.Log("Spawn button "+obj.options.Length);

    }
    //Appear 1 by 1
    IEnumerator AnimateButtons(ChessScript obj)
    {
        surroundType = StageController.GetSurroundDataByObj(obj.gameObject);
        //Debug.Log("Surround: " + surroundType.Length);
        for (int i = 0; i < ChessScript.surroundData.Length; i++)
        {
            if (ChessScript.surroundData[i].SMenu == null) continue;
            RadialButton newButton = Instantiate(buttonPrefab) as RadialButton;
            newButton.transform.SetParent(transform, false);
            
            //newButton.transform.localPosition = new Vector3(i*64.0f, 0f, 0f);
            Vector3 pos = ChessScript.surroundData[i].Pos - obj.transform.position;
            //Debug.Log("pos: "+pos);
            newButton.transform.localPosition = new Vector3(pos.x * GridManager.GridPixelSizeX, pos.y * GridManager.GridPixelSizeY, 0);
            newButton.icon.sprite = ChessScript.surroundData[i].SMenu.sprite;
            newButton.title = ChessScript.surroundData[i].SMenu.title;
            newButton.myMenu = this;
            yield return null;
        }
    }

    public void SpawnOption(int ID)
    {
        StartCoroutine(ChooseElement(ID));
    }
    IEnumerator ChooseElement(int ID)
    {
        for(int i = 0; i < ChessScript.surroundData.Length; i++)
        {
            if (ChessScript.surroundData[i].SMenu == null) continue;
            RadialButton newButton = Instantiate(buttonPrefab) as RadialButton;
            newButton.transform.SetParent(transform, false);

            //newButton.transform.localPosition = new Vector3(i*64.0f, 0f, 0f);
            Vector3 pos = ChessScript.surroundData[i].Pos - GridManager.GetPosbyID(ID);
            //Debug.Log("pos: "+pos);
            newButton.transform.localPosition = new Vector3(pos.x * GridManager.GridPixelSizeX, pos.y * GridManager.GridPixelSizeY, 0);
            //Replace with chess sprite
            newButton.icon.color = ChessSprite[i].color;
            newButton.icon.sprite = ChessSprite[i].sprite;
            newButton.title = ChessSprite[i].title;
            newButton.myMenu = this;
            yield return null;
        }

    }


    public void CallAction()
    {
        //Debug.Log("calling...");
        Vector3 pos = Camera.main.ScreenToWorldPoint(selected.transform.position);
        //Debug.Log("pos: " + pos);
        int ID = GridManager.GetIDbyPos(pos);
        //Debug.Log("ID in call: " + ID);
        if (ID>=0)
        currentChess.Act(selectedAction, ID);
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (selected)
            {
                //Debug.Log(selected.title+" is selected!");
                switch (selected.title)
                {
                    case "Move":
                        selectedAction = ActionList.Move;                        
                        break;
                    case "Attack":
                        selectedAction = ActionList.Attack;
                        break;
                    case "Suicide":
                        selectedAction = ActionList.Suicide;
                        break;
                    case "Swap":
                        selectedAction = ActionList.Swap;
                        break;
                    case "Summon":
                        selectedAction = ActionList.Summon;
                        break;
                    default:
                        selectedAction = ActionList.Idle;
                        break;
                }
                CallAction();
    
            }
            Destroy(gameObject);
        }
    }


}