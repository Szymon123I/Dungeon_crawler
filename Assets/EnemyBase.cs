using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [HideInInspector]
    public Unit enemyMovementScript;
    float distance = 999;
    private IDamagable target;
    private HealthScript healthScript;
    void Start()
    {
        target = GameObject.Find("Player").GetComponent<IDamagable>();
        healthScript = GetComponent<HealthScript>();
        enemyMovementScript = GetComponent<Unit>();
        enemyMovementScript.setTarget(target.transform);
    }

    private float attackTimer = 0;
    private float stunTimer = 0;
    protected bool attackCheck(){
        return distance<3;
    }

    protected void killCallback(){
        target = null;
    }

    protected IEnumerator Attack(){
        if (attackTimer > 0) yield break;
        attackTimer = 0.5f;
        stunTimer += 0.25f;
        yield return new WaitForSeconds(0.25f);
        if (attackCheck()) {
            target.Damage(25,killCallback);
        }
        
        yield break;
    }
    public void stun(float _time){
        stunTimer+=_time;
    }
    protected void distanceEnter(){
        if (attackCheck()){
            StartCoroutine(Attack());
        }
        enemyMovementScript.setCanMove(stunTimer<=0 && !attackCheck());
    }
    
    void Update(){
        enemyMovementScript.setCanMove(stunTimer<=0);
        if (stunTimer>0){
            stunTimer-=Time.deltaTime;
            return;
        }else stunTimer=0;
        attackTimer-=Time.deltaTime;
        if (target == null){
            distance = 999;
            return;
        }
        enemyMovementScript.setTarget(target.transform);
        distance = (target.transform.position-transform.position).sqrMagnitude;
        distanceEnter();
    }

}
