//
//
// This is a Data subclass that is stored in GameData.
//
//

namespace My_Utils.Examples
{
    [System.Serializable]
    public class LevelData : Data
    {
        public int level;

        public LevelData(int level)
        {
            this.level = level;
        }
    }
}
