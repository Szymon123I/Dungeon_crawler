using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IDamagable
{
    public Transform transform {get;}
    public void Damage(float dmgAmount, Action killCallback);
    public void Kill();
}
