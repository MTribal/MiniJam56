using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteCookieSpawner : MonoBehaviour
{
    [SerializeField] private float _randomWavesDelay = default;

    [SerializeField] private int _wheightIncreaseRate = default;

    [Tooltip("Wheight increase rate is added by this each new wave.")]
    [SerializeField] private int _increaseRateIncreaseRate = default;
    [SerializeField] private int _delayIncreaseRate = default;

    [SerializeField] private Cookie _stuffedCookie = default;
    [SerializeField] private Cookie _roolCookie = default;
    [SerializeField] private Cookie _golemCookie = default;

    [Tooltip("The first states to run in order. This will not be random.")]
    [SerializeField] private SpawnWave[] _firstStates = default; 

    [Space]
    [SerializeField] private Transform[] _spawnPoints = default;


    private void Start()
    {
        StartSpawner(); ////////////////////////////////////////////////////
    }

    public void StartSpawner()
    {
        StartCoroutine(SpawnFirstStates());
    }

    private IEnumerator SpawnFirstStates()
    {
        if (_firstStates.Length == 0) Debug.LogError("Need a first wave.");

        for (int i = 0; i < _firstStates.Length; i++)
        {
            if (i > 0) yield return new WaitForSeconds(_firstStates[i - 1].TotalDuration);
            StartCoroutine(ExecuteSpawnState(_firstStates[i]));   
        }

        SpawnWave previousWave = _firstStates[_firstStates.Length - 1];
        if (_firstStates.Length > 0) yield return new WaitForSeconds(previousWave.TotalDuration);

        StartCoroutine(SpawnLoop(previousWave));
    }

    private IEnumerator SpawnLoop(SpawnWave previousWave)
    {
        bool isFirst = true;
        while (GameManager.Instance.IsPlaying)
        {
            if (!isFirst) yield return new WaitForSeconds(previousWave.TotalDuration);

            SpawnWave spawnWave = GenerateSpawnState(previousWave);
            StartCoroutine(ExecuteSpawnState(spawnWave));
            previousWave = spawnWave;
            isFirst = false;
        }
    }

    private SpawnWave GenerateSpawnState(SpawnWave previousSpawnWave)
    {
        int newWheight = GetNewWheight(previousSpawnWave.Wheight);

        int qttOfStuffed = 0;
        int qttOfRoll = 0;
        int qttOfGolem = 0;

        int atualWheight = 0;
        while (atualWheight < newWheight)
        {
            int rand = Random.Range(0, 3);
            
            switch (rand)
            {
                case 0:
                    qttOfStuffed++;
                    atualWheight += _stuffedCookie.Wheight;
                    break;

                case 1:
                    qttOfRoll++;
                    atualWheight += _roolCookie.Wheight;
                    break;

                case 2:
                    qttOfGolem++;
                    atualWheight += _golemCookie.Wheight;
                    break;
            }
        }

        List<SpawnField> spawnFields = new List<SpawnField>();
        if (qttOfStuffed > 0)
            spawnFields.Add(new SpawnField(0, _randomWavesDelay, qttOfStuffed, _stuffedCookie));

        if (qttOfRoll > 0)
            spawnFields.Add(new SpawnField(0, _randomWavesDelay, qttOfRoll, _roolCookie));

        if (qttOfGolem > 0)
            spawnFields.Add(new SpawnField(0, _randomWavesDelay, qttOfGolem, _golemCookie));

        return new SpawnWave(0, spawnFields.ToArray());
    }

    private int GetNewWheight(int previousWheight)
    {
        _randomWavesDelay += _delayIncreaseRate;
        _wheightIncreaseRate += _increaseRateIncreaseRate;
        return previousWheight + _wheightIncreaseRate;
    }

    private IEnumerator ExecuteSpawnState(SpawnWave spawnState)
    {
        yield return new WaitForSeconds(spawnState.delay);

        foreach (SpawnField spawnField in spawnState.spawnFields)
        {
            StartCoroutine(ExecuteSpawnField(spawnField));
        }
    }

    private IEnumerator ExecuteSpawnField(SpawnField spawnField)
    {
        yield return new WaitForSeconds(spawnField.delay);

        float interval = spawnField.spawnDuration / spawnField.qttOfInstances;

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
