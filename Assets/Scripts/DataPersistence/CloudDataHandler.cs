using System;
using UnityEngine;

namespace DataPersistence
{
    public class CloudDataHandler
    {
        // We will combine the path and file name to create a unique key for PlayerPrefs.
        private string _saveKey;

        public CloudDataHandler(string dataDirPath, string dataFileName)
        {
            // Example key: "gameData-MyGameSave"
            this._saveKey = dataDirPath + "-" + dataFileName;
        }

        public GameData Load()
        {
            GameData loadedData = null;

            // Check if a save entry with our key exists in PlayerPrefs.
            if (PlayerPrefs.HasKey(_saveKey))
            {
                try
                {
                    // Retrieve the saved JSON string.
                    string dataToLoad = PlayerPrefs.GetString(_saveKey);
                
                    // Convert the JSON string back into a GameData object.
                    loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
                }
                catch (Exception e)
                {
                    Debug.LogError("CloudDataHandler: Error while loading data: " + e.Message);
                }
            }
            return loadedData;
        }

        public void Save(GameData data)
        {
            try
            {
                // Convert the GameData object to a JSON string.
                string dataToStore = JsonUtility.ToJson(data, true);

                // Save the JSON string to PlayerPrefs with our unique key.
                PlayerPrefs.SetString(_saveKey, dataToStore);
            
                // PlayerPrefs.Save() writes all modified preferences to disk (or localStorage).
                PlayerPrefs.Save();
            }
            catch (Exception e)
            {
                Debug.LogError("CloudDataHandler: Error while saving data: " + e.Message);
            }
        }

        public bool SaveFileExists()
        {
            // This is the PlayerPrefs equivalent of File.Exists().
            return PlayerPrefs.HasKey(_saveKey);
        }
    }
}