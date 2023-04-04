using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonHandler
{
    public static List<GameData> ReadJson()
    {
        List<GameData> GameDataList = new List<GameData>();
        using (StreamReader sr = new StreamReader(Application.dataPath + "/gameData.json"))
        {
            string json = sr.ReadToEnd();
            GameDataList.Add(JsonUtility.FromJson<GameData>(json));
        }

        return GameDataList;
    }
}

public class GameData {
    public int id;
    public string name;
    public string[] ingredients;
    public string[] guide;
};