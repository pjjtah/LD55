using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Object
{
    // 0 up, 1 right, 2 down, 3 left
    public int lastDirectionMoved;
    public Animator animator;
    public SpriteRenderer sp;

    void Start()
    {
        // Get the Animator component attached to the same GameObject
        animator = GetComponent<Animator>();
        sp = GetComponentInChildren<SpriteRenderer>();
    }

    internal void Fall(Vector3 targetPosition)
    {
        transform.position = targetPosition;
        animator.SetTrigger("death");
    }
}
