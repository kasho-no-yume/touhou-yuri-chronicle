using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    // Start is called before the first frame update
    private int dlgTransparent;
    private int bgmVolume;
    private int autoSpeed;
    private int textShowSpeed;
    private bool skipUnread;
    public GameObject clickArea;
    public TextAsset scenario;
    public GameObject Gbackground;
    public GameObject dialogPanel;
    public static int globalProgress = 0;
    public static bool autoing = false;
    public static bool skipping = false;
    private string[] stringScenario;
    private static bool canAutoNext;
    private int waitTime=0;
    private GameUIMgr uimgr;
    public static string background;
    public static string characterKoishi;
    public static string characterSatori;
    public static string bgm;
    public static string previewText;
    public static void ReadProgress(int progress)
    {
        globalProgress = progress;
    }
    static GameLogic()
    {

    }
    //只提供修改当前场景数据,生效需重载场景
    public static void ChangeSceneBySave(SaveFile save)
    {
        globalProgress = save.position-1;
        background = save.background;
        bgm = save.bgm;
        characterKoishi = save.characterKoishi;
        characterSatori = save.characterSatori;
        previewText = save.previewText;
    }
    void Start()
    {
        uimgr = GameUIMgr.instance;
        ConfigData.initClass();
        autoSpeed = ConfigData.getAutoSpeed();
        uimgr.BlackDisappear();
        dlgTransparent = ConfigData.getDlgTransparent();
        bgmVolume = ConfigData.getBgmVolume();
        autoSpeed = ConfigData.getAutoSpeed();
        textShowSpeed = ConfigData.getTextShowSpeed();
        skipUnread = ConfigData.getSkipUnread();
        stringScenario = scenario.text.Split('\n');
        clickArea.GetComponent<Button>().onClick.AddListener(ClickScreen);
        uimgr.ChangeCG(background);
        uimgr.ChangeCharaOfKoishi(characterKoishi);
        uimgr.ChangeCharaOfSatori(characterSatori);
        uimgr.ChangeBGM(bgm);
        //uimgr.ChangeText(stringScenario[globalProgress]);
        NextSentence();
    }
    
    public void NextSentence()
    {
        
        if (globalProgress >= stringScenario.Length)
        {
            uimgr.GameOver();
            autoing = false;
            skipping = false;
            return;
        }
        string segment = stringScenario[globalProgress];
        while (IsCommand(segment))
        {
            if (GetHeader(segment) == "bg")
            {
                background = GetContent(segment);
                uimgr.ChangeCG(background);
                
            }
            else if(GetHeader(segment)=="bgm")
            {
                bgm = GetContent(segment);
                uimgr.ChangeBGM(bgm);
            }
            else if(GetHeader(segment)=="koishi")
            {
                characterKoishi = GetContent(segment);
                uimgr.ChangeCharaOfKoishi(characterKoishi);
            }
            else if (GetHeader(segment) == "satori")
            {
                characterSatori = GetContent(segment);
                uimgr.ChangeCharaOfSatori(characterSatori);
            }
            globalProgress++;
            segment = stringScenario[globalProgress];
        }
        uimgr.ChangeText(segment);
        previewText = segment;
        globalProgress++;
    }
    public void ClickScreen()
    {
        skipping = false;
        autoing = false;
        if (!uimgr.hiding)
        {           
            if (uimgr.showing)
            {
                uimgr.ChangeTextImmediately();
            }
            else
            {
                NextSentence();
            }
        }
        else
        {
            uimgr.ShowMainUI();
        }
        
    }
    private string GetHeader(string s)
    {
        string[] res = s.Split(' ');
        return res[0];
    }
    private string GetContent(string s)
    {
        string[] res = s.Split(' ');
        if(res.Length==1)
        {
            return null;
        }
        return res[1].Substring(0,res[1].Length-1);
    }
    private bool IsCommand(string s)
    {
        string[] res = s.Split(' ');
        if(res.Length==1)
        {
            return false;
        }
        return true;
    }
    public static void SetAutoNext()
    {
        canAutoNext = true;
    }
    private void FixedUpdate()
    {
        if(autoing)
        {
            if(canAutoNext)
            {
                Invoke("NextSentence",autoSpeed/5f);
                canAutoNext = false;
            }
        }
        else if(skipping)
        {
            if (uimgr.showing)
            {
                uimgr.ChangeTextImmediately();
            }
            else
            {
                NextSentence();
            }
        }
    }
}
