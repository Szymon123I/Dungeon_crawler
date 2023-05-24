using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private BulletScript bullet;
    public bool IsAutomatic;

    public float FireRate = 0.1f;
    private float fireCooldown = 0;
    protected void onKill(){

    }
    protected void onBulletHit(IDamagable target, Transform hitObject){
        if (target is not null){
            target.Damage(10,onKill);
            target.transform.GetComponent<EnemyBase>().stun(0.05f);
        }
    }

    public virtual bool FireCheck(){ // nadpisac jezeli potrzebne sa dodatkowe warunki do strzalu
        return true;
    }

    public void _fire(){
        if (fireCooldown > 0) return;
        fireCooldown = FireRate;
        if (!FireCheck()) return;
        Fire();
    }
    public virtual void Fire(){
        
        var newBullet = Instantiate<BulletScript>(bullet);
        newBullet.Fire(onBulletHit,25,transform.position,transform.rotation * Quaternion.Euler(0,0,Random.Range(-10,10)));
        Destroy(newBullet.gameObject,2);
        CinemachineShake.Instance.ShakeCamera(1,0.1f);

        
    }
    void Update(){
        fireCooldown-=Time.deltaTime;
    }
}
