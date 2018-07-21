using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour {
    public Text KillCount;
    public Text DeathCount;

    private void Start()
    {
    }
    void OnReceivedData(string data)
    {
        Debug.Log(data);
    }
}
