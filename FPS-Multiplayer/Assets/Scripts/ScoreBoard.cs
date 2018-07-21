using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBoard : MonoBehaviour {

    private void OnEnable()
    {
        Player[] players = GameManagers.GetAllPlayers();
        foreach(Player player in players)
        {
            Debug.Log(player.name);
        }
    }

    private void OnDisable()
    {
        
    }
}
