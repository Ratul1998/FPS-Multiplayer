using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class WeaponManager : NetworkBehaviour
{

    [SerializeField]
    private string weaponLayerName = "Weapon";

    [SerializeField]
    private AudioSource[] ReloadSound;

    [SerializeField]
    private Animator[] a;

    [SerializeField]
    private Transform weaponHolder;

    [SerializeField]
    private PlayerWeapons primaryWeapon;

    private PlayerWeapons currentWeapon;

    public int Slot = 0;

    public bool isReloading = false;

    void Start()
    {
        EquipWeapon(primaryWeapon);
    }

    public PlayerWeapons GetCurrentWeapon()
    {
        return currentWeapon;
    }

    void EquipWeapon(PlayerWeapons _weapon)
    {
        currentWeapon = _weapon;

        _weapon.graphics[Slot].SetActive(true);

        

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Slot = 0;
            currentWeapon.graphics[1].SetActive(false);
            EquipWeapon(primaryWeapon);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Slot = 1;
            currentWeapon.graphics[0].SetActive(false);
            EquipWeapon(primaryWeapon);
        }


    }

    public void Reload()
    {
        if (isReloading)
            return;

        StartCoroutine(Reload_Coroutine());
    }

    private IEnumerator Reload_Coroutine()
    {

        isReloading = true;

        CmdOnReload();

        ReloadSound[Slot].Play();

        yield return new WaitForSeconds(currentWeapon.reloadTime);

        currentWeapon.bullets = currentWeapon.maxBullets;

        isReloading = false;
    }

    [Command]
    void CmdOnReload()
    {
        RpcOnReload();
    }

    [ClientRpc]
    void RpcOnReload()
    {
        a[Slot].SetTrigger("Reload");
    }

}