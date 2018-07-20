using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class PlayerShoot : NetworkBehaviour {

    private PlayerWeapons currentWeapon;

    private WeaponManager WM;

    public GameObject hitEffect;

    public static bool onFire = false;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

    void Start()
    {
        if (cam == null)
        {
            Debug.LogError("PlayerShoot: No Camera");
            this.enabled = false;
        }

        WM = GetComponent<WeaponManager>();
    }

    void Update()
    {
        currentWeapon = WM.GetCurrentWeapon();

        if (PauseMenu.isOn)
            return;

        if (currentWeapon.fireRate <= 0f)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
                onFire = false;
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                InvokeRepeating("Shoot", 0f,1f / currentWeapon.fireRate);
            }
        }
        if (Input.GetButtonUp("Fire1"))
        {
            onFire = false;
            CancelInvoke("Shoot");
        }
            
    }
    [Command]
    void CmdShoot()
    {
        RpcDoShootEffect();
    }
    
    void RpcDoShootEffect()
    {
        onFire = true;
    }
   
    [Client]
    void Shoot()
    {
        if (!isLocalPlayer)
            return;
        CmdShoot();
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position,cam.transform.forward,out hit, currentWeapon.range, mask)) 
        {
            if (hit.collider.tag == "Player")
            {
                CmdPlayerShot(hit.collider.name, currentWeapon.damage);
            }

            CmdOnHit(hit.point, hit.normal);

        }
            
    }
    [Command]
    void CmdPlayerShot(string _Id,int damage)
    {
        Debug.Log(_Id + " has been shot");

        Player _player = GameManager.GetPlayer(_Id);
        _player.RpcTakeDamage(damage);

    }
    [Command]
    void CmdOnHit(Vector3 _pos , Vector3 _normal)
    {
        RpcDoHitEffect(_pos, _normal);
    }
    [ClientRpc]
    void RpcDoHitEffect(Vector3 _pos, Vector3 _normal)
    {
        GameObject hitIns = (GameObject)Instantiate(hitEffect, _pos,Quaternion.LookRotation(_normal));
        Destroy(hitIns, 1f);
        
    }
}
