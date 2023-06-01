using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : Entity
{
    [HideInInspector]
    public Unit enemyMovementScript;
    protected float distance = 999;
    protected IDamagable target;
    protected HealthScript healthScript;
    protected Animator animator;
    private bool isWalking;
    void Start()
    {
        target = GameObject.Find("Player").GetComponent<IDamagable>();
        healthScript = GetComponent<HealthScript>();
        enemyMovementScript = GetComponent<Unit>();
        enemyMovementScript.setTarget(target.transform);
        animator = GetComponent<Animator>();
    }

    public void SetIsWalking(bool _isWalking){
        isWalking = _isWalking;
    }

    public override void WalkFlip(bool flip)
    {
        transform.localScale = new Vector2(flip? -1 : 1 ,1);
    }

    protected float attackTimer = 0;
    protected float stunTimer = 0;
    virtual protected bool attackCheck(){
        return distance<1.5f;
    }

    virtual protected void killCallback(){
        target = null;
    }

    virtual protected IEnumerator Attack(){
        if (attackTimer > 0) yield break;
        animator.SetTrigger("attack");
        attackTimer = 0.5f;
        stunTimer += 0.75f;
        yield return new WaitForSeconds(0.35f);
        if (attackCheck()) {
            target.Damage(25,killCallback);
        }
        
        yield break;
    }
    public void stun(float _time){
        stunTimer+=_time;
    }
    virtual protected void distanceEnter(){
        if (attackCheck()){
            StartCoroutine(Attack());
        }
        enemyMovementScript.setCanMove(stunTimer<=0 && !attackCheck());
    }
    
    void Update(){
        animator.SetBool("walking",isWalking);
        if (target != null){
            distance = (target.transform.position-transform.position).sqrMagnitude;
        }
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
        
        distanceEnter();
    }

}
