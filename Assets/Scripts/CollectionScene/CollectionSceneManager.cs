using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.SceneManagement;

public class CollectionSceneManager : MonoBehaviour
{   
    
    JsonController _jsonController;
    [HideInInspector] public List<EggsManager> _eggCollection = new List<EggsManager>();
    [HideInInspector] public List<ItemManager> _itemCollection = new List<ItemManager>();    
    [HideInInspector] public GameObject _egg;
    [HideInInspector] public GameObject _item;
    public GameObject _panel;
    public GameObject _useItemScreen;
    public Image _product;

    public GameObject _canvas;
    public GameObject _connent;

    //public TMP_Text _typeText;
    [HideInInspector] public GameObject _eggTYpeText;
    //[SerializeField]public GameObject[] _list;

    [SerializeField] private GameObject _showPanel;
    Scene _currentScene;
    private TimelineControllerMain _timelineControllerMain;
    [SerializeField] private GameObject _basket_1;
    [SerializeField] private GameObject _basket_2;
    [SerializeField] private GameObject _basket_3;
    [SerializeField] private GameObject _showBasket_1;
    [SerializeField] private GameObject _showBasket_2;
    [SerializeField] private GameObject _showBasket_Buy;
    [SerializeField] private Button _back;
    [SerializeField] private Button _left;
    [SerializeField] private Button _right;

    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        setConnentScale();
        checkEgg();
        setText();
        setItemPiece();
        checkItem();
        setTextItem();        
        _currentScene = SceneManager.GetActiveScene();
        if (PlayerPrefs.GetString("ShowTimelineCollection") == "true" || PlayerPrefs.GetString("ShowTimelineCollection") == "")
        {
            StartCoroutine(_showAnim());
            PlayerPrefs.SetString("ShowTimelineCollection", "false");
        }

        //_egg.GetComponent<Image>().sprite = Resources.Load<Sprite>("Eggs/" + "EasterEast" + "/" + "Easter" + " " + 2);
        //_egg = GameObject.Find("/Canvas/Scroll");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void Awake()
    {
        _jsonController = UnityEngine.Object.FindObjectOfType<JsonController>();
        _timelineControllerMain = UnityEngine.Object.FindObjectOfType<TimelineControllerMain>();
        loadEggCollection();
        loadItemCollection();
    }
    IEnumerator _showAnim()
    {
        //print("sss");
        Time.timeScale = 0f;
        //_timelineController = UnityEngine.Object.FindObjectOfType<TimelineControllerMain>();
        GameObject _handHeader = UnityEngine.Object.FindObjectOfType<DontDestroy>().gameObject;
        _handHeader.GetComponent<CanvasScaler>().matchWidthOrHeight = 1f;
        GameObject _hand = _handHeader.transform.Find("Hand").gameObject;
        _hand.SetActive(true);
        _showPanel.SetActive(true);
        _showBasket_1.SetActive(true);
        _back.interactable = false;
        _left.interactable = false;
        _right.interactable = false;
        _left.GetComponent<ScrollButton>().enabled = false;
        _right.GetComponent<ScrollButton>().enabled = false;

        _basket_1.gameObject.GetComponent<Canvas>().overrideSorting = true;
        _basket_1.gameObject.GetComponent<Canvas>().sortingOrder = 3;
        _basket_1.transform.Find("Lock").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Product/Big_Basket");

        _basket_2.gameObject.GetComponent<Canvas>().overrideSorting = true;
        _basket_2.gameObject.GetComponent<Canvas>().sortingOrder = 3;
        _basket_2.transform.Find("Lock").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Product/Bag");

        _basket_3.gameObject.GetComponent<Canvas>().overrideSorting = true;
        _basket_3.gameObject.GetComponent<Canvas>().sortingOrder = 3;
        _basket_3.transform.Find("Lock").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Product/Backpack");

        _timelineControllerMain.play(_currentScene.name.ToString()); // timeline here.
        yield return new WaitForSecondsRealtime(3f);        
        _showBasket_1.SetActive(false);
        _showBasket_2.SetActive(true);
        yield return new WaitForSecondsRealtime(3f);
        _showBasket_2.SetActive(false);
        _showBasket_Buy.SetActive(true);
        yield return new WaitForSecondsRealtime(2f);
        _basket_1.transform.Find("Lock").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Lock");
        _basket_1.gameObject.GetComponent<Canvas>().overrideSorting = false;
        _basket_2.transform.Find("Lock").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Lock");
        _basket_2.gameObject.GetComponent<Canvas>().overrideSorting = false;
        _basket_3.transform.Find("Lock").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Lock");
        _basket_3.gameObject.GetComponent<Canvas>().overrideSorting = false;

        _showBasket_Buy.SetActive(false);
        _back.interactable = true;
        _left.interactable = true;
        _right.interactable = true;
        _left.GetComponent<ScrollButton>().enabled = true;
        _right.GetComponent<ScrollButton>().enabled = true;
        _hand.SetActive(false);
        _showPanel.SetActive(false);
        _handHeader.GetComponent<CanvasScaler>().matchWidthOrHeight = 0f;
        //Time.timeScale = 1f;    
    }
    void setConnentScale()
    {
        float _width = _canvas.GetComponent<RectTransform>().rect.width;
        Vector3 _local = _connent.GetComponent<RectTransform>().localScale;
        _connent.GetComponent<RectTransform>().localScale = new Vector3(_width / 1440f, _local.y, _local.z);
    }
    void setItemPiece()
    {
        for (int i = 0; i < 12; i++)
        {
            GameObject.Find("/Canvas/Scroll/View/Content/GameObject/View/Content/" + i + "/piece").SetActive(false);
        }
    }
    public void setText()
    {
        int index = 0;
        foreach (string s in Enum.GetNames(typeof(EggTypes)))
        {
            _eggTYpeText = GameObject.Find("/Canvas/Scroll/View/Content/GameObject" + "  " + "(" + index + ")" + "/CollectionName/Text (TMP)");
            _eggTYpeText.GetComponent<TMPro.TMP_Text>().text = s;                 
            index++;
        }
    }


    public void loadEggCollection()
    {
        _jsonController.JsonLoad();
        _eggCollection = _jsonController._eggtypeList;
    }
    public void checkEgg()
    {           
        string eggtype;       
        int eggnum;
        foreach (var egg in _eggCollection)
        {            
            if (egg.HasEgg == true)
            {                
                eggtype = egg.EggType.ToString();
                int index = (int)Enum.Parse(typeof(EggTypes), eggtype);
                eggnum = (int)egg.EggNumber;    
                //print(eggtype + " " + index + " "+ eggnum);
                int eggNumber = eggnum + 1;
                //_egg.GetComponent<Image>().sprite = Resources.Load<Sprite>("Eggs/" + _eggType + "/" + _eggType + " " + _eggNumber);
                _egg = GameObject.Find("/Canvas/Scroll/View/Content/GameObject" + "  " + "(" + index + ")" + "/View/Content/" + eggnum + "/Lock");
                if (Resources.Load<Sprite>("Eggs/" + eggtype + "/" + eggtype + " " + eggnum) != null)
                {                    
                    _egg.GetComponent<Image>().sprite = Resources.Load<Sprite>("Eggs/" + eggtype + "/" + eggtype + " " + eggnum);
                }   
            }

        }
    }

    public void loadItemCollection()
    {
        
        _jsonController.JsonLoadItem();
        _itemCollection = _jsonController._itemmanager;
    }

    public void setTextItem()
    {
        _eggTYpeText = GameObject.Find("/Canvas/Scroll/View/Content/GameObject/CollectionName/Text (TMP)");
        _eggTYpeText.GetComponent<TMPro.TMP_Text>().text = "Item";        
    }

    public void checkItem()
    {
       
        string _Item;
        //int eggnum;
        foreach (var item in _itemCollection)
        {
            _Item = item.Item.ToString();
            int index = (int)Enum.Parse(typeof(Item), _Item);
            
            if (item.HasItem == true)
            {                
                //eggnum = (int)egg.EggNumber;
                //print(eggtype + " " + index + " "+ eggnum);
                //int eggNumber = eggnum + 1;
                //_egg.GetComponent<Image>().sprite = Resources.Load<Sprite>("Eggs/" + _eggType + "/" + _eggType + " " + _eggNumber);
                _item = GameObject.Find("/Canvas/Scroll/View/Content/GameObject/View/Content/" + index + "/Lock");                
                if (Resources.Load<Sprite>("Product/" + _Item) != null)
                {
                    if (index <= 6)
                    {
                        GameObject.Find("/Canvas/Scroll/View/Content/GameObject/View/Content/" + index).GetComponent<Image>().raycastTarget = true;
                        GameObject.Find("/Canvas/Scroll/View/Content/GameObject/View/Content/" + index).GetComponent<Button>().interactable = true;                        
                    }
                    _item.GetComponent<Image>().sprite = Resources.Load<Sprite>("Product/" + _Item);
                    if (index > 2)
                    {
                        GameObject.Find("/Canvas/Scroll/View/Content/GameObject/View/Content/" + index + "/piece").SetActive(true);
                    }                                       
                    GameObject.Find("/Canvas/Scroll/View/Content/GameObject/View/Content/" + index + "/piece/Text (TMP)").GetComponent<TMP_Text>().text = item.TotalItem.ToString();
                }
                
            }            
        }
        
    }


    public void back()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void openUseItemScreen(GameObject item)
    {
        _panel.SetActive(true);
        _useItemScreen.SetActive(true);
        //GameObject _tempHeader = item.transform.parent.gameObject;
        GameObject _item = item.transform.Find("Lock").gameObject;
        _product.sprite = _item.GetComponent<Image>().sprite;
    }
    public void noButton() // same CancelButton
    {
        _panel.SetActive(false);
        _useItemScreen.SetActive(false);
    }
    public void yesButton()
    {
        String _basketName = _product.sprite.name;
        print(_basketName);
        PlayerPrefs.SetString("BasketType", _basketName);
        PlayerPrefs.SetInt("BasketCounter", 5);
        SetItemStatus(_basketName);

        //loadItemCollection();
        //checkItem();
        _panel.SetActive(false);
        _useItemScreen.SetActive(false);        
    }

    public void SetItemStatus(String _itemName)
    {
        foreach (var item in _itemCollection)
        {
            if (_itemName == item.Item)
            {
                item.HasItem = false;
                item.TotalItem = 0;
                int index = (int)Enum.Parse(typeof(Item), item.Item);
                GameObject.Find("/Canvas/Scroll/View/Content/GameObject/View/Content/" + index + "/piece").SetActive(false);
                GameObject.Find("/Canvas/Scroll/View/Content/GameObject/View/Content/" + index + "/Lock").GetComponent<Image>().sprite = Resources.Load<Sprite>("Lock");
                GameObject.Find("/Canvas/Scroll/View/Content/GameObject/View/Content/" + index).GetComponent<Button>().interactable = false;
                GameObject.Find("/Canvas/Scroll/View/Content/GameObject/View/Content/" + index).GetComponent<Image>().raycastTarget = false;
                _jsonController.JsonSaveItem(_itemCollection);
            }
        }
            
    }

   

}
