using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIController : MonoBehaviour
{
    public static TypeList currentPlayer = TypeList.player1;    
    private static float timeCountdown;
    private static int turn;
    private static float endTime = 0f;

    private float turnTime;
    private float delayTime = 3f;

    private static TypeList winningE;

    [SerializeField]
    private GameObject summonable;
    [SerializeField]
    private Button summonButton;
    [SerializeField]
    private GameObject endText;

    private List<GameObject> highlightList;

    //private static bool end = false;

    //Text handler
    public TMPro.TextMeshProUGUI[] playerChess;
    public TMPro.TextMeshProUGUI[] fieldChess;
    public TMPro.TextMeshProUGUI currentPlayertxt;
    public TMPro.TextMeshProUGUI turntxt;
    public TMPro.TextMeshProUGUI timetxt;
    //public TMPro.TextMeshProUGUI endTurntxt;

    // Start is called before the first frame update
    private void Awake()
    {
        turnTime = 30f;
        timeCountdown = 0;        
        turn = 0;
        highlightList = new List<GameObject>();
    }
    void Start()
    {
        //timeCountdown = turnTime;
        turn = 1;
        timeCountdown = turnTime;
        ViewTurn();
        //Debug.Log("current player: " + currentPlayer);

        StartCoroutine(NextTurn());

        summonButton.onClick.AddListener(HighLightSummon);

    }

    // Update is called once per frame
    void Update()
    {
        //===========Display Section==============
        ViewTime();
        //=====================================
        timeCountdown -= Time.deltaTime;
        //Debug.Log("Current time: "+(int)timeCountdown);
        if (timeCountdown <= 0) EndTurn();
        //Debug.Log("Current turn: " + turn);
        endTime -= Time.deltaTime;
        //Debug.Log("curr endtime:"+endTime);
        if(turn == 10)
        {
            switch (winningE)
            {
                case TypeList.red: ResultHolder.Result = "RED PLAYER WIN!!!"; break;
                case TypeList.blue: ResultHolder.Result = "BLUE PLAYER WIN!!!"; break;
                case TypeList.brown: ResultHolder.Result = "YELLOW PLAYER WIN!!!"; break;
                case TypeList.green: ResultHolder.Result = "GREEN PLAYER WIN!!!"; break;
                case TypeList.Base: ResultHolder.Result = "DRAW!!!"; break;
            }
            SceneManager.LoadScene("ResultScene");
        }

        //Summon

        if (Input.GetKeyDown(KeyCode.Escape)) {
            FinishSummon();
            ReEnableSummon();

        }
    }
    private void LateUpdate()
    {
        CountChess();
        if (StageController.actionCount >= 2)
        {
            StageController.actionCount = 0;
            Debug.Log("CALLING END TURN...");
            EndTurn();            
        }

        ViewCurrentPlayer();
    }
    //==================Count Chess====================
    //return highest element chess number on the field
    private void CountChess()
    {

        //Reset
        if (playerChess.Length <= 0) return;
        int[] pChess = new int[4];
        int[] fChess = new int[4];
        int max = -1;
        for (int i = 0; i < GridManager.map.Length; i++)
        {
            if (GridManager.GetGridByID(i).Chess == null) continue;
            int[] dummyTypeData = ChessScript.GetTypeByObj(GridManager.GetGridByID(i).Chess);
            switch (dummyTypeData[0])
            {
                case (int)TypeList.red:
                    fChess[(int)TypeList.red]++;
                    if (dummyTypeData[1] == (int)currentPlayer) pChess[(int)TypeList.red]++;
                    break;
                case (int)TypeList.green:
                    fChess[(int)TypeList.green]++;
                    if (dummyTypeData[1] == (int)currentPlayer) pChess[(int)TypeList.green]++;
                    break;
                case (int)TypeList.brown:
                    fChess[(int)TypeList.brown]++;
                    if (dummyTypeData[1] == (int)currentPlayer) pChess[(int)TypeList.brown]++;
                    break;
                case (int)TypeList.blue:
                    fChess[(int)TypeList.blue]++;
                    if (dummyTypeData[1] == (int)currentPlayer) pChess[(int)TypeList.blue]++;
                    break;
            }
            for(int j = 0; j < playerChess.Length; j++)
            {
                playerChess[j].text = pChess[j].ToString();
                fieldChess[j].text = fChess[j].ToString();
                if (fChess[j] > max) { max = fChess[j]; winningE = (TypeList)j; }
                else if(fChess[j] == max && winningE != (TypeList)j) { winningE = TypeList.Base; }
            }            
        }
    }
    //================================================
    //==================Show Stat======================    
    private void ViewCurrentPlayer()
    {
        int dummy = (int)currentPlayer - ((int)TypeList.player1 - 1);
        currentPlayertxt.text = dummy.ToString();
    }
    private void ViewTime()
    {        
        timetxt.text = ((int)timeCountdown).ToString();
        if (timeCountdown <= 30f) timetxt.color = Color.green;
        if (timeCountdown <= 20f) timetxt.color = Color.yellow;
        if (timeCountdown <= 10f) timetxt.color = Color.red;
    }
    private void ViewTurn()
    {
        string turnText = "TURN " + turn.ToString();
        turntxt.text = turnText.ToUpper();
    }
    //================================================
    
    public void EndTurn()
    {

        for (int i = 0; i < GridManager.map.Length; i++)
        {
            GameObject currChess = GridManager.GetGridByID(i).Chess;
            if (currChess == null) continue;
            else
            {
                currChess.GetComponent<BoxCollider2D>().enabled = false;              
            }
        }
        Debug.Log("END TURN!");
        //Change to next player
        int nextPlayer = (int)currentPlayer + 1;
        if (nextPlayer > (int)TypeList.player4)
        {
            nextPlayer = (int)TypeList.player1;
        }
        currentPlayer = (TypeList)nextPlayer;

        //Reset count down time
        ResetTime();
        //To do Next turn
        turn++;
        ViewTurn();
        endTime = Time.time + delayTime;
        StartCoroutine(IEndTurn());
        //endTurntxt.enabled = true;
        //StartPause();
        //endTurntxt.enabled = false;
        

        //yield return new WaitForSeconds(3);

    }

    IEnumerator IEndTurn() {
        endText.SetActive(true);
        for(int i =0; i < delayTime; i++)
        {
            Debug.Log("Ending..." + i);
            yield return new WaitForSeconds(1);
        }
        endText.SetActive(false);
        StartCoroutine(NextTurn());

    }

    //public void StartPause()
    //{
    //    Debug.Log("Start pause");

    //    StartCoroutine(PauseGame(3f));

    //}
    //IEnumerator PauseGame(float pauseTime)
    //{
    //    Debug.Log("while pause");
    //    Time.timeScale = 0f;
    //    float pauseEndTime = Time.realtimeSinceStartup + pauseTime;
    //    while (Time.realtimeSinceStartup < pauseEndTime)
    //    {
    //        yield return 0;
    //    }
    //    Time.timeScale = 1f;
    //    Debug.Log("Done pause");
    //    endTurntxt.enabled = false;
 
    //}
    private void ResetTime()
    {
        //Debug.Log("RESET!");
        timeCountdown = turnTime;
    }
    IEnumerator NextTurn()
    {
        Debug.Log("Waiting for next turn..." + Time.time);
        ChangePlayer();
        Debug.Log("TURN BEGIN!");
        yield return null;
    }

    public static void ChangePlayer()
    {
        Debug.Log("Changing...");
        for (int i = 0; i < GridManager.map.Length; i++)
        {
            GameObject currChess = GridManager.GetGridByID(i).Chess;
            if (currChess == null) continue;
            else
            {
                int[] currChessType = ChessScript.GetTypeByObj(currChess);
                if (currChessType[1] != (int)currentPlayer)
                {
                    currChess.GetComponent<BoxCollider2D>().enabled = false;
                }
                else
                {
                    currChess.GetComponent<BoxCollider2D>().enabled = true;
                }
            }
        }
    }

    //==================Summon==========================
    private void HighLightSummon()
    {
        Debug.Log("Summon button clicked!");
        Territory cPTerritory = GridManager.pTerritories[(int)currentPlayer % (int)TypeList.player1];
        for(int i = (int)cPTerritory.MinID.x; i <= (int)cPTerritory.MaxID.x;i++)
        {
            for(int j = (int)cPTerritory.MinID.y; j <= (int)cPTerritory.MaxID.y;j++)
            {
                int cID = i * 8 + j;
                if (GridManager.map[i, j].Chess == null)
                {
                    highlightList.Add(Instantiate(summonable, GridManager.GetPosbyID(cID), Quaternion.identity));
                    //Debug.Log("highlight created! " + highlightList.Count);
                }
            }
        }

       
    }

    private void ReEnableSummon()
    {
        summonButton.interactable = false;
        summonButton.interactable = true;
    }

    public void FinishSummon()
    {
        if (highlightList.Count == 0) return;
        for(int i = 0; i< highlightList.Count; i++)
        {
            Destroy(highlightList[i]); 
        }
        highlightList = new List<GameObject>();
    }
    //===================================================

}
