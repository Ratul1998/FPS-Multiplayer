using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerWeapons {


   
    public string name ="SMG";
    public int damage=20 ;
    public float range=125f ;

    public float fireRate =10f;

    public int maxBullets = 30;

    public int guncount = 0;


    [HideInInspector]
    public int bullets;

    public GameObject [] graphics;

    public float reloadTime = 2.5f;

    public PlayerWeapons()
    {
        bullets = maxBullets;
    }
    

    

}
