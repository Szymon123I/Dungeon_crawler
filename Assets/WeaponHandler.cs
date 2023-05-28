using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    
    private Transform armTransform;
    
    private Weapon heldWeapon;
    public void pointWeapon(Quaternion rotation, Vector3 mouse){
        armTransform.rotation = rotation;
        if (mouse.x < transform.position.x){
        
            heldWeapon.GetComponentInChildren<SpriteRenderer>().transform.parent.localScale = new Vector3(1,-1,1);
        }else{
            heldWeapon.GetComponentInChildren<SpriteRenderer>().transform.parent.localScale = new Vector3(1,1,1);
        }

    }

    public void Fire(bool held = false){
        if ((!heldWeapon.IsAutomatic && !held) || (heldWeapon.IsAutomatic && held))
            heldWeapon._fire();
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
