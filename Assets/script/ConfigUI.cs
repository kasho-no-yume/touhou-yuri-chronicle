/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfigUI:MonoBehaviour
{
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
    private float swidth;
    private float sheight;
    private static ConfigUI instance;
    public static ConfigUI GetInstance()
    {
        if(instance==null)
        {
            instance = new ConfigUI();
        }
        return instance;
    }
    void Start()
    {
        swidth = UnityEngine.Screen.width;
        sheight = UnityEngine.Screen.height;
    }
    public void ShowUI()
    {
        panelconfig.SetActive(true);
        SetPosition(panelconfig, 0.05f, 0.05f);
        SetSize(panelconfig, 0.9f, 0.9f);
        btncloseconfig.GetComponent<Button>().onClick.AddListener(CloseConfig);
        btncloseconfig.GetComponent<RectTransform>().position =
            new Vector2(swidth * 18 / 20, sheight * 18 / 20);
        btncloseconfig.GetComponent<RectTransform>().sizeDelta =
            new Vector2(sheight / 10, sheight / 10);
        float pwidth = panelconfig.GetComponent<RectTransform>().sizeDelta.x;
        float pheight = panelconfig.GetComponent<RectTransform>().sizeDelta.y;
        GameObject[] textConfig = new GameObject[5]
        {textdlgtransparent,textbgmvolume,textautospeed,textshowspeed,textskipunread};
        GameObject[] itemConfig = new GameObject[5]
        {sliderdlgtransparent,sliderbgmvolume,sliderautospeed,slidershowspeed,toggleskipunread};
    }
    public void CloseConfig()
    {
        panelconfig.SetActive(false);
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
}
*/