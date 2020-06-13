using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelData", menuName = "ScriptableObjects/LevelData")]
public class LevelData : ScriptableObject
{
    [Tooltip("Level duration in seconds.")]
    public float duration;

    public SpawnField[] spawnFields;
}
