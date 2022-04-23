using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIMgr : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameUIMgr instance { get; private set; }
    public GameObject musicController;
    private AudioSource audioSource;
    public GameObject satori;
    public GameObject koishi;
    public GameObject blackTop;
    public GameObject dialogPanel;
    public GameObject dialogText;
    public GameObject background;
    public GameObject gaussBackground;
    public GameObject bConfig;
    public GameObject bSave;
    public GameObject bLoad;
    public GameObject bSkip;
    public GameObject bAuto;
    public GameObject bHide;
    public GameObject bReturn;
    public GameObject returnPanel;
    public GameObject sureReturn;
    public GameObject cancelReturn;
    public GameObject panelconfig;
    public GameObject textdlgtransparent, textbgmvolume, textautospeed, textshowspeed, textskipunread;
    public GameObject sliderdlgtransparent, sliderbgmvolume, sliderautospeed, slidershowspeed, toggleskipunread;
    public GameObject btncloseconfig, saveconfig;
    private static float disappearTime=-1;
    private static float appearTime = -1;
    private string aimText;
    private string nowText;
    private Text textComponent;
    private int swidth;
    private int sheight;
    private int textShowTime;
    private int bgmVolume;
    private bool showByChar = true;
    private float timeDelta = 0;    //控制文字逐显的时间计数
    public bool hiding = false;//ui界面是否被隐藏 
    public bool showing=false;//文字是否正在显示
    public bool autoing = false;    //是否正在自动播放
    public bool canAutoNext = false;
    //private float 
    public void BlackDisappear()
    {
        disappearTime = 300;
    }
    public void BlackAppear()
    {
        blackTop.SetActive(true);
        appearTime = 300;
    }
    public void Config()
    {
        HideMainUI();
        ConfigData.initClass();
        panelconfig.SetActive(true);
        audioSource.volume = bgmVolume/100f;
        GameObject[] textConfig = new GameObject[5]
        {textdlgtransparent,textbgmvolume,textautospeed,textshowspeed,textskipunread};
        GameObject[] itemConfig = new GameObject[5]
        {sliderdlgtransparent,sliderbgmvolume,sliderautospeed,slidershowspeed,toggleskipunread};
        itemConfig[0].GetComponent<Slider>().value = ConfigData.getDlgTransparent();
        itemConfig[1].GetComponent<Slider>().value = ConfigData.getBgmVolume();
        itemConfig[2].GetComponent<Slider>().value = ConfigData.getAutoSpeed();
        itemConfig[3].GetComponent<Slider>().value = ConfigData.getTextShowSpeed();
        itemConfig[4].GetComponent<Toggle>().isOn = ConfigData.getSkipUnread();
        itemConfig[1].GetComponent<Slider>().onValueChanged.AddListener
            (onVolumeChange);
        /*SetPosition(panelconfig, 0.05f, 0.05f);
        SetSize(panelconfig, 0.9f, 0.9f);*/
        btncloseconfig.GetComponent<Button>().onClick.AddListener(CloseConfig);
        /*btncloseconfig.GetComponent<RectTransform>().position =
            new Vector2(swidth * 18 / 20, sheight * 18 / 20);
        btncloseconfig.GetComponent<RectTransform>().sizeDelta =
            new Vector2(sheight / 10, sheight / 10);
        float pwidth = panelconfig.GetComponent<RectTransform>().sizeDelta.x;
        float pheight = panelconfig.GetComponent<RectTransform>().sizeDelta.y;
        for (int i = 0; i < 5; i++)
        {
            textConfig[i].GetComponent<RectTransform>().localPosition =
                new Vector2(pwidth / 10, pheight * (9 - i * 2) / 10);
            textConfig[i].GetComponent<RectTransform>().sizeDelta =
                new Vector2(pwidth / 5, pheight / 15);
            itemConfig[i].GetComponent<RectTransform>().localPosition =
                new Vector2(pwidth / 3, 0);
            itemConfig[i].GetComponent<RectTransform>().sizeDelta =
                new Vector2(pwidth / 3, pheight / 20);
        }
        saveconfig.GetComponent<RectTransform>().localPosition =
            new Vector2(pwidth * 4 / 5, pheight / 10);*/
    }

    private void HideMainUI()
    {
        dialogPanel.SetActive(false);
        bConfig.SetActive(false);
        bAuto.SetActive(false);
        bSave.SetActive(false);
        bSkip.SetActive(false);
        bReturn.SetActive(false);
        bHide.SetActive(false);
        bLoad.SetActive(false);
    }
    public void ShowMainUI()
    {
        dialogPanel.SetActive(true);
        bConfig.SetActive(true);
        bAuto.SetActive(true);
        bSave.SetActive(true);
        bSkip.SetActive(true);
        bReturn.SetActive(true);
        bHide.SetActive(true);
        bLoad.SetActive(true);
        hiding = false;
    }
    private void onVolumeChange(float vol)
    {
        audioSource.volume = vol/100f;
    }
    public void CloseConfig()
    {
        panelconfig.SetActive(false);
        ShowMainUI();
    }
    public void SaveConfig()
    {
        ConfigData.setDlgTransparent((int)sliderdlgtransparent.GetComponent<Slider>().value);
        ConfigData.setBgmVolume((int)sliderbgmvolume.GetComponent<Slider>().value);
        ConfigData.setAutoSpeed((int)sliderautospeed.GetComponent<Slider>().value);
        ConfigData.setTextShowSpeed((int)slidershowspeed.GetComponent<Slider>().value);
        ConfigData.setSkipUnread(toggleskipunread.GetComponent<Toggle>().isOn);
        ChangeUI();
        CloseConfig();
    }
    public void ChangeBGM(string bgm)
    {
        audioSource.clip = Resources.Load("bgm/" + bgm, typeof(AudioClip)) as AudioClip;
        audioSource.Play();
    }
    private void Awake()
    {
        ConfigData.initClass();
        audioSource = musicController.GetComponent<AudioSource>();
        audioSource.Play();
        instance = this;
        textComponent = dialogText.GetComponent<Text>();
        blackTop.SetActive(true);
        swidth = UnityEngine.Screen.width;
        sheight = UnityEngine.Screen.height;
        bConfig.GetComponent<Button>().onClick.AddListener(Config);
        btncloseconfig.GetComponent<Button>().onClick.AddListener(CloseConfig);
        saveconfig.GetComponent<Button>().onClick.AddListener(SaveConfig);
        bHide.GetComponent<Button>().onClick.AddListener(FHide);
        bReturn.GetComponent<Button>().onClick.AddListener(Menu);
        sureReturn.GetComponent<Button>().onClick.AddListener(SureReturn);
        cancelReturn.GetComponent<Button>().onClick.AddListener(CancelReturn);
        bSkip.GetComponent<Button>().onClick.AddListener(Skip);
        bAuto.GetComponent<Button>().onClick.AddListener(Auto);
        bSave.GetComponent<Button>().onClick.AddListener(BtnSave);
        bLoad.GetComponent<Button>().onClick.AddListener(BtnLoad);
    }
    private void BtnSave()
    {
        SLUIMgr.instance.ShowPanel(SLUIMgr.Statue.save);

    }
    private void BtnLoad()
    {
        SLUIMgr.instance.ShowPanel(SLUIMgr.Statue.load);
    }
    private void Skip()
    {
        GameLogic.skipping = true;
        GameLogic.autoing = false;
    }
    private void Auto()
    {
        GameLogic.autoing = true;
        GameLogic.skipping = false;
    }

    private void Start()
    {
        ChangeUI();
    }
    //提供方法改变文字显示
    public void ChangeText(string text)
    {
        aimText = text;
        nowText = "";
        timeDelta = 0;
        showing = true;
    }
    public void ChangeTextImmediately(string text)
    {
        textComponent.text = text;
        nowText = text;
        showing = false;
    }
    public void ChangeTextImmediately()
    {
     
        textComponent.text = aimText;
        nowText = aimText;
        showing = false;
    }
    //提供方法改背景cg
    public void ChangeCG(string name)
    {
        if(name=="null"||name==null)
        {
            background.GetComponent<Image>().color = new Color(0,0,0);
            background.GetComponent<Image>().overrideSprite = null;
            gaussBackground.GetComponent<Image>().color = new Color(0, 0, 0);
            gaussBackground.GetComponent<Image>().overrideSprite = null;
        }
        else
        {
            string path = "cg/" + name;
            Debug.Log(path);
            background.GetComponent<Image>().color = new Color(255, 255, 255);
            gaussBackground.GetComponent<Image>().color = new Color(255, 255, 255);
            background.GetComponent<Image>().overrideSprite = 
                Resources.Load(path,typeof(Sprite)) as Sprite;
            gaussBackground.GetComponent<Image>().overrideSprite =
                Resources.Load(path, typeof(Sprite)) as Sprite;
        }
    }
    //提供方法改觉的立绘
    public void ChangeCharaOfSatori(string name)
    {
        if(name==null||name=="null"||name.Length==0)
        {
            satori.SetActive(false);
        }
        else
        {
            satori.SetActive(true);
            string path = "character/satori/" + name;
            satori.GetComponent<Image>().overrideSprite =
                Resources.Load(path, typeof(Sprite)) as Sprite;
        }
    }
    //提供方法改恋的立绘
    public void ChangeCharaOfKoishi(string name)
    {
        if(name == null || name == "null"||name.Length==0)
        {
            koishi.SetActive(false);
        }
        else
        {
            koishi.SetActive(true);
            string path = "character/koishi/" + name;
            koishi.GetComponent<Image>().overrideSprite =
                Resources.Load(path, typeof(Sprite)) as Sprite;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        disappearTime -= 3f;
        appearTime -= 3f;
        //画面显示逻辑
        if (disappearTime>0)
        {
            blackTop.GetComponent<CanvasGroup>().alpha = disappearTime / 300f;
        }
        else if(disappearTime==0&&appearTime<0)
        {
            blackTop.SetActive(false);
        }
        //画面消失逻辑
        if(appearTime>0)
        {
            blackTop.GetComponent<CanvasGroup>().alpha = (300f-appearTime) / 300f;
        }
        else if(appearTime==0)
        {
            SceneManager.LoadScene("theme");
        }
        //文字逐显逻辑
        if (showByChar && timeDelta == 0 && aimText != null)
        {
            if (nowText.Length != aimText.Length)
            {
                nowText = aimText.Substring(0, nowText.Length + 1);
            }               
            else
            {
                showing = false;
            }                
            textComponent.text = nowText;
            timeDelta = (textShowTime / 2f) +1;
            //Debug.Log(textShowTime);
        }
        timeDelta -= 1;
    }
    //根据配置改ui设置
    private void ChangeUI()
    {
        dialogPanel.GetComponent<CanvasGroup>().alpha = (100-ConfigData.getDlgTransparent())/100f;
        textShowTime = 100-ConfigData.getTextShowSpeed();
        //autoShowSpeed = ConfigData.getAutoSpeed();
        bgmVolume = ConfigData.getBgmVolume();
    }
    private void FHide()
    {
        hiding = true;
        HideMainUI();
    }
    private void Menu()
    {
        returnPanel.SetActive(true);
    }
    private void SureReturn()
    {
        BlackAppear();
    }
    private void CancelReturn()
    {
        returnPanel.SetActive(false);
    }
    private void SetSize(GameObject gameObject, float width, float height)
    {
        gameObject.GetComponent<RectTransform>().sizeDelta =
            new Vector2(swidth * width, sheight * height);
    }
    private void SetPosition(GameObject gameObject, float x, float y)
    {
        gameObject.GetComponent<RectTransform>().position =
            new Vector2(swidth * x, sheight * y);
    }
    public void GameOver()
    {
        SureReturn();
    }
}
