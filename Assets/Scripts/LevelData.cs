using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level Data", menuName = "Level/Level Data")]
public class LevelData : ScriptableObject
{
    public List<Vector2> walkable;
    public List<Vector2> hole;
}
