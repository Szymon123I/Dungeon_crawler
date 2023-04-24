using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HealthScript : MonoBehaviour, IDamagable
{
    [SerializeField]
    float maxHealth = 100;
    float health;

    public Action onHit;

    public void Damage(float dmgAmount, Action killCallback)
    {
        health-=dmgAmount;
        if (health<=0){
            Kill();
            if (killCallback is not null) killCallback();
            if (onHit is not null) onHit();
        }
    }

    public void Kill()
    {
        Destroy(gameObject);
    }
    void Start(){
        health = maxHealth;
    }
}
