[System.Serializable]
public class SpawnWave
{
    public int Wheight { get; private set; }
    public float delay;
    public SpawnField[] spawnFields;

    private float _spawnDuration;
    private bool _calculatedDuration;

    public float TotalDuration 
    {
        get
        {
            if (!_calculatedDuration)
            {
                _spawnDuration = CalculateSpawnDuration();
            }

            return delay + _spawnDuration;
        }
    }

    private float CalculateSpawnDuration()
    {
        float maxDuration = -1;
        for (int i = 0; i < spawnFields.Length; i++)
        {
            if (i == 0)
                maxDuration = spawnFields[i].TotalDuration;
            else if (spawnFields[i].TotalDuration > maxDuration)
                maxDuration = spawnFields[i].TotalDuration;
        }
        _calculatedDuration = true;

        if (maxDuration == -1) return 0;
        else return maxDuration;
    }

    public SpawnWave(float delay, SpawnField[] spawnFields)
    {
        this.delay = delay;
        this.spawnFields = spawnFields;

        foreach (SpawnField spawnField in spawnFields)
        {
            Wheight += spawnField.Wheight;
        }

        _spawnDuration = CalculateSpawnDuration();
    }
}
