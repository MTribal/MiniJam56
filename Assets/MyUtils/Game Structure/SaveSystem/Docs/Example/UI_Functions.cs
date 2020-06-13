//
//
// This class contains the UI functions of the scene.
// The SaveSystem trigger is here.
//
//

using UnityEngine;
using TMPro;

namespace My_Utils.Examples
{
    public class UI_Functions : MonoBehaviour
    {
        private const string gameDataKey = "gameDataKey";

        public TextMeshProUGUI levelText;
        public TextMeshProUGUI posText;
        public TextMeshProUGUI healthText;

        public int level = 1;
        public int health = 10;
        public Vector3 pos = Vector3.zero; 

        public void LevelMore()
        {
            level++;
            levelText.text = level.ToString();
        }

        public void LevelMin()
        {
            level--;
            levelText.text = level.ToString();
        }

        public void PosMin()
        {
            pos -= MyRandom.NextVector3(0, 2);
            posText.text = pos.ToString();
        }

        public void PosMore()
        {
            pos += MyRandom.NextVector3(0, 2);
            posText.text = pos.ToString();
        }

        public void HealthMore()
        {
            health++;
            healthText.text = health.ToString();
        }

        public void HealthMin()
        {
            health--;
            healthText.text = health.ToString();
        }

        /// <summary>
        /// SaveSystem Save trigger
        /// </summary>
        public void Save()
        {
            LevelData levelData = new LevelData(level);
            HealthData healthData = new HealthData(health);
            PosData posData = new PosData(pos);
            GameData gameData = new GameData(levelData, healthData, posData); // Gather all data in the same class (like a 'MasterData')

            SaveSystem.SaveDataIn(gameData, gameDataKey); // Pass the 'MasterData' as Data (Polimorfism) and the key to save the data.
        }

        /// <summary>
        /// SaveSystem Load trigger
        /// </summary>
        public void Load()
        {
            GameData data = SaveSystem.LoadData<GameData>(gameDataKey); // Load the Data as GameData with the same key used to save.

            // -> Apply load 
            level = data.levelData.level;
            pos = data.posData.pos.ToVector3();
            health = data.healthData.health;

            levelText.text = level.ToString();
            posText.text = pos.ToString();
            healthText.text = health.ToString();
        }

        /// <summary>
        /// Delete A keys in 
        /// </summary>
        public void DeleteKey()
        {
            SaveSystem.DeleteKey(gameDataKey);
        }
    }
}
