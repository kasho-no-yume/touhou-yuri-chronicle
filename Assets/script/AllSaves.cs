using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AllSaves 
{
    public List<SaveFile> allSaves = new List<SaveFile>();
    public AllSaves(List<SaveFile> list)
    {
        allSaves = list;
    }
}
