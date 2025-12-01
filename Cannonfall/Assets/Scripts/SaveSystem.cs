using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class SaveSystem : MonoBehaviour
{
    private static SaveData _saveData = new SaveData(); // static struct

    [System.Serializable]
    public struct SaveData
    {
        public PlayerSaveData PlayerData;
    }

    public static string SaveFileName()
    {
        int saveSlot = GameManager.instance.SaveSlot;
        string saveFile = "";
        if (saveSlot == 1)
        {
            saveFile = Application.persistentDataPath + "slot1" + ".save";
        }
        else if (saveSlot == 2)
        {
            saveFile = Application.persistentDataPath + "slot2" + ".save";
        }
        else if (saveSlot == 3)
        {
            saveFile = Application.persistentDataPath + "slot3" + ".save";
        }
        else if (saveSlot == 4)
        {
            saveFile = Application.persistentDataPath + "slot4" + ".save";
        }
        else
        {
            return null;
        }
        return saveFile;
    }

    public static void Save()
    {
        //Debug.Log(SaveFileName());
        HandleSaveData();
        File.WriteAllText(SaveFileName(), JsonUtility.ToJson(_saveData, true)); // true makes it human-readable
    }

    private static void HandleSaveData()
    {
        GameManager.instance.Player.GetComponent<Player>().Save(ref _saveData.PlayerData); // call save in Player
    }

    public static void Load()
    {
        string saveContent = File.ReadAllText(SaveFileName());
        _saveData = JsonUtility.FromJson<SaveData>(saveContent);
        HandleLoadData();
    }

    private static void HandleLoadData()
    {
        //Debug.Log(SaveFileName());
        if (SceneManager.GetActiveScene().name != _saveData.PlayerData.currentScene && _saveData.PlayerData.currentScene != null)
        {
            SceneManager.LoadScene(_saveData.PlayerData.currentScene);
        }
        GameManager.instance.Player.GetComponent<Player>().Load(_saveData.PlayerData); // call in Player
    }

    public static string readFileTime(int fileNumber)
    {
        string saveFile = Application.persistentDataPath + "slot" + fileNumber + ".save";
        string saveContent = File.ReadAllText(saveFile);
        _saveData = JsonUtility.FromJson<SaveData>(saveContent);
        return _saveData.PlayerData.time;
    }

    public static string readFileScene(int fileNumber)
    {
        string saveFile = Application.persistentDataPath + "slot" + fileNumber + ".save";
        string saveContent = File.ReadAllText(saveFile);
        _saveData = JsonUtility.FromJson<SaveData>(saveContent);
        return _saveData.PlayerData.currentScene;
    }

    public static void setFileScene(string name)
    {
        HandleSaveData(); // get the necessary data
        _saveData.PlayerData.currentScene = name; // change the scene to the next one
        File.WriteAllText(SaveFileName(), JsonUtility.ToJson(_saveData, true)); // save it again
    }
}
