using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public virtual void WalkFlip(bool flip){
        if (spriteRenderer is null) spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.flipX = flip;
    }
}
