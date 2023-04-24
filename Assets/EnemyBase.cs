using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [HideInInspector]
    public Unit enemyMovementScript;
    void Start()
    {

        enemyMovementScript = GetComponent<Unit>();
        enemyMovementScript.setTarget(GameObject.Find("Player").transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
