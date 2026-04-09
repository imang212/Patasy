using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


[System.Serializable]
public static class SaveSystem {
    public static void SavePlayer (GameObject player, PlayerMovement player_stats, EnemyGenerator enemies, string name){
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/Patasy_saves/"+ name + ".psv";

        FileStream stream = new FileStream(path, FileMode.Create);
        Player_data data = new Player_data(player, player_stats, enemies);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static Player_data LoadPlayer(string name) {
        string path = Application.persistentDataPath + "/Patasy_saves/" + name;
        if (File.Exists(path)){
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            Player_data data = formatter.Deserialize(stream) as Player_data;
            stream.Close();
            return data;
        }
        else{
            Debug.LogError("Seve file not found in " + path);
            return null;
        }
    }

    public static string ShowSaves() {
        string return_value = null;
        string path = Application.persistentDataPath + "/Patasy_saves/";
        DirectoryInfo dir = new DirectoryInfo(path);
        FileInfo[] info = dir.GetFiles("*.psv");
        foreach (FileInfo f in info) {
            Debug.Log("Found: " + f.Name);
            if (File.Exists(path + f.Name)){
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path + f.Name, FileMode.Open);
                Player_data data = formatter.Deserialize(stream) as Player_data;
                return_value += "\n" + " Player name: " + data.name + " Player score: " + (data.score).ToString() + " Player health: " + (data.health).ToString() + "       Enemies: " + data.enemy_names.Length;
                stream.Close();

            }
            else{
                Debug.LogError("Seve file not found in ");
                return_value = "Save file not found in ";
            }
        }
        return return_value;
        //System.IO.Path.GetFileName(fullPath);  
    }

    public static int Count_of_Saves() {
        int return_value = 0;
        string path = Application.persistentDataPath + "/Patasy_saves/";
        DirectoryInfo dir = new DirectoryInfo(path);
        FileInfo[] info = dir.GetFiles("*.psv");
        foreach (FileInfo f in info) {
            return_value++;
        }
        return return_value;
    }

    public static string Return_save_names() {
        string return_value = null;
        string path = Application.persistentDataPath + "/Patasy_saves/";
        DirectoryInfo dir = new DirectoryInfo(path);
        FileInfo[] info = dir.GetFiles("*.psv");
        foreach ( FileInfo f in info ) {
            return_value+= "\n" + f.Name;
        }
        return return_value;
    }

    public static void Delete_file(string name) {
        string path = Application.persistentDataPath + "/Patasy_saves/" + name;
        if(!File.Exists(path)){
            Debug.Log("File not exist " + name);
        }
        else{
            File.Delete(path);
        }
    }
}
