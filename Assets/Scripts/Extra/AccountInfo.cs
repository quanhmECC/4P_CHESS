using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountInfo : MonoBehaviour
{
    private Account _currentAcc;
    public Account curentAcc { get { return _currentAcc; } set { _currentAcc = value; } }
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
