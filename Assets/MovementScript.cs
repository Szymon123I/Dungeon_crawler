using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    [SerializeField] private float speed = 6;
    private Rigidbody2D rig;
    private Entity entity;

    public void Move(Vector2 position){
        if (position.normalized.x < 0)
            entity.WalkFlip(true);
        else if (position.normalized.x > 0)
            entity.WalkFlip(false);

        rig.MovePosition(rig.position+position*speed);
    }

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        entity = GetComponent<Entity>();
    }

    // Update is called once per frame
    void Update()
    {
        // isWalking = false;
    }
}
