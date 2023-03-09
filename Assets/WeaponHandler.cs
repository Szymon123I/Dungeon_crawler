using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    
    private Transform armTransform;
    
    public void pointWeapon(Quaternion rotation){
        armTransform.rotation = rotation;
    }


    void Start()
    {
        armTransform = transform.Find("Arm");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
