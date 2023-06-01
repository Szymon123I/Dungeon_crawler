using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMarkerScript : MonoBehaviour
{
    void Start()
    {
       Destroy(gameObject,1); 
    }
    void Update(){
        transform.localScale = transform.localScale - (Vector3) Vector2.one*1f*Time.deltaTime;
        transform.Rotate(new Vector3(0,0,1), 10);
    }
}
