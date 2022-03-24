using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickedFaction : MonoBehaviour
{
    public static List<int> selectedFaction;
    private void Awake()
    {
        selectedFaction = new List<int>();
        //Dummy
        selectedFaction.Add(-1);
        DontDestroyOnLoad(this.gameObject);
    }
}
