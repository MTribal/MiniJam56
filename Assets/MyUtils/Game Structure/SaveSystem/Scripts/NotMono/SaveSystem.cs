using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

namespace My_Utils
{
    /// <summary>
    /// A static class that can save and load data.
    /// </summary>
    public static class SaveSystem
    {
        private const string EXTENSION = ".tbl";

        private readonly static string DATA_PATH = Application.persistentDataPath + "/StreamingAssets/";
        private readonly static string DICT_PATH;

        /// <summary>
        /// Contain each data path.
        /// </summary>
        private static readonly IDictionary<string, string> dataDict;


        static SaveSystem()
        {
            DICT_PATH = DATA_PATH + "dtdc" + EXTENSION;
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            if (File.Exists(DICT_PATH))
            {
                FileStream fileStream = new FileStream(DICT_PATH, FileMode.Open);

                dataDict = (IDictionary<string, string>)binaryFormatter.Deserialize(fileStream);
                fileStream.Close();
            }
            else
            {
                Directory.CreateDirectory(DATA_PATH);

                FileStream fileStream = new FileStream(DICT_PATH, FileMode.Create);

                dataDict = new Dictionary<string, string>();
                binaryFormatter.Serialize(fileStream, dataDict);

                fileStream.Close();
            }
        }


        /// <summary>
        /// Save the new dict information
        /// </summary>
        private static void SaveDict()
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(DICT_PATH, FileMode.Create);

            binaryFormatter.Serialize(fileStream, dataDict);
            fileStream.Close();
        }


        /// <summary>
        /// Return a new data key, that not exist yet.
        /// </summary>
        private static string GetNewKey()
        {
            string possibleKey = MyRandom.GetRandomString(4, CharType.Alphabetic);
            while (dataDict.ContainsKey(possibleKey))
            {
                possibleKey = MyRandom.GetRandomString(4, CharType.Alphabetic);
            }

            return possibleKey;
        }


        /// <summary>
        /// Return a new data path, that not exist yet.
        /// </summary>
        private static string GetNewPath()
        {
            string possiblePath = MyRandom.GetRandomString(8, CharType.Alphabetic);
            while (dataDict.Contains(possiblePath))
            {
                possiblePath = MyRandom.GetRandomString(8, CharType.Alphabetic);
            }
            return DATA_PATH + possiblePath + EXTENSION;
        }


        /// <summary>
        /// !!!! DANGEROUS !!!! Delete a key and clean it's data saved, if key exists.
        /// </summary>
        /// <param name="key">Key to remove.</param>
        /// <returns>Returns true if key exist.</returns>
        public static bool DeleteKey(string key)
        {
            if (dataDict.ContainsKey(key))
            {
                if (File.Exists(dataDict[key]))
                    File.Delete(dataDict[key]);
                dataDict.Remove(key);
                SaveDict();
                return true;
            }
            return false;
        }


        /// <summary>
        /// !!!! DANGEROUS !!!! Delete ALL KEYS and clean ALL DATA saved. Not recommended.
        /// </summary>
        public static void DeleteAllKeys()
        {
            foreach (KeyValuePair<string, string> keyValuePair in dataDict)
            {
                File.Delete(keyValuePair.Value);
            }
            dataDict.Clear();
            SaveDict();
        }


        /// <summary>
        /// Serialize and save your data in a new file and return the key to access that data later. Need be serializable.
        /// </summary>
        /// <param name="data">The data you want to save. Needs be serializable.</param>
        /// <returns>Return the key to access that data later.</returns>
        public static string SaveDataNew(object data)
        {
            string newKey = GetNewKey(); // A key that not exists yet
            string newPath = GetNewPath(); // A path that not exist yet
            dataDict.Add(newKey, newPath);
            SaveDict();

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(newPath, FileMode.Create);

            formatter.Serialize(fileStream, data);
            fileStream.Close();

            return newKey;
        }


        /// <summary>
        /// Serialize and save your data in a existing file, specified by a key. If the key not exists, create a new file with that key.
        /// Need be serializable.
        /// </summary>
        /// <param name="data">The data you want to save.</param>
        /// <param name="key">The key that you want to save your data. Need be serializable.</param>
        public static void SaveDataIn(object data, string key)
        {
            string dataPath;
            if (dataDict.ContainsKey(key))
            {
                dataPath = dataDict[key];
            }
            else
            {
                dataPath = GetNewPath();
                dataDict.Add(key, dataPath);
                SaveDict();
            }

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(dataPath, FileMode.Create);

            formatter.Serialize(fileStream, data);
            fileStream.Close();
        }


        /// <summary>
        /// Return if has a saved data with a specif key.
        /// </summary>
        /// <param name="key">The key that you want to test.</param>
        /// <returns></returns>
        public static bool ContainsKey(string key)
        {
            return key != null && dataDict.ContainsKey(key);
        }


        /// <summary>
        /// Add a key to the system if key not aready exist.
        /// </summary>
        /// <param name="key">The key that you want to add.</param>
        public static void AddKey(string key)
        {
            if (key != null && !dataDict.ContainsKey(key))
            {
                dataDict[key] = "";
            }
        }


        /// <summary>
        /// Load data saved in a key, if key not exist return a default value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T LoadData<T>(string key, T defaultValue)
        {
            if (ContainsKey(key))
            {
                return LoadData<T>(key);
            }
            else
            {
                return defaultValue;
            }
        }


        /// <summary>
        /// Load data saved in a key.
        /// </summary>
        /// <typeparam name="T">The type of the object that you want to load.</typeparam>
        /// <param name="key">The key of the saved data.</param>
        /// <returns></returns>
        public static T LoadData<T>(string key)
        {
            if (!dataDict.ContainsKey(key))
            {
                Debug.LogError("Specified key not found in data base. " + nameof(key));
                return default;
            }
            string dataPath = dataDict[key];

            if (File.Exists(dataPath))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(dataPath, FileMode.Open);

                T data = (T)formatter.Deserialize(fileStream);
                fileStream.Close();

                return data;
            }
            else
            {
                dataDict.Remove(key);
                Debug.LogError("Data not found at " + dataPath);
                return default;
            }
        }
    }
}
