using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public int doorSide = 3;
    RoomObject roomObject;
    [SerializeField] Sprite openSprite;
    [SerializeField] Sprite closedSprite;
    SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        roomObject = GetComponentInParent<RoomObject>();
    }
    
    void PlayerEnter(){
        roomObject.DoorEnter(doorSide);
    }
    void Update()
    {
        if (!roomObject.doorsClosed){
            spriteRenderer.sprite = openSprite;
        }else{
            spriteRenderer.sprite = closedSprite;
        }
        var lewoprawo = doorSide % 2 != 0? doorSide-2 : 0;
        var goradol = doorSide % 2 == 0 ? doorSide-1 : 0;
        if ((
            PlayerEntity.Instance.transform.position-transform.position
            +new Vector3(0,goradol == -1? 0.5f : 0)
            ).sqrMagnitude <= 1f){
            PlayerEnter();
        }
    }
}
