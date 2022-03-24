using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Result : MonoBehaviour
{
    public TextMeshProUGUI ResultText;
    private void Awake()
    {
        ResultText.SetText(ResultHolder.Result);
    }
}
