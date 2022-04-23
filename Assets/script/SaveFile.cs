using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class SaveFile
{
    public int position;
    public string background;
    public string characterKoishi;
    public string characterSatori;
    public string bgm;
    public string previewText;
    public SaveFile()
    {
        position = 1;
        background = null;
        characterKoishi = null;
        characterSatori = null;
        previewText = null;
    }
}
