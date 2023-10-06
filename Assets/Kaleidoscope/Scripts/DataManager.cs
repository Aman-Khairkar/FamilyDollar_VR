using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public Config config;
    string savePath;

    public void Save()
    {
        string json = JsonUtility.ToJson(config);

        WriteToFile(savePath, json);
    }

    public void Load()
    {
        savePath = Application.streamingAssetsPath + "/Config.json";
        //savePath = "jar:file://" + Application.dataPath + "!/assets/Config.json";
        string json = ReadFromFile(savePath);
        Debug.Log("json is " + json);

        if (json == "")
        {
            Save();
            return;
        }
        
        config = JsonUtility.FromJson<Config>(json);
    }

    private void WriteToFile(string filename, string json)
    {
        FileStream fileStream = new FileStream(filename, FileMode.Create);

        using(StreamWriter writer = new StreamWriter(fileStream))
        {
            writer.Write(json);
        }
    }

    private string ReadFromFile(string filename)
    {
        if (File.Exists(filename))
        {
            Debug.Log("Config found!");
            using(StreamReader reader = new StreamReader(filename))
            {
                string json = reader.ReadToEnd();
                return json;
            }
        }else
        {
            Debug.LogWarning("Config not found");
            return "";
        }
    }
}

