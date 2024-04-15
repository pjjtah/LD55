using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Imp : Enemy
{
    public bool dead = false;
    public AudioSource impSound;
    public override void MoveLogic(Player player)
    {
        if (!dead)
        {
            switch (player.lastDirectionMoved)
            {
                case 0:
                    targetPosition = new Vector3(position.x, position.y - 1, position.z);
                    break;
                case 1:
                    targetPosition = new Vector3(position.x + 1, position.y, position.z);
                    break;
                case 2:
                    targetPosition = new Vector3(position.x, position.y + 1, position.z);
                    break;
                case 3:
                    targetPosition = new Vector3(position.x - 1, position.y, position.z);
                    break;
            }
        }

    }

    internal void Fall(Vector3 targetPosition)
    {
        transform.position = targetPosition;
        animator.SetTrigger("death");
        dead = true;
    }
}
