using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveGameInfo ( Game_Manager gm)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/ShadowArcher.data";
        FileStream stream = new FileStream(path, FileMode.Create);

        LevelSaveData data = new LevelSaveData(gm);
        formatter.Serialize(stream, data);
        stream.Close();

    }

    public static LevelSaveData LoadGameInfo()
    {
        string path = Application.persistentDataPath + "/ShadowArcher.data";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            
            LevelSaveData data = formatter.Deserialize(stream) as LevelSaveData;

            stream.Close();

            return data;
        } else
        {
            return null;
        }
        
    }
}
