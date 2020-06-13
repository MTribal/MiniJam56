//
//
// This is a Data subclass that is stored in GameData.
//
//

namespace My_Utils.Examples
{
    [System.Serializable]
    public class HealthData : Data
    {
        public int health;

        public HealthData(int health)
        {
            this.health = health;
        }
    }
}
