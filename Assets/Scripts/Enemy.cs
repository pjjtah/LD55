using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Object
{
    public Animator animator;
    public SpriteRenderer sp;

    public virtual void MoveLogic(Player player)
    {

    }

    void Start()
    {
        // Get the Animator component attached to the same GameObject
        animator = GetComponent<Animator>();
    }


}
