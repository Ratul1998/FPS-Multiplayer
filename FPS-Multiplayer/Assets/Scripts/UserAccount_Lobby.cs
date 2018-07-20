using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UserAccount_Lobby : MonoBehaviour {

    public Text usernameText;
    private void Start()
    {
        usernameText.text = DCF_DemoScene_ManagerScript_CSharp.playerUsername;
    }

    public void LogOut()
    {
        SceneManager.LoadScene(0);
    }
}
