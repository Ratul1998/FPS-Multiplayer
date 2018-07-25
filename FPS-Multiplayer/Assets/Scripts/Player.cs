using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(PlayerSetup))]
public class Player : NetworkBehaviour {
    [SyncVar]
    private bool _isDead = false;
    public bool isDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }
    [SerializeField]
    private float maxHealth = 100f;

    public int kills;
    public int deaths;

    [SerializeField]
    private Behaviour[] disableOnDeath;
    private bool[] wasEnabled;

    [SerializeField]
    private GameObject[] disableGameObjectOnDeath;

    [SyncVar]
    private float currentHealth;

    [SerializeField]
    private GameObject Explosion;

    [SerializeField]
    private GameObject RespwanEffect;

    private bool firstSetup = true;

    public void PlayerSetup()
    {
        if (isLocalPlayer)
        {
            GameManagers.instance.SetSceneCameraActive(false);
            GetComponent<PlayerSetup>().PlayerUIInstance.SetActive(true);
        }
        CmdBroadCastNewPlayerSetup();
    }
    [Command]
    private void CmdBroadCastNewPlayerSetup()
    {
        RpcSetupPlayerOnAllClient();
    }

    [ClientRpc]
    private void RpcSetupPlayerOnAllClient()
    {
        if (firstSetup)
        {
            wasEnabled = new bool[disableOnDeath.Length];
            for (int i = 0; i < wasEnabled.Length; i++)
            {
                wasEnabled[i] = disableOnDeath[i].enabled;
            }
            firstSetup = false;
        }
        SetDefaults();
    }

	public void SetDefaults()
    {
        isDead = false;
        currentHealth = maxHealth;
        for(int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];
        }

        for (int i = 0; i < disableGameObjectOnDeath.Length; i++)
        {
            disableGameObjectOnDeath[i].SetActive(true);
        }

        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = true;
        }

       

        //Create Spwan Effect
        GameObject gdxIns = (GameObject)Instantiate(RespwanEffect , transform.position, Quaternion.identity);
        Destroy(gdxIns, 2f);

    }
    [ClientRpc]

    public void RpcTakeDamage(float amt)
    {
        if (isDead)
            return;
        currentHealth -= amt;
        if (currentHealth <= 0)
        {
            Die();
            
        }
    }
    void Die()
    {
        isDead = true;
        for(int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }

        //DisAble GameObjects
        for (int i = 0; i < disableGameObjectOnDeath .Length; i++)
        {
            disableGameObjectOnDeath [i].SetActive(false);
        }

        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }

        GameObject gdxIns=(GameObject) Instantiate(Explosion, transform.position, Quaternion.identity);
        Destroy(gdxIns, 2f);

        if (isLocalPlayer)
        {
            GameManagers.instance.SetSceneCameraActive(true);
            GetComponent<PlayerSetup>().PlayerUIInstance.SetActive(false);
        }

        //Call Respwan Method
        StartCoroutine(Respawn());

    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManagers.instance.matchSettings.respwantime);
        
        Transform _startPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = _startPoint.position;
        transform.rotation = _startPoint.rotation;

        yield return new WaitForSeconds(0.1f);

        PlayerSetup();


    }

    private void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            RpcTakeDamage(10f);
        }
    }

    public float GetHealthPct()
    {
        return (currentHealth / maxHealth);
    }

}
