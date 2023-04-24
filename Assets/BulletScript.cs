using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BulletScript : MonoBehaviour
{
    private float bulletSpeed;
    private Action<IDamagable, Transform> hitCallback;
    public void Fire(Action<IDamagable, Transform> _hitCallback, float _bulletSpeed, Vector2 pos, Quaternion rotation){
        bulletSpeed = _bulletSpeed;
        hitCallback = _hitCallback;
        transform.position = pos;
        transform.rotation = rotation;
    }

    protected void bulletHit(Transform objectHit, RaycastHit2D hitInfo){
        var enemBase = objectHit.GetComponent<IDamagable>();
        hitCallback(enemBase,objectHit);
        Destroy(gameObject);
    }

    protected void bulletTick(){
        int layerMask =~ LayerMask.GetMask("Player");

        RaycastHit2D result = Physics2D.Raycast(transform.position,transform.right,bulletSpeed*Time.deltaTime,layerMask);
        if (result.transform is not null){
            bulletHit(result.transform,result);
            return;
        }
        transform.position = transform.position+ transform.right*bulletSpeed*Time.deltaTime;
    }

    void Update(){
        bulletTick();
    }
}
