using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TM : Enemy
{
    public bool activated = false;
    public Sprite activateSprite;
    public Sprite deactivateSprite;
    public override void MoveLogic(Player player)
    {
        if (player.position.x == position.x)
        {
            if (player.position.y > position.y)
            {
                sp.flipX = false;
                sp.transform.rotation = Quaternion.Euler(0f, 0f, 270f);
                sp.sprite = activateSprite;
            }
            else if (player.position.y < position.y)
            {
                sp.flipX = false;
                sp.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                sp.sprite = activateSprite;
            }
            if (!activated)
            {
                activated = true;
            }
        }
        else if (player.position.y == position.y)
        {
            if (player.position.x > position.x)
            {
                sp.flipX = true;
                sp.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                sp.sprite = activateSprite;
            }
            else if (player.position.x < position.x)
            {
                sp.flipX = false;
                sp.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                sp.sprite = activateSprite;
            }
            if (!activated)
            {
                activated = true;
            }
        }
        else
        {
            if (activated)
            {
                activated = false;
                sp.sprite = deactivateSprite;
            }
        }
    }
}
