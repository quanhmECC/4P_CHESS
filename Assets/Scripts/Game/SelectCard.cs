using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SelectCard : MonoBehaviour
{
    public Sprite[] factionSprites;
    public Image Avatar;
    public Button button;
    public TextMeshProUGUI des;

    public static int fCount;   //selected faction
    public static float countdown;
    public static bool allPicked;

    private void Start()
    {
        fCount = 0;
        countdown = 4;
        allPicked = false;
    }
    public void Select()
    {
        int selected = -1;
        while (!checkAvailable(selected))
        {
            selected = UnityEngine.Random.Range(0, 4);
        }
        PickedFaction.selectedFaction.Add(selected);

        
        //Transform t = gameObject.transform;
        if (factionSprites == null) { Debug.Log("No Sprites to load!"); return; }
        fCount++;
        switch (selected)
        {
            //Green
            case 0:
                Avatar.sprite = factionSprites[0];
                des.SetText("Player "+(PickedFaction.selectedFaction.Count-1)+"\nGreen");
                des.color = Color.green;

                break;
                //Red
            case 1:
                Avatar.sprite = factionSprites[1];
                des.SetText("Player " + (PickedFaction.selectedFaction.Count-1) + "\nRed");
                des.color = Color.red;
                break;
                //Blue
            case 2:
                Avatar.sprite = factionSprites[2];
                des.SetText("Player " + (PickedFaction.selectedFaction.Count - 1) + "\nBlue");
                des.color = Color.blue;
                break;
                //Yellow
            case 3:
                Avatar.sprite = factionSprites[3];
                des.SetText("Player " + (PickedFaction.selectedFaction.Count - 1) + "\nYellow");
                des.color = Color.yellow;
                break;
            default:
                Debug.Log("???");
                break;
        }
        button.interactable = false;
        Animator anim = button.GetComponent<Animator>();
        anim.SetTrigger("Selected");

        if(fCount == 4) { allPicked = true; StartCoroutine(ChangeScene()); }

    }
    private bool checkAvailable(int faction)
    {
        for (int i = 0; i < PickedFaction.selectedFaction.Count; i++)
        {
            if (faction == PickedFaction.selectedFaction[i]) return false;
        }
        return true;
    }
    IEnumerator ChangeScene()
    {
        while (countdown > 1)
        {
            countdown -= Time.deltaTime;
            Debug.Log("Countdown: " + (int)countdown);
            yield return null;
        }
        LoadPlayScene();

    }
    static void LoadPlayScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
