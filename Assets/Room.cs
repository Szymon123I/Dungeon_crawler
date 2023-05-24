using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Room
{
    RoomObject roomLayout;
    RoomObject currentRoomObject;
    public RoomObject[] layouts;
    public int[] id {get; private set;}
    private RoomObject getRandomLayout(){
        return layouts[UnityEngine.Random.Range(0,layouts.Length)];
    }
    public Room(int[] _id,RoomType roomType) {
        id = _id;
        layouts = Resources.LoadAll<RoomObject>($"Rooms/{Enum.GetName(typeof(RoomType), roomType)}");
        roomLayout = getRandomLayout(); 
        
    } 

    public void Clear(){
        UnityEngine.Object.Destroy(currentRoomObject.gameObject);
    }

    public void Spawn(bool firstEnter){
       
        currentRoomObject = GameObject.Instantiate<RoomObject>(roomLayout);
        currentRoomObject.transform.position = Vector2.zero;
        if (firstEnter){
            currentRoomObject.OnFirstEnter();
            GameObject.Find("Grid").transform.GetComponent<Grid>().CreateGrid();
        }
    }

}
