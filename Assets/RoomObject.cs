using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomObject : MonoBehaviour
{
    FloorManager floorManager;

    void Start(){
        floorManager = GameObject.Find("FloorManager").GetComponent<FloorManager>();
    }

    // 0 = góra, 1 = lewo, 2 = dół, 3 = prawo
    public void DoorEnter(int side){
        floorManager.OnDoorEnter(side);
    }

    public void OnFirstEnter(){

    }
}
