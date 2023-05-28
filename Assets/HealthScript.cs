using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HealthScript : MonoBehaviour, IDamagable
{
    public float maxHealth = 100;
    public float health;

    public Action onHit;

    public void Damage(float dmgAmount, Action killCallback)
    {
        if (health<=0) return;
        health-=dmgAmount;
        if (onHit != null) onHit();
        if (health<=0){
            Kill();
            if (killCallback != null) killCallback();
            
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
