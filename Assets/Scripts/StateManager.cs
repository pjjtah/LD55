using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    PlayerMovement,
    Wait,
    LevelCompletion,
    Fail
}
public class StateManager : MonoBehaviour
{
    private GameState currentState;
    public Player player;
    public List<Enemy> enemies;
    public LevelData levelData;
    public Animator CanvasAnimator;
    private bool gameover;
    private bool complete;
    public SkullGram[] skullgrams;
    public Trapdoor trapdoor;

    void Start()
    {
        // Initialize the game with the PlayerMovement state
        currentState = GameState.PlayerMovement;
    }

    void Update()
    {
        // Update based on the current state
        switch (currentState)
        {
            case GameState.PlayerMovement:
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    StartCoroutine(WaitState());
                    player.lastDirectionMoved = 3;
                    Vector3 tarPos = new Vector3(player.position.x - 1, player.position.y, player.position.z);
                    if (CheckPossibleMovement(player, tarPos))
                    {
                        player.targetPosition = tarPos;
                        player.position = tarPos;
                        CalculateEnemies();
                        StartCoroutine(player.MoveCoroutine());
                        CheckSkullGrams(player, tarPos);
                    }
                    else if (CheckHole(player, tarPos)){
                        player.Fall(tarPos);
                        GameOver();
                        break;
                    }

                    MoveEnemies();
                }
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    StartCoroutine(WaitState());
                    player.lastDirectionMoved = 1;
                    Vector3 tarPos = new Vector3(player.position.x + 1, player.position.y, player.position.z);
                    if (CheckPossibleMovement(player, tarPos))
                    {
                        player.targetPosition = tarPos;
                        player.position = tarPos;
                        CalculateEnemies();
                        StartCoroutine(player.MoveCoroutine());
                        CheckSkullGrams(player, tarPos);
                    }
                    else if (CheckHole(player, tarPos))
                    {
                        player.Fall(tarPos);
                        GameOver();
                        break;
                    }
                    MoveEnemies();

                }
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    StartCoroutine(WaitState());
                    player.lastDirectionMoved = 0;
                    Vector3 tarPos = new Vector3(player.position.x, player.position.y + 1, player.position.z);
                    if (CheckPossibleMovement(player, tarPos))
                    {
                        player.targetPosition = tarPos;
                        player.position = tarPos;
                        CalculateEnemies();
                        StartCoroutine(player.MoveCoroutine());
                        CheckSkullGrams(player, tarPos);
                    }
                    else if (CheckHole(player, tarPos))
                    {
                        player.Fall(tarPos);
                        GameOver();
                        break;
                    }
                    MoveEnemies();

                }
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    StartCoroutine(WaitState());
                    player.lastDirectionMoved = 2;
                    Vector3 tarPos = new Vector3(player.position.x , player.position.y - 1, player.position.z);
                    if (CheckPossibleMovement(player, tarPos))
                    {
                        player.targetPosition = tarPos;
                        player.position = tarPos;
                        CalculateEnemies();
                        StartCoroutine(player.MoveCoroutine());
                        CheckSkullGrams(player, tarPos);
                    }
                    else if (CheckHole(player, tarPos))
                    {
                        player.Fall(tarPos);
                        GameOver();
                        break;
                    }
                    MoveEnemies();

                }
                break;
            case GameState.Wait:
                break;
            case GameState.LevelCompletion:
                break;
            case GameState.Fail:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
                    break;
            default:
                break;
        }
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;
    }
    public IEnumerator WaitState()
    {
        currentState = GameState.Wait;
        yield return new WaitForSeconds(0.3f);
        if (!gameover && !complete)
        {
            currentState = GameState.PlayerMovement;
        }

    }

    public void GameOver()
    {
        gameover = true;
        CanvasAnimator.SetTrigger("gameover");
        ChangeState(GameState.Fail);
    }

    public void CalculateEnemies()
    {
        foreach (Enemy e in enemies)
        {
            e.MoveLogic(player);
            if(e.targetPosition == player.position)
            {
                player.Fall(player.position);
                GameOver();
            }
        }
    }

    public void MoveEnemies()
    {
        foreach(Enemy e in enemies)
        {
            StartCoroutine(e.MoveCoroutine());
        }
    }

    public IEnumerator NextLevel()
    {
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public bool CheckPossibleMovement(Object o, Vector3 targetPosition)
    {
        Vector2 pos = new Vector2(targetPosition.x, targetPosition.y);
        int index = Array.IndexOf(levelData.walkable, pos);
        if (index > -1)
        {
            if(trapdoor.open && trapdoor.position == targetPosition)
            {
                complete = true;
                CanvasAnimator.SetTrigger("complete");
                ChangeState(GameState.LevelCompletion);
                StartCoroutine(NextLevel());
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckHole(Object o, Vector3 targetPosition)
    {
        Vector2 pos = new Vector2(targetPosition.x, targetPosition.y);
        int index = Array.IndexOf(levelData.hole, pos);
        if (index > -1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckSkullGrams(Object o, Vector3 targetPosition)
    {
        int index = -1;
        foreach (SkullGram g in skullgrams)
        {
            if(g.position == targetPosition)
            {
                index = g.index;
                break;
            }
        }
        if (index > -1)
        {
            foreach (SkullGram g in skullgrams)
            {
                g.Activate();
                if (g.index != index)
                {
                    Enemy e = g.SpawnEnemy();
                    enemies.Add(e);
                }
            }
            trapdoor.Activate(0);
            return true;
        }
        else
        {
            return false;
        }
    }
}