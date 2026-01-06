using UnityEngine;
using System.IO;

public static class SaveSystem
{
    public static void SavePlayer(MyCharacterController player)
    {
        PlayerData data = new PlayerData(player);
        string json = JsonUtility.ToJson(data, true);
        
        // CHANGED: Now saves to Assets/my_scripts/capsule_save.json
        string path = Application.dataPath + "/my_scripts/capsule_save.json";
        
        File.WriteAllText(path, json);
        Debug.Log("Saved to: " + path);
    }

    public static PlayerData LoadPlayer()
    {
        // CHANGED: Load from the same new path
        string path = Application.dataPath + "/my_scripts/capsule_save.json";
        
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<PlayerData>(json);
        }
        else
        {
            Debug.LogWarning("No save file found at: " + path);
            return null;
        }
    }
}