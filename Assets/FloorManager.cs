using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum RoomType {
    Empty = 0,
    NormalRoom = 1,
    TreasureRoom = 2,
    BossRoom = 3,
    StartRoom = 4
}

public class FloorManager : MonoBehaviour
{

    // 0 - empty, 1 - normal room, 2 - treasure room, 3 - boss room, 4 - start room
    int[,] mapCode = {
        {4,1,0,0},
        {1,1,1,3},
        {2,0,1,0},
    };

     

    [SerializeField] Room startRoom; 
    Room currentRoom;
    public DoorScript LRdoorPrefab;
    public DoorScript TDdoorPrefab;

    Dictionary<(int x, int y),Room> enteredRooms = new Dictionary<(int x, int y), Room>();

    // 0 = góra, 1 = lewo, 2 = dół, 3 = prawo

    public Dictionary<int, bool> getDoors(){
        var id = currentRoom.id;

        Dictionary<int, bool> doorDict = new Dictionary<int, bool>(){
            [0] = false, [1] = false, [2] = false, [3] = false,
        };

        if (id.y-1 >= 0 && mapCode[id.y-1,id.x] != 0){
            doorDict[0]=true;
        }

        if (id.x-1 >= 0 && mapCode[id.y,id.x-1] != 0){
            doorDict[1]=true;
        }
        // print($"EEE [{id.y},{id.x}]");
        // print($"YYY {mapCode[id.y,id.x+1]}");
        if (mapCode.GetLength(0) > id.y+1 && mapCode[id.y+1,id.x] != 0){
            doorDict[2]=true;
        }

        if (mapCode.GetLength(1) > id.x+1 && mapCode[id.y,id.x+1] != 0){
            doorDict[3]=true;
        }

        return doorDict;
    }

    void generateRoom((int y, int x) id){
        currentRoom = new Room(id,getRoomType(id));
        enteredRooms[id]=currentRoom;
        currentRoom.Spawn(true);
    }

    // 0 = góra, 1 = lewo, 2 = dół, 3 = prawo
    public void OnDoorEnter(int side){
        var currentId = currentRoom.id;
        var lewoprawo = side % 2 != 0? side-2 : 0;
        var goradol = side % 2 == 0 ? side-1 : 0;
        PlayerEntity.Instance.transform.position = Vector2.zero + new Vector2(lewoprawo*-6f,goradol*2.5f);
        (int y, int x) t = (currentId.y+goradol,currentId.x+lewoprawo);
        enterRoom(t);
    }
    void enterRoom((int y, int x) roomId){
        if (currentRoom != null){
            currentRoom.Clear();
        }
        if (!enteredRooms.ContainsKey(roomId)){
            generateRoom(roomId);
            return;
        }
        currentRoom = enteredRooms[roomId];
        currentRoom.Spawn(false);
    }
    RoomType getRoomType((int y, int x) id){
        return (RoomType) Enum.GetValues(typeof(RoomType)).GetValue(mapCode[id.y,id.x]);
    }

    (int y, int x) getStartRoomIndex(){
        for(int i = 0; i < mapCode.Length; i++ ){
            for (int e = 0; e < mapCode.GetLength(1); e++){
                if (mapCode[i,e] == 4){
                    return new (i,e);
                }

            }
        }
        return new (0,0);
    }
    void Start()
    {
        generateRoom(getStartRoomIndex());
    }
    void Update()
    {
        
    }
}
