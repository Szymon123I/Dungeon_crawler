using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemyBase : EnemyBase
{
    [SerializeField] protected BulletScript bullet;
    [SerializeField] protected float range = 25;

    virtual protected void onBulletHit(IDamagable hit, Transform hitTransform){
        print(transform.name);
    }
    virtual protected void Fire(){
        
        var dir = (target.transform.position-transform.position).normalized;
        var newBullet = Instantiate<BulletScript>(bullet);
        newBullet.Fire(onBulletHit,25,transform.position,Quaternion.LookRotation(dir,Vector3.forward),LayerMask.GetMask("Enemy"));
        Destroy(newBullet.gameObject,2);
        
    }
    override protected IEnumerator Attack(){
        if (attackTimer > 0) yield break;
        animator.SetTrigger("attack");
        attackTimer = 0.5f;
        stunTimer += 0.1f;
        yield return new WaitForSeconds(0.35f);
        Fire();

        yield break;
    }

    override protected bool attackCheck(){
        int layerMask =~ LayerMask.GetMask("Enemy");
        RaycastHit2D cast = Physics2D.Raycast(transform.position,(target.transform.position-transform.position).normalized,range,layerMask);
        return cast.transform != null && cast.transform.gameObject.layer == LayerMask.NameToLayer("Player");
    }
}
