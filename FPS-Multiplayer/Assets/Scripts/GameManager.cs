using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;



public class GameManager : NetworkBehaviour {

    public static GameManager instance;
    public MatchSettings matchSettings;

    [SerializeField]
    private GameObject sceenCamera;

    #region Player Tracking
    private const string PLAYER_ID_PREFIX = "Player";

    private static Dictionary<string, Player> players = new Dictionary<string, Player>();
	
    public static void RegisterPlayer(string _netID,Player _player)
    {
        string _playerId = PLAYER_ID_PREFIX + _netID;
        players.Add(_playerId, _player);
        _player.transform.name = _playerId;
    }

    public static void UnRegisterPlayer(string _playerId)
    {
        players.Remove(_playerId);
    }

    public static Player GetPlayer(string _playerID)
    {
        return players[_playerID];
    }

    //void OnGUI()
    //{    
    //    GUILayout.BeginArea(new Rect(200, 200, 200, 500));
    //    GUILayout.BeginVertical();
    //
    //    foreach(string _playerId in players.Keys)
    //  {
    //     GUILayout.Label(_playerId + " - " + players[_playerId].transform.name);
    //  }

    //  GUILayout.EndVertical();
    //  GUILayout.EndArea();
    //}
    #endregion

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More Then One Game Manager");

        }
        else
        {
            instance = this;
        }
    }

    public void SetSceneCameraActive(bool isActive)
    {
        if (sceenCamera == null)
            return;

        sceenCamera.SetActive(isActive);
    }
}

