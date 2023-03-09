using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    private float speed = 10;
    private Rigidbody2D rig;

    public void Move(Vector2 position){
        rig.MovePosition(transform.position+(Vector3) position*speed);
    }

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
