using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SavePosition : MonoBehaviour {

    [DllImport("__Internal")]
    private static extern void SyncFiles();

    public void Save()
        {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/resumeSaveData.dat");

        SaveData save = new SaveData();
        save.x = transform.position.x;

        bf.Serialize(file, save);
        file.Close();

        if (Application.platform == RuntimePlatform.WebGLPlayer)
            SyncFiles();
        }

    public float Load()
        {
        float toReturn = 0;
        if (File.Exists(Application.persistentDataPath))
            {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/dominoSaveData.dat", FileMode.Open);

            SaveData save = (SaveData)bf.Deserialize(file);
            toReturn = save.x;
            
            file.Close();
            }
        return toReturn;
        }

    }

[Serializable]
class SaveData
    {
    public float x;
    }
