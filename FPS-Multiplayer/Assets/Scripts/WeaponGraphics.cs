using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponGraphics : NetworkBehaviour {

    public ParticleSystem muzzleFlash;
    public GameObject hitEffect;
  

    void Update()
    {
        
        PlayEffect();
    }
    void RpcShowEffect()
    {
        if (PlayerShoot.onFire)
        {
            muzzleFlash.Play();
           
        }
       
    }

    void PlayEffect()
    {
        RpcShowEffect();
    }

   
}
