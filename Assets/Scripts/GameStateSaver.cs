using System.IO;
using System.Text;
using UnityEngine;

public static class GameStateSaver
{
    public static void Save(GameState state, string path)
    {
        
        var json = JsonUtility.ToJson(state);
        StringBuilder sb = new StringBuilder(json);
        foreach (var item in state.enemiesTransform) 
        {
            string enemeJson = JsonUtility.ToJson(item);
            sb.AppendLine("&&" + enemeJson);
        }
        File.WriteAllText(path, sb.ToString());
        
    }

    public static GameState Load(string path)
    {
        string str = File.ReadAllText(path);
        string[] jsons = str.Split("&&");
        GameState state = JsonUtility.FromJson<GameState>(jsons[0]);
        for (int i = 1; i < jsons.Length; i++)
        {
            state.enemiesTransform[i-1] = JsonUtility.FromJson<TransformSerializable>(jsons[i]);
        }
        return state;
    }
}

