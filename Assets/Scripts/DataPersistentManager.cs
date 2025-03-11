using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistentManager : MonoBehaviour
{
    public static DataPersistentManager instance { get; private set; }
    private List<IDataPersistent> dataPersistentObjects;
    private FileHandler fileHandler;
    private GameState gameState;

    // Start is called before the first frame update
    void Start()
    {
        this.dataPersistentObjects = FindAllDataPersistentObjects();
        this.fileHandler = new FileHandler("Save.sav");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("error");
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        // 取消訂閱，避免記憶體洩漏
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    public void NewGame()
    {
        GameState.Instance.CreateNewGame();
        // SceneManager.sceneLoaded += OnSceneLoaded;
        StartCoroutine(DisplayLoadingScreen("Level1Scene"));
        foreach (IDataPersistent dataPersistent in dataPersistentObjects)
        {
            dataPersistent.LoadData(GameState.Instance);
        }
    }

    public void LoadGame()
    {
        gameState = fileHandler.Load();
        Debug.Log(gameState);
        if (gameState == null)
        {
            Debug.Log("No gameState");
            // NewGame();
        }

        // GameState.Instance.SetGameState(gameState);
        // SceneManager.sceneLoaded += OnSceneLoaded;
        LoadLevel(GameState.Instance.GetCurrentLoadScene());
    }

    public void EntryGame()
    {
        // gameState = GameState.Instance;
        // SceneManager.sceneLoaded += OnSceneLoaded;
        LoadLevel(GameState.Instance.GetCurrentLoadScene());
    }

    public void LoadLevel(string sceneName)
    {
        // 加載場景
        StartCoroutine(DisplayLoadingScreen(sceneName));
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 確保場景已加載完畢，可以安全地操作場景中的物件
        Debug.Log(scene.name + " Loaded");
        this.dataPersistentObjects = FindAllDataPersistentObjects();
        foreach (IDataPersistent dataPersistent in dataPersistentObjects)
        {
            dataPersistent.LoadData(GameState.Instance);
        }
    }

    public void SaveGame()
    {
        foreach (IDataPersistent dataPersistent in dataPersistentObjects)
        {
            dataPersistent.SaveData();
        }
        fileHandler.Save(GameState.Instance);
    }

    public void EndGame()
    {
        SceneManager.LoadSceneAsync("GameOverScene");
    }

    List<IDataPersistent> FindAllDataPersistentObjects()
    {
        IEnumerable<IDataPersistent> dataPersistents = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistent>();
        return new List<IDataPersistent>(dataPersistents);
    }

    IEnumerator DisplayLoadingScreen(string sceneName)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        while (!async.isDone)
        {
            // EventManager.instance.TriggerLoadingActive();
            yield return new WaitForSeconds(0.05f);
        }
    }
}
