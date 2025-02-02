using System.IO;
using UnityEngine;
public class FileHandler
{
    private string fileName;
    public FileHandler(string fileName)
    {
        this.fileName = fileName;
    }
    public void Save(GameState data)
    {
        Debug.Log(JsonUtility.ToJson(data));
        string json = JsonUtility.ToJson(data);
        string path = Path.Combine(Application.persistentDataPath, fileName);

        File.WriteAllText(path, json);
        Debug.Log("Game Saved at:" + path);
    }

    public GameState Load()
    {
        GameState gameState = null;
        string path = Path.Combine(Application.persistentDataPath, fileName);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            gameState = JsonUtility.FromJson<GameState>(json);
        }
        else
        {
            Debug.Log("File not exist.");
        }
        return gameState;
    }
}