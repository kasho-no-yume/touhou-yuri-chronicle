using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//存档规则,json间以******作为分隔符
public static class SLData
{
    private static List<SaveFile> allSaves=new List<SaveFile>();
    private static string path=Application.persistentDataPath;
    static SLData()
    {
        allSaves = ReadSaves();
    }
    private static List<SaveFile> ReadSaves()
    {
        List<SaveFile> tempList = new List<SaveFile>();
        if (File.Exists(path + "/save.txt")) 
        {
            string tempString = File.ReadAllText(path + "/save.txt");
            AllSaves all = JsonUtility.FromJson<AllSaves>(tempString);
            tempList = all.allSaves;
        }
        return tempList;
    }
    public static void SaveNew(SaveFile save)
    {
        allSaves.Add(save);
        SaveToLocal();
    }
    public static void DeleteSave(SaveFile save)
    {
        allSaves.Remove(save);
    }
    public static void ReplaceSaveOf(SaveFile uselessSave,SaveFile save)
    {
        allSaves.Insert(allSaves.IndexOf(uselessSave), save);
        allSaves.Remove(uselessSave); 
        SaveToLocal();
    }
    public static int GetSaveNumber()
    {
        return allSaves.Count;
    }
    public static List<SaveFile> GetSaves()
    {
        return allSaves;
    }
    private static void SaveToLocal()
    {
        string res=JsonUtility.ToJson(new AllSaves(allSaves));
        File.WriteAllText(path + "/save.txt", res);
    }
}
