using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private BulletScript bullet;

    protected void onKill(){

    }
    protected void onBulletHit(IDamagable target, Transform hitObject){
        if (target is not null){
            target.Damage(2,onKill);
            target.transform.GetComponent<EnemyBase>().stun(0.1f);
        }
    }
    public virtual void Fire(){
        var newBullet = Instantiate<BulletScript>(bullet);
        newBullet.Fire(onBulletHit,100,transform.position,transform.rotation);
        Destroy(newBullet.gameObject,2);
        CinemachineShake.Instance.ShakeCamera(1,0.1f);

        
    }
}
