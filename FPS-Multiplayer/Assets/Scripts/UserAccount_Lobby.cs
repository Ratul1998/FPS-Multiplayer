using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class UserAccount_Lobby : NetworkBehaviour {

    [SyncVar]
    public string username;

    public static UserAccount_Lobby instance;

    public Text usernameText;
    private void Start()
    {
        usernameText.text = DCF_DemoScene_ManagerScript_CSharp.playerUsername;
        username = DCF_DemoScene_ManagerScript_CSharp.playerUsername;
    }

    public void LogOut()
    {
        SceneManager.LoadScene(0);
    }
}
