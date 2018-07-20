using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour {
    [SerializeField]
    Behaviour[] componentsToDisable;

    [SerializeField]
    string RemoteLayerName = "RemotePlayer";

    

    [SerializeField]
    GameObject playerGraphics;

    [SerializeField]
    GameObject PlayerUI;

    [HideInInspector]
    public GameObject PlayerUIInstance;

    void Start()
    {
        if (!isLocalPlayer)
        {
            DisabedComponents();
            AssignRemoteLayer();
        }
        else
        {
            //DisablePlayer Graphics 
            SetLayerRecursively(playerGraphics);

            //Create PlayerUI
            PlayerUIInstance= Instantiate(PlayerUI);
            PlayerUIInstance.name = PlayerUI.name;

            //Configure PlayerUI
            PlayerUI ui = PlayerUIInstance.GetComponent<PlayerUI>();
            ui.SetController(GetComponent<PlayerController>());


            GetComponent<Player>().PlayerSetup();
        }

    }

    void SetLayerRecursively(GameObject obj)
    {
        obj.layer = 10;
        foreach(Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject);
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        string _netId = GetComponent<NetworkIdentity>().netId.ToString();
        Player _player = GetComponent<Player>();

        GameManager.RegisterPlayer(_netId, _player);
    }
    void AssignRemoteLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(RemoteLayerName);
    }

    void DisabedComponents()
    {
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }
    }


    void OnDisable()
    {
        Destroy(PlayerUIInstance);
        if(isLocalPlayer)
            GameManager.instance.SetSceneCameraActive(true);

        GameManager.UnRegisterPlayer(transform.name);
    }
}
