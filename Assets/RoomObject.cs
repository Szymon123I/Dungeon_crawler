using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomObject : MonoBehaviour
{
    FloorManager floorManager;
    GameObject spawnMarker;
    Transform enemies;
    public bool doorsClosed {private set; get;} = true;
    bool spawning = false; 

    void Awake(){
        spawnMarker = Resources.Load<GameObject>("SpawnMarker");
        floorManager = GameObject.Find("FloorManager").GetComponent<FloorManager>();
        enemies = transform.Find("Enemies");
        enemies.gameObject.SetActive(false);
    }
    void Start(){
        doorClose();
    }

    // 0 = góra, 1 = lewo, 2 = dół, 3 = prawo
    public void DoorEnter(int side){
        if (!doorsClosed)
            floorManager.OnDoorEnter(side);
    }

    private void doorClose(){
        doorsClosed = true;
    }

    private void doorOpen(){
        doorsClosed = false;
    }

    IEnumerator enemySpawn(){
        yield return new WaitForSeconds(1);
        enemies.gameObject.SetActive(true);
        spawning = false;
    }

    public void OnFirstEnter(){
        spawning = true;
        foreach (EnemyBase enemy in enemies.gameObject.GetComponentsInChildren<EnemyBase>()){
            var marker = Instantiate<GameObject>(spawnMarker);
            marker.transform.position = enemy.transform.position;
        }
        StartCoroutine(enemySpawn());
        // enemies.gameObject.SetActive(true);
        
    }

    void Update(){
        if ((enemies.childCount <= 0 || (!enemies.gameObject.activeSelf && !spawning)) && doorsClosed){
            doorOpen();
        }
    }
}
