using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickFactionMenu : MonoBehaviour
{
    public TextMeshProUGUI cTime;
    private void Update()
    {
        if(SelectCard.allPicked)
        cTime.SetText("Start in \n"+((int)SelectCard.countdown).ToString());
    }
}
