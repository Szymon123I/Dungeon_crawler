using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public int doorSide = 3;
    RoomObject roomObject;
    void Start()
    {
        roomObject = GetComponentInParent<RoomObject>();
    }
    
    void PlayerEnter(){
        roomObject.DoorEnter(doorSide);
    }
    void Update()
    {
        if ((PlayerEntity.Instance.transform.position-transform.position).sqrMagnitude <= 1){
            PlayerEnter();
        }
    }
}
