using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Reflection;


public enum Item { Big_Basket, Bag, Backpack, Chest_1, Chest_2, Box_1, Box_2, Diamond_Blue, Diamond_Green, Diamond_Pink, Skull_1, Skull_2};

public class StoreController : MonoBehaviour
{
    public float[] priceItem = { 0.99f, 1.99f, 2.99f, 1.99f, 2.99f, 2.99f, 3.99f , 0f, 0f, 0f, 0f, 0f};
    public GameObject _canvas;
    public GameObject _view;
    [SerializeField] public GameObject _panel;
    [SerializeField] public GameObject _panelBlack;
    [SerializeField] public GameObject _loadingScreen;
    [SerializeField] public GameObject _youWin;
    [SerializeField] public GameObject _openFailedBuy;
    [SerializeField] public GameObject _openFailedBuyforSingular;
    [SerializeField] private Image _failedBuyItem;
    [SerializeField] private Image _failedBuyItemforSingqular;
    [SerializeField] private TMP_Text _failedBuyItemPiece;
    JsonController _jsonController;
    public List<ItemManager> _itemCollection = new List<ItemManager>();
    public GameObject _prefabItem;

    private GameObject _temp;    
    [SerializeField] private Image _product;
    [SerializeField] private TMP_Text _pieceText;
    int tempTotalItem;
    private GameObject _instantiatedObj;
    [SerializeField] GameObject _youWinParticles;
    public bool isPaused;
    public bool _isSingularİtemCantBuy;

    [SerializeField] private GameObject _showPanel;
    Scene _currentScene;
    private TimelineControllerMain _timelineController;
    [SerializeField] private GameObject _basket_1;
    [SerializeField] private GameObject _basket_2;
    [SerializeField] private GameObject _basket_3;

    // Start is called before the first frame update
    void Start()
    {
        _currentScene = SceneManager.GetActiveScene();
        StartCoroutine(LoadingAnimation());

        // we save store entrance scene this part not necessery but we didn't set price
        /*
        if (PlayerPrefs.GetString("SaveCollectionFirstTime") == "")
        {
            saveItemCollection();
            //print("saveCollection");
            PlayerPrefs.SetString("SaveCollectionFirstTime", "true");
        } */
        
        setConnentScale();
        loadItemCollection();
        // set price 
        setPrice();
        setItem();
       
        
        //AdMobManager.BannerGizle();
        /*
        AdMobManager.RewardedAdDestroy();
        AdMobManager.RewardedReklamAl();
        */

    }
    private void Awake()
    {        
        _panelBlack.SetActive(true);
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        _jsonController = UnityEngine.Object.FindObjectOfType<JsonController>();
        _timelineController = UnityEngine.Object.FindObjectOfType<TimelineControllerMain>();

    }
    // Update is called once per frame
    void Update()
    {
        
        if (isPaused)
        {
            if (bool.Parse(PlayerPrefs.GetString("isRewardedClose")))
            {               
                //Screen.orientation = ScreenOrientation.LandscapeLeft;
                PlayerPrefs.SetString("isRewardedClose", "False");
                StartCoroutine(closeRewardedVideo());
            }
        }
        
    }
    IEnumerator LoadingAnimation()
    {
        yield return new WaitForSeconds(0.2f);
        _panelBlack.SetActive(false);
        _panel.SetActive(true);
        _loadingScreen.SetActive(true);
        //float time = UnityEngine.Random.Range(0.5f, 3f);
        yield return new WaitForSeconds(3f);
        _panel.SetActive(false);
        _loadingScreen.SetActive(false);
        
    }
    
    
    
    IEnumerator closeRewardedVideo()
    {
        //print("sdas");
        Time.timeScale = 1f;
        //Screen.orientation = ScreenOrientation.LandscapeLeft;
        yield return new WaitForSecondsRealtime(0.2f);
        //Get the method information using the method info class
        MethodInfo mi = this.GetType().GetMethod("openYouWin");

        //Invoke the method
        // (null- no parameter for the method call
        // or you can pass the array of parameters...)
        mi.Invoke(this, null);
    }

    
    void setConnentScale()
    {
        StartCoroutine(set());        
    }
    IEnumerator set()
    {
        yield return new WaitForSeconds(2f);        
        float _height = _canvas.GetComponent<RectTransform>().rect.height;
        Vector3 _local = _view.GetComponent<RectTransform>().localScale;
        _view.GetComponent<RectTransform>().localScale = new Vector3(_local.x, _height / 1440f, _local.z);
    }

    public void saveItemCollection()
    {
        bool hasItem = false;        
        string[] item = (System.Enum.GetNames(typeof(Item)));
        int totalItem = 0;
        bool isSingular = false;
        //int[] numbers = ((int[])System.Enum.GetValues(typeof(ItemNo)));
        for (int i = 0; i < item.Length; i++)
        {
            _itemCollection.Add(new ItemManager(item[i], hasItem, totalItem, isSingular, priceItem[i]));
            
        }
        _jsonController.JsonSaveItem(_itemCollection);

    }
    public void setPrice()
    {
        int count = 0;
        foreach (var item in _itemCollection)
        {
            item.Price = priceItem[count];
            count++;
        }
    }
    public void loadItemCollection()
    {
        _jsonController.JsonLoadItem();
        _itemCollection = _jsonController._itemmanager;
    }
    public void setItem()
    {        
        string _tempItem;        
        foreach (var item in _itemCollection)
        {
            _tempItem = item.Item.ToString();
            int index = (int)Enum.Parse(typeof(Item), _tempItem);
            //_ItemNo = (int)item.ItemNum;
            //print(_tempItem + " " + index + " ");            
            _prefabItem = GameObject.Find("/Canvas/Scroll/View/Content/GameObject" + "  " + "(" + index + ")" + "/View/Product");
            if (Resources.Load<Sprite>("Product/" + _tempItem) != null)
            {                
                GameObject.Find("/Canvas/Scroll/View/Content/GameObject" + "  " + "(" + index + ")" + "/View/Product/piece").SetActive(false);
                //_prefabItem.GetComponent<Image>().sprite = Resources.Load<Sprite>("Product/" + _item);                
                if (index <= 2)
                {                    
                    item.IsSingular = true;                    
                }
                if (index > 6)
                {                    
                    GameObject.Find("/Canvas/Scroll/View/Content/GameObject" + "  " + "(" + index + ")" + "/View/Product/piece").SetActive(true);
                }
                _jsonController.JsonSaveItem(_itemCollection);
                //print(GameObject.Find("/Canvas/Scroll/View/Content/GameObject" + "  " + "(" + index + ")" + "/View/Product/piece").activeSelf);

                /*if(GameObject.Find("/Canvas/Scroll/View/Content/GameObject" + "  " + "(" + index + ")" + "/View/Product/piece").activeSelf == true)
                {
                    item.IsSingular = true;
                }*/
            }
            
        }
    }


    public void buyUseAdMob()
    {
        isPaused = true;
        bool isAdReady = AdMobManager.RewardedReklamHazirMi();
        if (isAdReady)
        {            
            AdMobManager.RewardedReklamGoster(RewardedAdItem);            
        }
        else
        {           
            AdMobManager.RewardedReklamAl();
        }
        
    }
    public void buyUseDiamondorSkull(GameObject item)
    {
        GameObject _pay = item.transform.Find("GameObject").gameObject;
        Image pay = _pay.GetComponent<Image>();
        GameObject _payText = item.transform.Find("Text (TMP)").gameObject;
        TMPro.TMP_Text payText = _payText.GetComponent<TMP_Text>();

        GameObject _tempHeader = item.transform.parent.gameObject;
        GameObject _item = _tempHeader.transform.Find("Product").gameObject;
        _product.sprite = _item.GetComponent<Image>().sprite;

        if(checkItemCollectionforSingular(_product, _item))
        {
            return;
        }

        /*if (_isSingularİtemCantBuy)
        {
            //GameObject tempHeader = item.transform.parent.gameObject;
            //Image singularItem = tempHeader.transform.Find("Product").gameObject.GetComponent<Image>();
            //openFailedBuyforSingularObject(_item.GetComponent<Image>());
        }*/

        if (checkGiftValueforItemCollection(pay, payText) == true)
        {            
            _temp = _item.transform.Find("piece").transform.Find("Text (TMP)").gameObject;
            _pieceText.text = _temp.GetComponent<TMPro.TMP_Text>().text;

            setItemCollection(_product, int.Parse(_pieceText.text.ToString()));
            openYouWin();
        }
        else
        {
            openFailedBuy(pay, tempTotalItem);           
        }




        /*if(checkItemCollection(pay, payText) == true)
        {
            _item = item;
            _product.sprite = item.GetComponent<Image>().sprite;
            _temp = item.transform.Find("piece").transform.Find("Text (TMP)").gameObject;
            _pieceText.text = _temp.GetComponent<TMPro.TMP_Text>().text;
            openYouWin();
        }
        else
        {
            openFailedBuy(pay,tempTotalItem);
        }
        //_temp = GameObject.Find("/Canvas/Scroll/View/Content/GameObject/" + "  " + "(" + 0 + ")" + "/View/Product");
        */
    }
    public bool checkGiftValueforItemCollection(Image pay, TMPro.TMP_Text payText)
    {
        loadItemCollection();
        foreach (var item in _itemCollection)
        {            
            if (pay.sprite.name == item.Item)
            {
                if(item.TotalItem >= int.Parse(payText.text.ToString())){
                    print(item.Item + "." + item.HasItem + "." + item.IsSingular);                    
                    item.TotalItem -= int.Parse(payText.text.ToString());
                    _jsonController.JsonSaveItem(_itemCollection);
                    return true;
                }
                else
                {
                    tempTotalItem = item.TotalItem;                    
                }
                //_jsonController.JsonSaveItem(_itemCollection);
            }
        }
        return false;
    }
    public bool checkItemCollectionforSingular(Image _object ,GameObject _item)
    {
        foreach (var item in _itemCollection)
        {
            if (_object.sprite.name == item.Item)
            {
                if(item.IsSingular == true && item.HasItem == true)
                {
                    //print("ssss");
                    //_isSingularİtemCantBuy = true;
                    openFailedBuyforSingularObject(_item.GetComponent<Image>());
                    return true;
                }
            }
        }
        return false;
    }
    public void openFailedBuy(Image pay,int totalItem)
    {
        _failedBuyItem.sprite = pay.sprite;
        _failedBuyItemPiece.text = totalItem.ToString();
        _panel.SetActive(true);
        _openFailedBuy.SetActive(true);
        StartCoroutine(closeFailedBuy(_openFailedBuy,_panel));
    }
    public void openFailedBuyforSingularObject(Image singularItem)
    {
        _failedBuyItemforSingqular.sprite = singularItem.sprite;
        //_failedBuyItemPiece.text = totalItem.ToString();
        _panel.SetActive(true);
        _openFailedBuyforSingular.SetActive(true);
        StartCoroutine(closeFailedBuy(_openFailedBuyforSingular, _panel));
    }
    IEnumerator closeFailedBuy(GameObject _object1 , GameObject _object2) //(GameObject _object,string methodName)
    {
        yield return new WaitForSecondsRealtime(2f);
        _object1.SetActive(false);
        _object2.SetActive(false);
        /*
        _object.SetActive(false);
        yield return new WaitForSecondsRealtime(0.1f);

        //Get the method information using the method info class
        MethodInfo mi = this.GetType().GetMethod(methodName);

        //Invoke the method
        // (null- no parameter for the method call
        // or you can pass the array of parameters...)
        mi.Invoke(this, null);

        */
    }


    public void RewardedAdItem(GoogleMobileAds.Api.Reward odul)
    {

        //_temp = GameObject.Find("/Canvas/Scroll/View/Content/GameObject/" + "  " + "(" + 0 + ")" + "/View/Product");
        //int heart = PlayerPrefs.GetInt("Heart") + 3;
        //PlayerPrefs.SetInt("Heart", heart);

        setItemCollection(_product, int.Parse(_pieceText.text.ToString()));
        //openYouWin();
        /*
        //Get the method information using the method info class
        MethodInfo mi = this.GetType().GetMethod("openYouWin");

        //Invoke the method
        // (null- no parameter for the method call
        // or you can pass the array of parameters...)
        mi.Invoke(this, null);
        */
    }
    public void openYouWin()
    {
        //GameObject.FindGameObjectWithTag("Music").GetComponent<MusicClass>().PlayMusic();
        isPaused = false;
        _youWin.SetActive(true);
        _panel.SetActive(true);
        //_product.sprite = _temp.GetComponent<Image>().sprite; // we did in onclicked function
        Vector2 _pos = new Vector2(_youWinParticles.transform.position.x, 5f);
        _instantiatedObj = Instantiate(_youWinParticles, _pos, Quaternion.identity);
    }
    public void setItemCollection(Image _object, int piece)
    {
        foreach (var item in _itemCollection)
        {
            if (_object.sprite.name == item.Item)
            {
                if (item.HasItem == false) { item.HasItem = true; }
                item.TotalItem += piece;
                _jsonController.JsonSaveItem(_itemCollection);
            }
        }
    }
    public void Collect()
    {
        _youWin.SetActive(false);
        _panel.SetActive(false);
        Destroy(_instantiatedObj);
    }


    public void OnClicked(GameObject item )
    {    

        /*_item = item;
        _product.sprite = item.GetComponent<Image>().sprite;
        _temp = item.transform.Find("piece").transform.Find("Text (TMP)").gameObject;
        _pieceText.text = _temp.GetComponent<TMPro.TMP_Text>().text;*/
    }
    public void OnClickedforAd(GameObject item)
    {
        // method for find which object clicked. we use for find which item

        // _temp = button.transform.root.gameObject; // button for root gameobject-example
        // _temp = item.transform.parent.gameObject; // product for parent-example       
        _product.sprite = item.GetComponent<Image>().sprite;
        _temp = item.transform.Find("piece").transform.Find("Text (TMP)").gameObject;        
        _pieceText.text = _temp.GetComponent<TMPro.TMP_Text>().text;
    }


    public void back()
    {
        SceneManager.LoadScene("MainScene");
    }
}
