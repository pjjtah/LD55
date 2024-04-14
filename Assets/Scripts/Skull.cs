using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skull : Enemy
{
    public bool activated = false;
    public override void MoveLogic(Player player)
    {
        if(player.position.x == position.x)
        {
            if(player.position.y > position.y)
            {
                targetPosition = new Vector3(position.x, position.y + 1, position.z);
            }
            else if (player.position.y < position.y)
            {
                targetPosition = new Vector3(position.x, position.y - 1, position.z);
            }
            if (!activated)
            {
                activated = true;
                animator.SetTrigger("activate");
            }
        }
        else if (player.position.y == position.y)
        {
            if (player.position.x > position.x)
            {
                targetPosition = new Vector3(position.x + 1, position.y, position.z);
                sp.flipX = false;
            }
            else if (player.position.x < position.x)
            {
                targetPosition = new Vector3(position.x - 1, position.y, position.z);
                sp.flipX = true;
            }
            if (!activated)
            {
                activated = true;
                animator.SetTrigger("activate");
            }
        }
        else
        {
            if (activated)
            {
                activated = false;
                animator.SetTrigger("deactivate");
            }
        }
    }

}
