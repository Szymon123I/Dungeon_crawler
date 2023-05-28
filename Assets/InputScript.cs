using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputScript : MonoBehaviour
{
    private bool held;
    private Vector2 walkDirection = Vector2.zero;
    private MovementScript movementScript;
    private WeaponHandler weaponHandler;
     public bool isWalking {get; private set;}

    private Vector2 mouseWorldPos;
    void Start()
    {
        weaponHandler = GetComponent<WeaponHandler>();
        movementScript = GetComponent<MovementScript>();
    }

    void MouseClick(){
        weaponHandler.Fire();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = new Vector3(Input.mousePosition.x,Input.mousePosition.y, 0);
        mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
        walkDirection = new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical")).normalized;      

        float AngleRad = Mathf.Atan2(mouseWorldPos.y - this.transform.position.y, mouseWorldPos.x - this.transform.position.x);

        float AngleDeg = (180 / Mathf.PI) * AngleRad;
        weaponHandler.pointWeapon(Quaternion.Euler(0, 0, AngleDeg), mouseWorldPos);
        if (Input.GetMouseButtonDown(0)){
            MouseClick();
            held = true;
        }
        if (Input.GetMouseButtonUp(0)){
            held = false;
        }
        if (held){
            weaponHandler.Fire(held);
        }
    }

    void FixedUpdate(){
        isWalking = walkDirection.magnitude > 0? true : false;
        movementScript.Move(walkDirection*Time.deltaTime);
    }
}
