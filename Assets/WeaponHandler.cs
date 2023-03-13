using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    
    private Transform armTransform;
    private Weapon heldWeapon;
    public void pointWeapon(Quaternion rotation){
        armTransform.rotation = rotation;
    }

    public void Fire(){
        heldWeapon.Fire();
    }

    void Start()
    {
        armTransform = transform.Find("Arm");
        heldWeapon = GetComponentInChildren<Weapon>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
