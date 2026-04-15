using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


[System.Serializable]
public static class SaveSystem {
    private static string SavePath => Application.persistentDataPath + "/Patasy_saves/";
    private static void EnsureDirectory() {
        if (!Directory.Exists(SavePath))
            Directory.CreateDirectory(SavePath);
    }
    public static void SavePlayer(GameObject player, PlayerMovement player_stats, EnemyGenerator enemies, string name){
        EnsureDirectory();
        string path = SavePath + name + ".psv";
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream stream = new FileStream(path, FileMode.Create)) {
            Player_data data = new Player_data(player, player_stats, enemies);
            formatter.Serialize(stream, data);
        }
    }

    public static Player_data LoadPlayer(string name) {
        EnsureDirectory();
        string path = SavePath + name;
        if (File.Exists(path)){
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(path, FileMode.Open)) {
                return formatter.Deserialize(stream) as Player_data;
            }
        }
        Debug.LogError("Seve file not found in " + path);
        return null;     
    }

    public static string ShowSaves() {
        EnsureDirectory();
        string return_value = null;
        string path = SavePath;
        DirectoryInfo dir = new DirectoryInfo(path);
        foreach (FileInfo f in dir.GetFiles("*.psv")) {
            using (FileStream stream = new FileStream(f.FullName, FileMode.Open)) {
                BinaryFormatter formatter = new BinaryFormatter();
                Player_data data = formatter.Deserialize(stream) as Player_data;
                return_value += "\n" + " Player name: " + data.name + " Player score: " + data.score.ToString() + " Player health: " + data.health.ToString() +
                                "       Enemies: " + data.enemy_names.Length;
            }
            Debug.Log("Found: " + f.Name);
        }
        return return_value;
        //System.IO.Path.GetFileName(fullPath);  
    }

    public static int Count_of_Saves() {
        EnsureDirectory();
        return new DirectoryInfo(SavePath).GetFiles("*.psv").Length;
    }

    public static string Return_save_names() {
        EnsureDirectory();
        string return_value = null;
        foreach (FileInfo f in new DirectoryInfo(SavePath).GetFiles("*.psv")) {
            return_value += "\n" + f.Name;
        }
        return return_value;
    }

    public static void Delete_file(string name) {
        EnsureDirectory();
        string path = SavePath + name;
        if(!File.Exists(path)){
            Debug.Log("File not exist " + name);
        }
        else{
            File.Delete(path);
        }
    }
}
