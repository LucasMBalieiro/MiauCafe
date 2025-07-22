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
            
            this.fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        }
        #endregion
    
        private GameData gameData;
        private List<IDataPersistence> dataPersistenceObjects;
        
        [SerializeField] private string fileName;
        private FileDataHandler fileDataHandler;
        
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
        
        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // TODO: gambiarra do krl isso, melhor generalizar no futuro
            if (scene.name == "Scene - Bala")
            {
                this.LoadGame();
            }
        }
        
        public bool HasSaveData()
        {
            return fileDataHandler != null && fileDataHandler.SaveFileExists();
        }

        public void NewGame()
        {
            this.gameData = new GameData();
        }

        public void LoadGame()
        {
            this.dataPersistenceObjects = FindAllDataPersistenceObjects();
            this.gameData = fileDataHandler.Load();

            if (this.gameData == null)
            {
                Debug.Log("gameData is null, gerando NewGame");
                NewGame();
            }

            foreach (IDataPersistence persistence in dataPersistenceObjects)
            {
                persistence.LoadData(gameData);
            }
        }

        public void SaveGame()
        {
            foreach (IDataPersistence persistence in dataPersistenceObjects)
            {
                persistence.SaveData(ref gameData);
            }
        
            fileDataHandler.Save(gameData);
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
