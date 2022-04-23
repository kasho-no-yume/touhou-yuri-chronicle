using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ThemeUiMgr : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject musicController;
    private AudioSource audioSource;
    public GameObject btnstart;
    public GameObject btnload;
    public GameObject btngallary;
    public GameObject btnconfig;
    public GameObject btnexit;
    public GameObject panelconfig;
    public GameObject btncloseconfig;
    public GameObject textdlgtransparent;
    public GameObject sliderdlgtransparent;
    public GameObject textbgmvolume;
    public GameObject sliderbgmvolume;
    public GameObject textautospeed;
    public GameObject sliderautospeed;
    public GameObject textshowspeed;
    public GameObject slidershowspeed;
    public GameObject textskipunread;
    public GameObject toggleskipunread;
    public GameObject saveconfig;
    public GameObject blackboard;
    private CanvasGroup cgbb;
    private float timeDisappear=300f;
    private float timeAppear = -1;
    private int swidth;
    private int sheight;
    void Start()
    {
        ConfigData.initClass();
        audioSource = musicController.GetComponent<AudioSource>();
        cgbb = blackboard.GetComponent<CanvasGroup>();
        btnstart.GetComponent<Button>().onClick.AddListener(BtnStart);
        btnload.GetComponent<Button>().onClick.AddListener(BtnLoad);
        btngallary.GetComponent<Button>().onClick.AddListener(BtnGallary);
        btnconfig.GetComponent<Button>().onClick.AddListener(BtnConfig);
        btnexit.GetComponent<Button>().onClick.AddListener(BtnExit);
        saveconfig.GetComponent<Button>().onClick.AddListener(SaveConfig);
        swidth = UnityEngine.Screen.width;
        sheight = UnityEngine.Screen.height;
        GameObject[] imgButtons = new GameObject[5] { btnstart, btnload, btngallary, btnconfig, btnexit };
        /*for (int i = 0; i < 5; i++)
        {
            imgButtons[i].GetComponent<RectTransform>().position =
                new Vector2((swidth / 26) * (1 + (5 * i)), sheight / 10);
            imgButtons[i].GetComponent<RectTransform>().sizeDelta =
                new Vector2(swidth * 4 / 26, sheight / 15);
        }*/
        audioSource.volume = ConfigData.getBgmVolume();
        audioSource.Play();
    }
    void FixedUpdate()
    {
        timeAppear -= 3f;
        timeDisappear -= 3;
        if(timeDisappear>0)
        {
            cgbb.alpha = (timeDisappear) / 300f;
        }
        else if(timeDisappear==0)
        {
            blackboard.SetActive(false);
        }
        if(timeAppear>0)
        {
            cgbb.alpha = (300f - timeAppear) / 300f;
        }
        else if(timeAppear==0)
        {
            SceneManager.LoadScene("Gaming");
        }

    }

    private void HideMainUI()
    {
        btnstart.SetActive(false);
        btnload.SetActive(false);
        btngallary.SetActive(false);
        btnexit.SetActive(false);
        btnconfig.SetActive(false);
    }
    private void ShowMainUI()
    {
        btnstart.SetActive(true);
        btnload.SetActive(true);
        btngallary.SetActive(true);
        btnexit.SetActive(true);
        btnconfig.SetActive(true);
    }

    void BtnStart()
    {
        blackboard.SetActive(true);
        timeAppear = 300f;
        GameLogic.ChangeSceneBySave(new SaveFile());
        
    }
    void BtnLoad()
    {
        SLUIMgr.instance.ShowPanel(SLUIMgr.Statue.load);
    }
    void BtnGallary()
    {

    }
    private void onVolumeChange(float vol)
    {
        audioSource.volume = vol/100f;
    }
    void BtnConfig()
    {
        HideMainUI();
        panelconfig.SetActive(true);
        GameObject[] textConfig = new GameObject[5]
        {textdlgtransparent,textbgmvolume,textautospeed,textshowspeed,textskipunread};
        GameObject[] itemConfig = new GameObject[5]
        {sliderdlgtransparent,sliderbgmvolume,sliderautospeed,slidershowspeed,toggleskipunread};
        itemConfig[0].GetComponent<Slider>().value = ConfigData.getDlgTransparent();
        itemConfig[1].GetComponent<Slider>().value = ConfigData.getBgmVolume();
        itemConfig[2].GetComponent<Slider>().value = ConfigData.getAutoSpeed();
        itemConfig[3].GetComponent<Slider>().value = ConfigData.getTextShowSpeed();
        itemConfig[4].GetComponent<Toggle>().isOn = ConfigData.getSkipUnread();
        itemConfig[1].GetComponent<Slider>().onValueChanged.AddListener(onVolumeChange);
        /*SetPosition(panelconfig, 0.05f, 0.05f);
        SetSize(panelconfig, 0.9f, 0.9f);*/
        btncloseconfig.GetComponent<Button>().onClick.AddListener(CloseConfig);
        /*btncloseconfig.GetComponent<RectTransform>().position =
            new Vector2(swidth * 18 / 20, sheight * 18 / 20);
        btncloseconfig.GetComponent<RectTransform>().sizeDelta =
            new Vector2(sheight / 10, sheight / 10);
        float pwidth = panelconfig.GetComponent<RectTransform>().sizeDelta.x;
        float pheight = panelconfig.GetComponent<RectTransform>().sizeDelta.y;
        for(int i=0;i<5;i++)
        {
            textConfig[i].GetComponent<RectTransform>().localPosition =           
                new Vector2(pwidth / 10, pheight * (9 - i*2) / 10);
            textConfig[i].GetComponent<RectTransform>().sizeDelta =
                new Vector2(pwidth / 5, pheight / 15);
            itemConfig[i].GetComponent<RectTransform>().localPosition =
                new Vector2(pwidth / 3, 0);
            itemConfig[i].GetComponent<RectTransform>().sizeDelta =
                new Vector2(pwidth/3,pheight/20);
        }
        saveconfig.GetComponent<RectTransform>().localPosition =
            new Vector2(pwidth * 4 / 5, pheight / 10);*/
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
        CloseConfig();
    }
    void BtnExit()
    {
        Application.Quit();
    }
    //全部是相对屏幕的位置,占屏幕大小的比例
    private void SetSize(GameObject gameObject,float width,float height)
    {
        gameObject.GetComponent<RectTransform>().sizeDelta =
            new Vector2(swidth * width, sheight * height);
    }
    private void SetPosition(GameObject gameObject,float x,float y)
    {
        gameObject.GetComponent<RectTransform>().position =
            new Vector2(swidth * x, sheight * y);
    }
}
