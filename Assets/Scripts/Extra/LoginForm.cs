using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LoginForm : MonoBehaviour
{
    List<Account> accountList;

    public TextMeshProUGUI errorText;
    public TMP_InputField ILogin;
    public TMP_InputField IPassword;

    //[SerializeField]
    private AccountInfo acc;

    private void Start()
    {
        accountList = new List<Account>();
        accountList.Add(new Account("admin", "123"));
        errorText.SetText("");
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void Login()
    {
        //string sLogin = ILogin.text.ToLower();
        //if(sLogin == "") { errorText.SetText("Please enter login ID!"); return; }
        //string sPassword = IPassword.text;
        //if(sPassword == "") { errorText.SetText("Please enter password!"); return; }
        //Debug.Log("Pass: " + sPassword);
        //for (int i = 0; i < accountList.Count; i++)
        //{
        //    if(sLogin == accountList[i].Name)
        //    {
        //        if (sPassword == accountList[i].Password)
        //        {
        //            acc.curentAcc = accountList[i];
        StartCoroutine(LoadPickScene());
        //        }               
        //    }            
        //}
        //errorText.SetText("ID or password is incorrect!");

    }
    IEnumerator LoadPickScene()
    {
        yield return null;
        SceneManager.LoadScene("PickFactionScene");
    }
    public void ResetError()
    {
        errorText.SetText("");
    }
}
