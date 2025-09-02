using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DataPersistence
{
    public class DataPersistenceManager : MonoBehaviour
    {
        #region Singleton
        public static DataPersistenceManager Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            #if UNITY_WEBGL
                        this.cloudDataHandler = new CloudDataHandler(Application.persistentDataPath, fileName);
            #else
                        this.fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
            #endif
        }
        #endregion
    
        private GameData gameData;
        private List<IDataPersistence> dataPersistenceObjects;
        
        [SerializeField] private string fileName;
        private FileDataHandler fileDataHandler;
        private CloudDataHandler cloudDataHandler;
        
        private bool isNewGame = false;
        
        private void Start()
        {
            LoadGame();
        }
        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name is "Scene - Bala" or "Tutorial - FINAL" or "Main Menu - FINAL")
            {
                LoadGame();
            }
        }
        
        public bool HasSaveData()
        {
            #if UNITY_WEBGL
                        return cloudDataHandler != null && cloudDataHandler.SaveFileExists();
            #else
                        return fileDataHandler != null && fileDataHandler.SaveFileExists();
            #endif
        }

        public void NewGame()
        {
            this.gameData = new GameData();
            this.isNewGame = true;
        }

        public void LoadGame()
        {
            this.dataPersistenceObjects = FindAllDataPersistenceObjects();
            if (this.isNewGame)
            {
                this.isNewGame = false; 
            }
            else
            {
                #if UNITY_WEBGL
                    this.gameData = cloudDataHandler.Load();
                #else
                    this.gameData = fileDataHandler.Load();
                #endif
            }
            if (this.gameData == null)
            {
                Debug.Log("No save data found. Starting a New Game.");
                NewGame();
            }

            foreach (IDataPersistence persistence in dataPersistenceObjects)
            {
                persistence.LoadData(gameData);
            }
        }

        public void SaveGame()
        {
            this.dataPersistenceObjects = FindAllDataPersistenceObjects();
            
            foreach (IDataPersistence persistence in dataPersistenceObjects)
            {
                persistence.SaveData(ref gameData);
            }
        
            #if UNITY_WEBGL
                        cloudDataHandler.Save(gameData);
            #else
                        fileDataHandler.Save(gameData);
            #endif
        }
        
        private List<IDataPersistence> FindAllDataPersistenceObjects()
        {
            IEnumerable<IDataPersistence> dataPersistenceObjects = 
                FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None)
                    .OfType<IDataPersistence>();
            
            return new List<IDataPersistence>(dataPersistenceObjects);
        }
    }
}
