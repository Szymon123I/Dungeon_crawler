using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomObject : MonoBehaviour
{
    FloorManager floorManager;
    Transform enemies;
    public bool doorsClosed {private set; get;} = true;

    void Start(){
        floorManager = GameObject.Find("FloorManager").GetComponent<FloorManager>();
        enemies = transform.Find("Enemies");
        doorClose();
    }

    // 0 = góra, 1 = lewo, 2 = dół, 3 = prawo
    public void DoorEnter(int side){
        if (!doorsClosed)
            floorManager.OnDoorEnter(side);
    }

    private void doorClose(){

    }

    private void doorOpen(){
        doorsClosed = false;
    }

    public void OnFirstEnter(){

    }

    void Update(){
        if (enemies.childCount <= 0 && doorsClosed){
            doorOpen();
        }
    }
}
