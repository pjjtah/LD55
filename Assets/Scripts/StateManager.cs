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
    public GameObject[] breakingTiles;
    public List<Vector2> walkableData;
    public List<Vector2> holeData;

    void Start()
    {
        // Initialize the game with the PlayerMovement state
        currentState = GameState.PlayerMovement;
        walkableData = new List<Vector2>(levelData.walkable);
        holeData = new List<Vector2>(levelData.hole);
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
                    else
                    {
                        StartCoroutine(player.TryMoveCoroutine(new Vector3(player.position.x - 0.5f, player.position.y, player.position.z)));
                        CalculateEnemies();
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
                    else
                    {
                        StartCoroutine(player.TryMoveCoroutine(new Vector3(player.position.x + 0.5f, player.position.y, player.position.z)));
                        CalculateEnemies();
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
                    else
                    {
                        StartCoroutine(player.TryMoveCoroutine(new Vector3(player.position.x, player.position.y + 0.5f, player.position.z)));
                        CalculateEnemies();
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
                    else
                    {
                        StartCoroutine(player.TryMoveCoroutine(new Vector3(player.position.x, player.position.y - 0.5f, player.position.z)));
                        CalculateEnemies();
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
            if(e is Imp)
            {
                if(CheckPossibleMovement(e, e.targetPosition))
                {
                    StartCoroutine(e.MoveCoroutine());
                }
                else if(CheckHole(e, e.targetPosition))
                {
                    (e as Imp).Fall(e.targetPosition);
                }
                else
                {
                    StartCoroutine(e.TryMoveCoroutine(e.targetPosition));
                    e.targetPosition = e.position;
                }
            }
            else
            {
                StartCoroutine(e.MoveCoroutine());
            }
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
        int index = walkableData.IndexOf(pos);
        if (index > -1)
        {
            foreach(GameObject tile in breakingTiles)
            {
                if(tile.transform.position == o.position)
                {
                    tile.GetComponent<Animator>().SetTrigger("break");
                    walkableData.Remove(new Vector2(tile.transform.position.x, tile.transform.position.y));
                    holeData.Add(new Vector2(tile.transform.position.x, tile.transform.position.y));
                }
            }

            if(trapdoor.open && trapdoor.position == targetPosition && !gameover && o is Player)
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
        int index = holeData.IndexOf(pos);
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
                if (!g.activated)
                {
                    g.Activate();
                    if (g.index != index)
                    {
                        Enemy e = g.SpawnEnemy();
                        enemies.Add(e);
                    }
                    trapdoor.Activate(0);
                }

            }

            return true;
        }
        else
        {
            return false;
        }
    }
}