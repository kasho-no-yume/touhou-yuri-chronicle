using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//存档读档界面ui管理类
public class SLUIMgr : MonoBehaviour
{
    // Start is called before the first frame update
    public enum Statue { save,load};
    public static SLUIMgr instance { get; private set; }
    public Statue currentStatue;
    private SaveFile selectedSave;
    public GameObject slInterface;
    private GameObject button1;
    private GameObject button2;
    private GameObject textPreview;
    private GameObject scrollView;
    private GameObject close;
    private GameObject content;

    void Awake()
    {
        GetComponent();
        instance = this;
        close.GetComponent<Button>().onClick.AddListener(CloseSL);
        CloseSL();
    }
    public void ShowPanel(Statue s)
    {
        slInterface.SetActive(true);
        currentStatue = s;
        InitPanel(s);
        UpdateList();

    }
    //
    private void InitPanel(Statue s)
    {        
        textPreview.GetComponent<Text>().text = "";
        if(s==Statue.load)
        {
            button1.SetActive(false);
        }
        else
        {
            button1.SetActive(true);
            button1.transform.Find("Text").GetComponent<Text>().text = "新建存档";
            button1.GetComponent<Button>().onClick.RemoveAllListeners();
            button1.GetComponent<Button>().onClick.AddListener(SaveNew);
        }
        button2.SetActive(false);
        close.GetComponent<Button>().onClick.AddListener(CloseSL);
        UpdateList();
    }
    private void GetComponent()
    {

        button1 = GameObject.Find("Button1");
        button2 = GameObject.Find("Button2");
        textPreview = GameObject.Find("Preview");
        scrollView = GameObject.Find("ScrollView");
        close = GameObject.Find("CloseSL");
        content = GameObject.Find("Content");
    }
    private void CloseSL()
    {
        slInterface.SetActive(false);
    }
    private void SaveNew()
    {
        SaveFile save = new SaveFile();
        save.position = GameLogic.globalProgress;
        save.characterKoishi = GameLogic.characterKoishi;
        save.characterSatori = GameLogic.characterSatori;
        save.bgm = GameLogic.bgm;
        save.background = GameLogic.background;
        save.previewText = GameLogic.previewText;
        SLData.SaveNew(save);
        InitPanel(currentStatue);
        UpdateList();
    }
    private void ReplaceOf(SaveFile selectedFile)
    {
        SaveFile save = new SaveFile();
        save.position = GameLogic.globalProgress;
        save.characterKoishi = GameLogic.characterKoishi;
        save.characterSatori = GameLogic.characterSatori;
        save.bgm = GameLogic.bgm;
        save.background = GameLogic.background;
        save.previewText = GameLogic.previewText;
        SLData.ReplaceSaveOf(selectedFile, save);
        InitPanel(currentStatue);
    }
    private void UpdateList()
    {
        List <SaveFile> saves= SLData.GetSaves();
        int i = 1;
        for(int ii=0;ii<content.transform.childCount;ii++)
        {
            GameObject.Destroy(content.transform.GetChild(ii).gameObject);
        }
        foreach(SaveFile s in saves)
        {
            GameObject gameObject = Instantiate(Resources.Load("perfab/SaveItem") as GameObject);
            gameObject.transform.Find("Text").GetComponent<Text>().text = "Save " + i + ":" + s.previewText;
            gameObject.GetComponent<Button>().onClick.AddListener(delegate() { SetSelectedSave(s); });
            gameObject.transform.SetParent(content.transform);
            gameObject.GetComponent<RectTransform>().sizeDelta =
                new Vector2(UnityEngine.Screen.width / 2, UnityEngine.Screen.height/7);
            /*gameObject.GetComponent<RectTransform>().localPosition = 
                new Vector2(0, UnityEngine.Screen.height / 7 * (1-i));*/
            content.GetComponent<RectTransform>().sizeDelta = new Vector2(0, UnityEngine.Screen.height / 7 * (i-7));
            i++;
        }
    }
    public void SetSelectedSave(SaveFile saveFile)
    {
        selectedSave = saveFile;
        textPreview.GetComponent<Text>().text = saveFile.previewText;
        button1.SetActive(true);
        switch(currentStatue)
        {
            case Statue.load:
                button1.transform.Find("Text").GetComponent<Text>().text = "加载存档";
                button1.GetComponent<Button>().onClick.RemoveAllListeners();
                button1.GetComponent<Button>().onClick.AddListener(delegate() { LoadSave(selectedSave); });              
                break;
            case Statue.save:
                button1.transform.Find("Text").GetComponent<Text>().text = "覆盖存档";
                button1.GetComponent<Button>().onClick.RemoveAllListeners();
                button1.GetComponent<Button>().onClick.AddListener(delegate () { ReplaceOf(selectedSave); });
                break;
        }
        button2.SetActive(true);
        button2.GetComponent<Button>().onClick.RemoveAllListeners();
        button2.transform.Find("Text").GetComponent<Text>().text = "删除存档";
        button2.GetComponent<Button>().onClick.AddListener(delegate () { DeleteSave(selectedSave); });
    }
    private void LoadSave(SaveFile save)
    {
        GameLogic.ChangeSceneBySave(save);
        CloseSL();
        SceneManager.LoadScene("Gaming");
    }
    private void DeleteSave(SaveFile save)
    {
        SLData.DeleteSave(save);
        InitPanel(currentStatue);
        UpdateList();
    }
}
