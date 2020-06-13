//
//
// This is a master Data subclass that contains all data of the game
// This is the class that is used in SaveSystem
//
//

namespace My_Utils.Examples
{
    [System.Serializable]
    public class GameData : Data
    {
        public LevelData levelData;
        public HealthData healthData;
        public PosData posData;

        public GameData(LevelData levelData, HealthData healthData, PosData posData)
        {
            this.levelData = levelData;
            this.healthData = healthData;
            this.posData = posData;
        }
    }
}
