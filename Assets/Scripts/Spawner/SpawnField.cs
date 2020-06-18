[System.Serializable]
public class SpawnField
{
    public int Wheight { get; private set; }

    public float delay;
    public float spawnDuration;
    public int qttOfInstances;
    public Cookie cookiePrefab;

    public float TotalDuration { get => spawnDuration + delay; }

    public SpawnField(float delay, float spawnDuration, int qttOfInstances, Cookie cookiePrefab)
    {
        this.delay = delay;
        this.spawnDuration = spawnDuration;
        this.qttOfInstances = qttOfInstances;
        this.cookiePrefab = cookiePrefab;
        Wheight = cookiePrefab.Wheight * qttOfInstances;
    }
}
