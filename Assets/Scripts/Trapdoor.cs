using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trapdoor : MonoBehaviour
{
    public bool[] activated;
    public Animator animator;
    public bool open = false;
    public SpriteRenderer[] keys;
    public Sprite[] activeSprites;
    public Vector3 position;

    void Start()
    {

        // Get the Animator component attached to the same GameObject
        animator = GetComponent<Animator>();
        if (open)
        {
            animator.SetTrigger("open");
        }
    }

    public void Activate(int i)
    {
        activated[i] = true;
        keys[i].sprite = activeSprites[i];
        bool allActive = true;
        foreach(bool b in activated)
        {
            if(!b){
                allActive = false;
                break;
            }
        }
        if (allActive)
        {
            foreach(SpriteRenderer s in keys)
            {
                s.enabled = false;
            }
            animator.SetTrigger("open");
            open = true;
        }
    }
}
