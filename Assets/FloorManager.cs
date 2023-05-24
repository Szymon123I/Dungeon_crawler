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

    Dictionary<int[],Room> enteredRooms = new Dictionary<int[], Room>();



    void generateRoom(int[] id){
        currentRoom = new Room(id,getRoomType(id));
        enteredRooms[id]=currentRoom;
        currentRoom.Spawn(true);
    }

    // 0 = góra, 1 = lewo, 2 = dół, 3 = prawo
    public void OnDoorEnter(int side){
        var currentId = currentRoom.id;
        var lewoprawo = side % 2 != 0? side-2 : 0;
        var goradol = side % 2 == 0 ? side-1 : 0;
        PlayerEntity.Instance.transform.position = Vector2.zero + new Vector2(lewoprawo*-7f,goradol*3f);
        enterRoom(new int[]{
            currentId[0]+lewoprawo,
            currentId[1]+goradol
        });
    }
    void enterRoom(int[] roomId){
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
    RoomType getRoomType(int[] id){
        return (RoomType) Enum.GetValues(typeof(RoomType)).GetValue(mapCode[id[0],id[1]]);
    }

    int[] getStartRoomIndex(){
        for(int i = 0; i < mapCode.Length; i++ ){
            for (int e = 0; e < mapCode.GetLength(1); e++){
                if (mapCode[i,e] == 4){
                    return new int[]{i,e};
                }

            }
        }
        return new int[]{0,0};
    }
    void Start()
    {
        generateRoom(getStartRoomIndex());
    }
    void Update()
    {
        
    }
}
