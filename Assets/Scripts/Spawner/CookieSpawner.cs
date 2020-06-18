using My_Utils;
using System.Collections;
using UnityEngine;

public class CookieSpawner : MonoBehaviour
{
    [SerializeField] private LevelData _levelData = default;
    [SerializeField] private Transform[] _spawnPoints = default;

    private void Start()
    {
        StartLevel(0.5f);
    }

    public void StartLevel(float delay)
    {
        StartCoroutine(StartSpawner(delay));
    }

    private IEnumerator StartSpawner(float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (SpawnField spawnField in _levelData.spawnFields)
        {
            StartCoroutine(SpawnFied(spawnField));
        }
    }

    private IEnumerator SpawnFied(SpawnField spawnField)
    {
        float interval = _levelData.duration / spawnField.qttOfInstances;

        for (int i = 0; i < spawnField.qttOfInstances; i++)
        {
            yield return new WaitForSeconds(interval);
            Instantiate(spawnField.cookiePrefab, GetRandomSpawnPos(), Quaternion.identity);
        }
    }

    private Vector2 GetRandomSpawnPos()
    {
        return _spawnPoints[Random.Range(0, _spawnPoints.Length)].position;
    }
}
