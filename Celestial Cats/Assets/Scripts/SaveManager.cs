using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour {

    private const string _saveFilePath = "/save.json";

    public static SaveManager Instance;

    private void Awake() {
        DontDestroyOnLoad(gameObject);

        if (!Instance) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        Load();
    }

    public void Save() {

        // create new save
        SaveData data = new(GameManager.Instance.LastLevelWon);
        string jsonString = JsonUtility.ToJson(data);

        // write to file
        string destination = Application.persistentDataPath + _saveFilePath;
        File.WriteAllText(destination, jsonString);
    }

    private void Load() {

        string destination = Application.persistentDataPath + _saveFilePath;

        // look for save file
        if (File.Exists(destination)) {

            // read in data
            string fileContents = File.ReadAllText(destination);

            // deserialize
            SaveData data = JsonUtility.FromJson<SaveData>(fileContents);

            // set values
            GameManager.Instance.LastLevelWon = data.LastLevelWon;
        }

        // if no save file found, load default values
        else {
            GameManager.Instance.LastLevelWon = -1;
        }
    }
}