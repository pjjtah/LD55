using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : Object
{
    public Animator animator;
    public SpriteRenderer sp;
    public bool fallen = false;
    public int index;

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
