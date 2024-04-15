using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TM : Enemy
{
    public bool activated = false;
    public Sprite activateSprite;
    public Sprite deactivateSprite;
    public SpriteRenderer[] laserSprites;
    public Box[] boxes;
    public bool kills = false;
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
                float d = player.position.y - position.y;
            kills = true;
                foreach (Box b in boxes)
                {
                if (!b.fallen)
                {
                    if (b.position.x == position.x)
                    {
                        if (d > 0)
                        {
                            if (b.position.y < player.position.y && b.position.y > position.y)
                            {
                                d = b.position.y - position.y + 1;
                                kills = false;
                            }
                        }
                        else
                        {
                            if (b.position.y > player.position.y && b.position.y < position.y)
                            {
                                d = b.position.y - position.y + 1;
                                kills = false;
                            }
                        }
                    }
                }

                }
                int distanceFromPlayer = (int) Mathf.Abs(d);
                for (int i = 0; i < distanceFromPlayer; i++)
                {
                    Vector3 dest = new Vector3(position.x, position.y + d + i, position.z);
                    SpriteRenderer l = laserSprites[i];
                    l.enabled = true;
                    l.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                    l.transform.position = dest;
                }
                activated = true;
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
                foreach(SpriteRenderer s in laserSprites)
                {
                    s.enabled = false;
                }
                activated = false;
                sp.sprite = deactivateSprite;
            }
        }
    }
}
