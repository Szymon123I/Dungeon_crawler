using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : MonoBehaviour
{
    public static PlayerEntity Instance {get; private set;}
    public HealthScript healthScript;
    public InputScript inputScript;
    public MovementScript movementScript;
    void Awake()
    {
        Instance = this;
    }
    void Start(){
        healthScript = GetComponent<HealthScript>();
        inputScript = GetComponent<InputScript>();
        movementScript = GetComponent<MovementScript>(); 
    }
    void Update()
    {
        
    }
}
