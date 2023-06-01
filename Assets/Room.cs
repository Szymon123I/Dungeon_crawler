using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Room
{
    RoomObject roomLayout;
    RoomObject currentRoomObject;
    FloorManager floorManager;
    public RoomObject[] layouts;
    public (int y, int x) id {get; private set;}
    private RoomObject getRandomLayout(){
        return layouts[UnityEngine.Random.Range(0,layouts.Length)];
    }
    public Room((int y, int x) _id,RoomType roomType) {
        id = _id;
        floorManager = GameObject.Find("FloorManager").GetComponent<FloorManager>();
        layouts = Resources.LoadAll<RoomObject>($"Rooms/{Enum.GetName(typeof(RoomType), roomType)}");
        roomLayout = getRandomLayout(); 
        
    } 

    public void Clear(){
        UnityEngine.Object.Destroy(currentRoomObject.gameObject);
    }

    public void Spawn(bool firstEnter){
       
        currentRoomObject = GameObject.Instantiate<RoomObject>(roomLayout);
        currentRoomObject.transform.position = Vector2.zero;
        foreach (var r in floorManager.getDoors()){
            if (r.Value == false) continue;
            var side = r.Key;
            var lewoprawo = side % 2 != 0? side-2 : 0;
            var goradol = side % 2 == 0 ? side-1 : 0;
            DoorScript door;
            float doorOffset = 0;
            if (lewoprawo != 0){
                doorOffset = 0.5f;
                door = MonoBehaviour.Instantiate<DoorScript>(floorManager.LRdoorPrefab);
            }else{
                doorOffset = goradol == -1? 1f : 0;
                door = MonoBehaviour.Instantiate<DoorScript>(floorManager.TDdoorPrefab);
            }
            door.transform.position = Vector2.zero + new Vector2(lewoprawo*8.5f,goradol*-4f+doorOffset);
            door.transform.SetParent(currentRoomObject.transform);
            door.doorSide = side;
        }

        

        if (firstEnter){
            currentRoomObject.OnFirstEnter();
            GameObject.Find("Grid").transform.GetComponent<Grid>().CreateGrid();
        }
        
    }

}
