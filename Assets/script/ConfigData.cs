using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ConfigData
{
    private static int dlgTransparent;
    private static int bgmVolume;
    private static int autoSpeed;
    private static int textShowSpeed;
    private static bool skipUnread;
    public static void initClass()
    {
        dlgTransparent = PlayerPrefs.GetInt("dlgTransparent", 0);
        bgmVolume = PlayerPrefs.GetInt("bgmVolume", 100);
        autoSpeed = PlayerPrefs.GetInt("autoSpeed", 50);
        textShowSpeed = PlayerPrefs.GetInt("textShowSpeed", 50);
        skipUnread = PlayerPrefs.GetInt("skipUnread", 0)==0?false:true;
    }
    public static void setDlgTransparent(int value)
    {
        dlgTransparent = value;
        PlayerPrefs.SetInt("dlgTransparent", value);
    }
    public static void setBgmVolume(int value)
    {
        bgmVolume = value;
        PlayerPrefs.SetInt("bgmVolume", value);
    }
    public static void setAutoSpeed(int value)
    {
        autoSpeed = value;
        PlayerPrefs.SetInt("autoSpeed", value);
    }
    public static void setTextShowSpeed(int value)
    {
        textShowSpeed = value;
        PlayerPrefs.SetInt("textShowSpeed", value);
    }
    public static void setSkipUnread(bool value)
    {
        skipUnread = value;
        PlayerPrefs.SetInt("skipUnread", value==false?0:1);
    }
    public static int getDlgTransparent()
    {
        return dlgTransparent;
    }
    public static int getBgmVolume()
    {
        return bgmVolume;
    }
    public static int getAutoSpeed()
    {
        return autoSpeed;
    }
    public static int getTextShowSpeed()
    {
        return textShowSpeed;
    }
    public static bool getSkipUnread()
    {
        return skipUnread;
    }
}
