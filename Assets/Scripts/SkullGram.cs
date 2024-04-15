using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullGram : MonoBehaviour
{
    public int index;
    public SpriteRenderer sp;
    public Animator animator;
    public Sprite deactivate;
    public Sprite activate;
    public Vector3 position;
    public GameObject enemy;
    public bool activated = false;
    public int enemyType = 0;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate()
    {
        activated = true;
        sp.sprite = activate;
    }
    public Enemy SpawnEnemy()
    {
        animator.SetTrigger("thunder");
        enemy.SetActive(true);
        return enemy.GetComponent<Enemy>();
    }
}
