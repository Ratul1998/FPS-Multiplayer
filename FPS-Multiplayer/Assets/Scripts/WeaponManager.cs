using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponManager : NetworkBehaviour {
    [SerializeField]
    private PlayerWeapons PrimaryWeapon;

    private PlayerWeapons currentWeapon;

    [SerializeField]
    private Transform WeaponHolder;

    private WeaponGraphics currentGraphics;

    void Start()
    {
        EquipWeapon(PrimaryWeapon);
    }

    void EquipWeapon(PlayerWeapons _weapon)
    {
        currentWeapon = _weapon;
        GameObject _weaponsIns = Instantiate(currentWeapon.graphics, WeaponHolder.position, WeaponHolder.rotation);
        _weaponsIns.transform.SetParent(WeaponHolder);
        currentGraphics = GetComponent<WeaponGraphics>();
        if (isLocalPlayer)
            Utility.SetLayerRecursively(_weaponsIns, 11);


    }

    public PlayerWeapons GetCurrentWeapon()
    {
        return currentWeapon;
    }

    public WeaponGraphics GetCurrentGraphics()
    {
        return currentGraphics;
    }


}
