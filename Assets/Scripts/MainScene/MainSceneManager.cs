using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Reflection;
using UnityEngine.EventSystems;


public class MainSceneManager : MonoBehaviour
{
    public AudioSource _audioSource;
    private ContentController _contentController;
    private JsonController _jsonController;
    public AudioSource _clickforVolumeandSound;
    public TimelineControllerMain _timelineControllerMain;

    [SerializeField] private Toggle _volume, _sound;
    [SerializeField] private TMP_Text _coinText;
    [SerializeField] public GameObject _panel;
    [SerializeField] public GameObject _panelBlack;
    [SerializeField] public GameObject _settingScreen;
    [SerializeField] public GameObject _helpScreen;
    [SerializeField] public GameObject _quitScreen;
    [SerializeField] public GameObject _loadingScreen;
    [SerializeField] public GameObject _failedScreenHeart;
    [SerializeField] public GameObject _failedScreenCoin;
    [SerializeField] private TMP_Text _failedcoinText;
    [SerializeField] public GameObject _shopSmall;
    [SerializeField] public GameObject _youWin;

    public Canvas _canvas_1;
    //[SerializeField] public Text _text;

    Vector2[] map1 = new Vector2[40];
    Vector2[] map2 = new Vector2[40];
    Vector2[] map3 = new Vector2[40];
    Vector2[] map4 = new Vector2[40];
    Vector2[] map5 = new Vector2[40];    

    private GameObject _tempLevelObject;
    private GameObject _instantiatedObj;
    [SerializeField] GameObject _youWinParticles;
    public GameObject _rabbit;
    private GameObject _star1;
    private GameObject _star2;
    private GameObject _star3;
    private GameObject _levelText;

    [SerializeField] private GameObject _store;
    [SerializeField] private GameObject _collection;
    [SerializeField] private GameObject _showPanel;
    [SerializeField] private GameObject _showCollection;
    [SerializeField] private GameObject _showShop;

    public Sprite _star1Sprite;
    public Sprite _star2Sprite;
    public Sprite _star3Sprite;
    public Sprite _levelSprite;
    public Sprite _levelEmptySprite;
    public Sprite _currentlevelSprite;       
      
    public List<LevelManager> _levelManager = new List<LevelManager>();
    int _currentOrder =-1;    
    //int _currentOrderPos = -1;
    int _currentLoopNum;
    int _levelNum;
    int _itemNumber;
    public bool isPaused; // for admob
    public bool isGameReady;
    GameObject _SelectedLevel;

    Scene _currentScene;
    // Start is called before the first frame update
    void Start()
    {        
        StartCoroutine(LoadingAnimation());
        /*
        AdMobManager.RewardedAdDestroy();
        AdMobManager.RewardedReklamAl();
        */

        //AdMobManager.BannerGoster();
        setLevelMap();
        LoadLevel();
        _currentScene = SceneManager.GetActiveScene();

        //_currentOrder = _contentController._loopOrder;
        //_currentOrderPos = _contentController._loopPos;


    }
    private void Awake()
    {
        //PlayerPrefs.SetInt("VolumeOn", 1);
        //PlayerPrefs.SetInt("SoundOn", 1);
        
        _panelBlack.SetActive(true);
        Screen.orientation = ScreenOrientation.Portrait;
        checkSetttings();
        _jsonController = UnityEngine.Object.FindObjectOfType<JsonController>();
        _contentController = UnityEngine.Object.FindObjectOfType<ContentController>();
        _timelineControllerMain = UnityEngine.Object.FindObjectOfType<TimelineControllerMain>();
        
        /*PlayerPrefs.SetInt("Heart", 0);
        PlayerPrefs.SetFloat("Time", 30);
        PlayerPrefs.SetInt("TotalCoin", 150);*/
    }

    // Update is called once per frame
    void Update()
    {
        if (isPaused)
        {
            if (bool.Parse(PlayerPrefs.GetString("isRewardedClose")))
            {                
                PlayerPrefs.SetString("isRewardedClose", "False");                
                StartCoroutine(closeRewardedVideo());

                // first mehtod without time delay
                /*
                //Get the method information using the method info class
                MethodInfo mi = this.GetType().GetMethod("openYouWin");

                //Invoke the method
                // (null- no parameter for the method call
                // or you can pass the array of parameters...)
                mi.Invoke(this, null);     
                */

                //this.Invoke("Print", "A message to print (1)", 0f); // code-completes // ınvoke with parameters and time
                //Invoke("Print", "A message to print (2)", 1f); // doesn't code-complete but still works

                // second method
                //Invoke("openYouWin", 0.01f);
            }
        }
        

        if (_currentOrder !=(int)_contentController._loopOrder)
        {
            // for not create new objects
            if( (PlayerPrefs.GetInt("Level")-1) / 40 < _contentController._loopOrder -5)
            {
                return;
            }
            // for setPassive rabbit
            if(_rabbit.activeSelf == true && (_currentLoopNum-1 < _currentOrder || _currentLoopNum - 1 > _currentOrder))
            {
                _rabbit.SetActive(false);
            }
            _currentOrder = (int)_contentController._loopOrder;            

            setlevelOrder(_currentOrder);
            //_currentOrder = (_contentController._loopOrder + 1) % 5;
            //_currentOrderLoop = _contentController._loopOrder / 5;
            setlevelOrder(_currentOrder+1);
            //_currentOrder = (_contentController._loopOrder -1) % 5;
            //_currentOrderLoop = _contentController._loopOrder / 5;
            if(_currentOrder != 0) { setlevelOrder(_currentOrder - 1); }          
        }


        /*if(_currentOrderPos != _contentController._loopPos)
        {
            //_currentOrderLoop = _contentController._loopOrder / 5;
            _currentOrder = _contentController._loopOrder % 5;
            if(_contentController._loopPos == -1)
            {
                if (_currentOrder == 0)
                {
                    setlevelOrder(4);
                }
                else
                {
                    setlevelOrder(_currentOrder + _contentController._loopPos);
                }
                
            }
            else
            {
                if (_currentOrder == 4)
                {
                    setlevelOrder(0);
                }
                else
                {
                    setlevelOrder(_currentOrder + _contentController._loopPos);
                }
            }
            _currentOrderPos = _contentController._loopPos;
            switch (_currentOrder)
            {
                case 0:
                    setlevelOrder(4);
                    break;
                case 1:
                case 2:
                case 3:
                    setlevelOrder(_currentOrder + _contentController._loopPos);
                    break;
                case 4:
                    setlevelOrder(0);
                    break;
            }
            if (_currentOrder != _contentController._loopOrder % 5){

            }
        }
        if(_currentOrder != _contentController._loopOrder % 5 )
        {     
            _currentOrder = (int)_contentController._loopOrder % 5;
            _currentOrderLoop = (int)_contentController._loopOrder / 5;
            print(_currentOrder + ",,,,,");
            print(_currentOrderLoop + ".....");
            setlevelOrder(_currentOrder);
            if (_contentController._loopNum == 1)
            {
                setlevelOrder(_currentOrder + 1);
            }
            else
            {
                if(_currentOrder == 0)
                { setlevelOrder(4); }
                else { setlevelOrder(_currentOrder - 1); }                
            }            
        }*/
    }

    IEnumerator closeRewardedVideo()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        //SceneManager.LoadScene("GameScene");
        Time.timeScale = 1f;
        //yield return new WaitForSecondsRealtime(0.2f);
        //Get the method information using the method info class
        MethodInfo mi = this.GetType().GetMethod("openYouWin");

        //Invoke the method
        // (null- no parameter for the method call
        // or you can pass the array of parameters...)
        mi.Invoke(this, null);
    }

    // we use this later for add pause 
    /*void OnApplicationPause(bool hasFocus)
    {
        isPaused = hasFocus;
        print(isPaused);
    }*/
    IEnumerator LoadingAnimation()
    {        
        yield return new WaitForSeconds(0.2f);
        _panelBlack.SetActive(false);
        _panel.SetActive(true);
        _loadingScreen.SetActive(true);        
        yield return new WaitForSeconds(0.5f);
        _contentController.setContentPos();
        float time = UnityEngine.Random.Range(1f, 3f);
        yield return new WaitForSeconds(time);

        isGameReady = true; // for volume and sound;
        /*
        while (time >= 0)
        {
            time -= Time.timeScale;
            yield return new WaitForSeconds(0.3f);

        }
        */
        _panel.SetActive(false);
        _loadingScreen.SetActive(false);
        if (PlayerPrefs.GetString("ShowTimelineMain") == "true" || PlayerPrefs.GetString("ShowTimelineMain") == "")
        {
            StartCoroutine(_showAnim());
            PlayerPrefs.SetString("ShowTimelineMain", "false");
        }
    }

    IEnumerator _showAnim()
    {
        //print("sss");
        //_timelineController = UnityEngine.Object.FindObjectOfType<TimelineControllerMain>();
        GameObject _handHeader = UnityEngine.Object.FindObjectOfType<DontDestroy>().gameObject;        
        GameObject _hand = _handHeader.transform.Find("Hand").gameObject;
        _hand.SetActive(true);
        _showPanel.SetActive(true);


        _collection.GetComponent<Canvas>().overrideSorting = true;
        _collection.GetComponent<Canvas>().sortingOrder = 10;
        _collection.GetComponent<Button>().interactable = false;
        _showCollection.SetActive(true);

        _timelineControllerMain.play(_currentScene.name.ToString()); // timeline here.
        yield return new WaitForSeconds(3f);
        _collection.GetComponent<Canvas>().overrideSorting = false;
        //_collection.GetComponent<Canvas>().sortingOrder = 10;
        _collection.GetComponent<Button>().interactable = true;
        _showCollection.SetActive(false);

        _store.GetComponent<Canvas>().overrideSorting = true;
        _store.GetComponent<Canvas>().sortingOrder = 10;
        _store.GetComponent<Button>().interactable = false;
        _showShop.SetActive(true);
        yield return new WaitForSeconds(3f);
        _store.GetComponent<Canvas>().overrideSorting = false;
        _store.GetComponent<Button>().interactable = true;    
        _showShop.SetActive(false);

        _hand.SetActive(false);
        _showPanel.SetActive(false);        

    }
    public void checkSetttings()
    {
        GameObject musicClass = UnityEngine.Object.FindObjectOfType<MusicClass>().gameObject;
        if(musicClass.GetComponent<AudioSource>().enabled == false){
            musicClass.GetComponent<AudioSource>().enabled = true;
        }        

        bool soundBool = (PlayerPrefs.GetInt("SoundOn") == 1) ? true : false;        
        if (soundBool) 
        {
            _sound.isOn = true;
            _audioSource.mute = false; 
            //GameObject.FindGameObjectWithTag("Music").GetComponent<MusicClass>().PlayMusic();
        }
        else 
        {
            _sound.isOn = false;
            _audioSource.mute = true;
            //GameObject.FindGameObjectWithTag("Music").GetComponent<MusicClass>().StopMusic();
        }

        switch (PlayerPrefs.GetInt("VolumeOn"))
        {
            case 1:
                {
                    _volume.isOn = true;
                    AudioListener.volume = 1;                   
                    break;
                }
            case 0:
                {
                    _volume.isOn = false;
                    AudioListener.volume = 0;                    
                    break;
                }
        }
        
        _coinText.text = PlayerPrefs.GetInt("TotalCoin").ToString();
    }
    public void volumeOnOff()
    {
        if (isGameReady)
        {
            _clickforVolumeandSound.Play();
        }        
        if (_volume.isOn)
        {
            AudioListener.volume = 1;
            PlayerPrefs.SetInt("VolumeOn", 1);       
        }
        else
        {
            AudioListener.volume = 0;
            PlayerPrefs.SetInt("VolumeOn", 0);            
        }
    }
    public void soundOnOff()
    {
        if (isGameReady)
        {
            _clickforVolumeandSound.Play();
        }        
        if (_sound.isOn)
        {            
            _audioSource.mute = false;
            PlayerPrefs.SetInt("SoundOn", 1);
            GameObject.FindGameObjectWithTag("Music").GetComponent<MusicClass>().PlayMusic();
        }
        else
        {            
            _audioSource.mute = true;
            PlayerPrefs.SetInt("SoundOn", 0);
            GameObject.FindGameObjectWithTag("Music").GetComponent<MusicClass>().StopMusic();
        }
    }
    public void setLevelMap()
    {
        //int itemNumber = 0;   
        for (int i = 0; i < 5; i++)
        {
            _rabbit = GameObject.Find("Canvas 1/VerticalScroll/Viewport/ScrollContent/Item " + i + "/Rabbit");
            _rabbit.SetActive(false);
        }
        for (int i = 1; i <= 200; i++)
        {
            _itemNumber = checkItemNumber(i-1);            

            _star1 = GameObject.Find("Canvas 1/VerticalScroll/Viewport/ScrollContent/Item " + _itemNumber + "/Level" + " " + "(" + i + ")" + "/Level_Indicator/GameObject" + " " + "(" + 1 + ")");
            _star1.SetActive(false);
            _star2 = GameObject.Find("Canvas 1/VerticalScroll/Viewport/ScrollContent/Item " + _itemNumber + "/Level" + " " + "(" + i + ")" + "/Level_Indicator/GameObject" + " " + "(" + 2 + ")");
            _star2.SetActive(false);
            _star3 = GameObject.Find("Canvas 1/VerticalScroll/Viewport/ScrollContent/Item " + _itemNumber + "/Level" + " " + "(" + i + ")" + "/Level_Indicator/GameObject" + " " + "(" + 3 + ")");
            _star3.SetActive(false);
            _tempLevelObject = GameObject.Find("Canvas 1/VerticalScroll/Viewport/ScrollContent/Item " + _itemNumber + "/Level" + " " + "(" + i + ")");
            _tempLevelObject.SetActive(false);
            _tempLevelObject = GameObject.Find("Canvas 1/VerticalScroll/Viewport/ScrollContent/Item " + _itemNumber + "/Level" + " " + "(" + i + ")" + "/Level_Indicator");
            _tempLevelObject.GetComponent<Button>().enabled = false;
        }
    }       
    public int checkItemNumber(int level)
    {
        int itemNum = 0;
        switch (level  % 200)
        {
            case int i when (i < 40):
                itemNum = 0;
                break;
            case int i when (i >= 40 && i < 80):
                itemNum = 1;
                break;
            case int i when (i >= 80 && i < 120):
                itemNum = 2;
                break;
            case int i when (i >= 120 && i < 160):
                itemNum = 3;
                break;
            case int i when (i >= 160 && i < 200):
                itemNum = 4;
                break;

        }
        return itemNum;
    }
    public void setlevelOrder(int currentOrder)
    {        
        int _k = 40 * currentOrder;
        int _l = 40 * (currentOrder + 1);             
        
        for(int i = _k + 1; i <= _l; i++)
        {            
            int count = 0;            
            _itemNumber = checkItemNumber(i-1);
            _levelNum = (i-1) % 200;
            _levelNum += 1;
            switch (i)
            {
                case int j when (j < PlayerPrefs.GetInt("Level")):
                    var _level = _levelManager[i-1];                   
                    
                    if (_level.LevelStatus == true)
                    {
                        _tempLevelObject = GameObject.Find("Canvas 1/VerticalScroll/Viewport/ScrollContent/Item " + _itemNumber + "/Level" + " " + "(" + _levelNum + ")");
                        _tempLevelObject.SetActive(true);
                        //print(_tempLevelObject.transform.position);
                        _levelText = GameObject.Find("Canvas 1/VerticalScroll/Viewport/ScrollContent/Item " + _itemNumber + "/Level" + " " + "(" + _levelNum + ")" + "/Level_Indicator/Text (TMP)");
                        _levelText.GetComponent<TMP_Text>().text = (i).ToString();

                        _tempLevelObject = GameObject.Find("Canvas 1/VerticalScroll/Viewport/ScrollContent/Item " + _itemNumber + "/Level" + " " + "(" + _levelNum + ")" + "/Level_Indicator");
                        _tempLevelObject.GetComponent<Button>().enabled = true;
                        _tempLevelObject.GetComponent<SpriteRenderer>().sprite = _levelSprite;
                        _tempLevelObject.GetComponent<SpriteRenderer>().size = new Vector2(90f, 90f);
                        if (_level.Star1 == true)
                        {
                            _star1 = GameObject.Find("Canvas 1/VerticalScroll/Viewport/ScrollContent/Item " + _itemNumber + "/Level" + " " + "(" + _levelNum + ")" + "/Level_Indicator/GameObject" + " " + "(" + 1 + ")");
                            _star1.SetActive(true);
                            _star1.GetComponent<SpriteRenderer>().sprite = _star1Sprite;
                        }
                        if (_level.Star2 == true)
                        {
                            _star2 = GameObject.Find("Canvas 1/VerticalScroll/Viewport/ScrollContent/Item " + _itemNumber + "/Level" + " " + "(" + _levelNum + ")" + "/Level_Indicator/GameObject" + " " + "(" + 2 + ")");
                            _star2.SetActive(true);
                            _star2.GetComponent<SpriteRenderer>().sprite = _star2Sprite;
                        }
                        if (_level.Star3 == true)
                        {
                            _star3 = GameObject.Find("Canvas 1/VerticalScroll/Viewport/ScrollContent/Item " + _itemNumber + "/Level" + " " + "(" + _levelNum + ")" + "/Level_Indicator/GameObject" + " " + "(" + 3 + ")");
                            _star3.SetActive(true);
                            _star3.GetComponent<SpriteRenderer>().sprite = _star3Sprite;
                        }

                    }
                    break;
                case int j when (j >= PlayerPrefs.GetInt("Level")):
                    if (i == PlayerPrefs.GetInt("Level"))
                    {
                        /*int currentLevel = (PlayerPrefs.GetInt("Level") - 1) % 200;
                        _itemNumber = checkItemNumber(currentLevel);
                        currentLevel += 1;*/
                        
                        _tempLevelObject = GameObject.Find("Canvas 1/VerticalScroll/Viewport/ScrollContent/Item " + _itemNumber + "/Level" + " " + "(" + _levelNum + ")");
                        _tempLevelObject.SetActive(true);
                        _tempLevelObject.GetComponent<Animator>().enabled = true;
                        _tempLevelObject = GameObject.Find("Canvas 1/VerticalScroll/Viewport/ScrollContent/Item " + _itemNumber + "/Level" + " " + "(" + _levelNum + ")" + "/Level_Indicator");
                        //print(_tempLevelObject.transform.position + "sssssss");
                        _tempLevelObject.GetComponent<Button>().enabled = true;
                        _tempLevelObject.GetComponent<SpriteRenderer>().sprite = _currentlevelSprite;
                        _tempLevelObject.GetComponent<SpriteRenderer>().size = new Vector2(90f, 90f);
                        _rabbit = GameObject.Find("Canvas 1/VerticalScroll/Viewport/ScrollContent/Item " + _itemNumber + "/Rabbit");
                        _rabbit.SetActive(true);
                        _currentLoopNum = currentOrder;
                        //_rabbit.GetComponent<Animator>().enabled = true;
                        _rabbit.transform.position = new Vector2((_tempLevelObject.transform.position.x + 70f), (_tempLevelObject.transform.position.y - 50f));
                        //print(_rabbit.transform.position + "sssssss");
                        _levelText = GameObject.Find("Canvas 1/VerticalScroll/Viewport/ScrollContent/Item " + _itemNumber + "/Level" + " " + "(" + _levelNum + ")" + "/Level_Indicator/Text (TMP)");
                        _levelText.GetComponent<TMP_Text>().text = (i).ToString();

                        _star1 = GameObject.Find("Canvas 1/VerticalScroll/Viewport/ScrollContent/Item " + _itemNumber + "/Level" + " " + "(" + _levelNum + ")" + "/Level_Indicator/GameObject" + " " + "(" + 1 + ")");
                        _star1.SetActive(false);
                        _star2 = GameObject.Find("Canvas 1/VerticalScroll/Viewport/ScrollContent/Item " + _itemNumber + "/Level" + " " + "(" + _levelNum + ")" + "/Level_Indicator/GameObject" + " " + "(" + 2 + ")");
                        _star2.SetActive(false);
                        _star3 = GameObject.Find("Canvas 1/VerticalScroll/Viewport/ScrollContent/Item " + _itemNumber + "/Level" + " " + "(" + _levelNum + ")" + "/Level_Indicator/GameObject" + " " + "(" + 3 + ")");
                        _star3.SetActive(false);
                    }
                    else if (i <= PlayerPrefs.GetInt("Level")+9 && i!= PlayerPrefs.GetInt("Level"))
                    {
                        _tempLevelObject = GameObject.Find("Canvas 1/VerticalScroll/Viewport/ScrollContent/Item " + _itemNumber + "/Level" + " " + "(" + _levelNum + ")");
                        _tempLevelObject.SetActive(true);

                        _levelText = GameObject.Find("Canvas 1/VerticalScroll/Viewport/ScrollContent/Item " + _itemNumber + "/Level" + " " + "(" + _levelNum + ")" + "/Level_Indicator/Text (TMP)");
                        _levelText.GetComponent<TMP_Text>().text = ("").ToString();

                        _tempLevelObject = GameObject.Find("Canvas 1/VerticalScroll/Viewport/ScrollContent/Item " + _itemNumber + "/Level" + " " + "(" + _levelNum + ")" + "/Level_Indicator");
                        _tempLevelObject.GetComponent<SpriteRenderer>().sprite = _levelSprite;
                        _tempLevelObject.GetComponent<SpriteRenderer>().size = new Vector2(50f, 50f);

                        _star1 = GameObject.Find("Canvas 1/VerticalScroll/Viewport/ScrollContent/Item " + _itemNumber + "/Level" + " " + "(" + _levelNum + ")" + "/Level_Indicator/GameObject" + " " + "(" + 1 + ")");
                        _star1.SetActive(false);
                        _star2 = GameObject.Find("Canvas 1/VerticalScroll/Viewport/ScrollContent/Item " + _itemNumber + "/Level" + " " + "(" + _levelNum + ")" + "/Level_Indicator/GameObject" + " " + "(" + 2 + ")");
                        _star2.SetActive(false);
                        _star3 = GameObject.Find("Canvas 1/VerticalScroll/Viewport/ScrollContent/Item " + _itemNumber + "/Level" + " " + "(" + _levelNum + ")" + "/Level_Indicator/GameObject" + " " + "(" + 3 + ")");
                        _star3.SetActive(false);

                        count++;
                    }
                    else
                    {
                        _tempLevelObject = GameObject.Find("Canvas 1/VerticalScroll/Viewport/ScrollContent/Item " + _itemNumber + "/Level" + " " + "(" + _levelNum + ")");
                        _tempLevelObject.SetActive(false);
                                             
                    }
                    break;

            }
            
           
        }

    }   
    public void LoadLevel()
    {
        _jsonController.JsonLoadLevel();
        _levelManager = _jsonController._levelManager;
    }
    public void playGame()
    {        
        if(_SelectedLevel == null)
        {
            _SelectedLevel = EventSystem.current.currentSelectedGameObject.transform.Find("Text (TMP)").gameObject;
        }         
        string _level = PlayerPrefs.GetInt("Level").ToString();

        if (PlayerPrefs.GetInt("Heart") <= 0)
        {
            openFailedHeart();            
        }
        else
        {
            //GameObject.FindGameObjectWithTag("Music").GetComponent<MusicClass>().StopMusic();
            //string name = EventSystem.current.currentSelectedGameObject.name;            

            if (_SelectedLevel.GetComponent<TMP_Text>().text != _level)
            {
                PlayerPrefs.SetInt("CurrentLevel", int.Parse(_SelectedLevel.GetComponent<TMP_Text>().text));
                SceneManager.LoadScene("GameScene");
            }
            else
            {
                PlayerPrefs.SetInt("CurrentLevel", 0);
                SceneManager.LoadScene("GameScene");
            }
            
            //print(name);
            //print(EventSystem.current.currentSelectedGameObject.transform.parent.name);
            
        }
       

    }


    public void openSettingMenu()
    {
        //Time.timeScale = 0; // we use after menudown       
        _panel.SetActive(true);        
        StartCoroutine(MenuDown(_settingScreen));
    }
    IEnumerator MenuDown(GameObject _object)
    {
        _object.SetActive(true);
        float posy = _object.GetComponent<RectTransform>().position.y;
        float posyStatic = _object.GetComponent<RectTransform>().position.y;        
        while (posy >= posyStatic - 1050f * _canvas_1.transform.localScale.x)
        {            
            yield return new WaitForSeconds(0.001f);
            _object.GetComponent<RectTransform>().position = new Vector2(_object.GetComponent<RectTransform>().position.x, posy);
            posy -= 50f;
        }
        Time.timeScale = 0;
    }
    public void closeSettingMenu()
    {
        //Time.timeScale = 1f; // we use after menuup
        _helpScreen.SetActive(false);
        _panel.SetActive(false);
        StartCoroutine(MenuUp(_settingScreen));      
    }
    IEnumerator MenuUp(GameObject _object)
    {
        Time.timeScale = 1;
        float posy = _object.GetComponent<RectTransform>().position.y;
        float posyStatic = _object.GetComponent<RectTransform>().position.y;
        
        while (posy <= posyStatic + 1050f * _canvas_1.transform.localScale.x)
        {
            yield return new WaitForSeconds(0.001f);
            _object.GetComponent<RectTransform>().position = new Vector2(_object.GetComponent<RectTransform>().position.x, posy);
            posy += 50f;
        }
        yield return new WaitForSeconds(0.04f);        
        _object.SetActive(false);
        
    }
    public void openFailedHeart()
    {
        //Time.timeScale = 0f;
        _panel.SetActive(true);
        _failedScreenHeart.SetActive(true);
        StartCoroutine(closeFailed(_failedScreenHeart,"openshopSmall"));
    }
    IEnumerator closeFailed(GameObject _object, string methodName )
    {
        yield return new WaitForSecondsRealtime(2f);
        _object.SetActive(false);
        yield return new WaitForSecondsRealtime(0.1f);
        //Get the method information using the method info class
        MethodInfo mi = this.GetType().GetMethod(methodName);

        //Invoke the method
        // (null- no parameter for the method call
        // or you can pass the array of parameters...)
        mi.Invoke(this, null);
    }
    public void openshopSmall()
    {
        //Time.timeScale = 0; // we use after menudown
        _panel.SetActive(true);        
        StartCoroutine(MenuDown(_shopSmall));
    }
    public void closeshopSmall()
    {
        //Time.timeScale = 1f; // we use after menuup
        _panel.SetActive(false);        
        StartCoroutine(MenuUp(_shopSmall));
    }


    public void buyHeartUseCoin()
    {
        if (PlayerPrefs.GetInt("TotalCoin") > 100)
        {
            int total = PlayerPrefs.GetInt("TotalCoin") - 100;
            PlayerPrefs.SetInt("TotalCoin", total);
            _coinText.text = total.ToString();
            int heart = PlayerPrefs.GetInt("Heart") + 5;
            PlayerPrefs.SetInt("Heart", heart);
            closeshopSmall();
            openYouWin();
        }
        else
        {
            closeshopSmall();
            _failedcoinText.text = PlayerPrefs.GetInt("TotalCoin").ToString();
            openFailedCoin();            
        }        
    }
    public void openFailedCoin()
    {
        //Time.timeScale = 0f;
        _panel.SetActive(true);
        _failedScreenCoin.SetActive(true);
        StartCoroutine(closeFailed(_failedScreenCoin, "openshopSmall"));
    }
    public void buyHeartUseAdMob()
    {
        isPaused = true;
        bool isAdReady = AdMobManager.RewardedReklamHazirMi();
        if (isAdReady)
        {            
            AdMobManager.RewardedReklamGoster(RewardedAdHeart);
        }
        else
        {            
            AdMobManager.RewardedReklamAl();
        }

    }
    public void RewardedAdHeart(GoogleMobileAds.Api.Reward odul)
    {
        //print("sadsa");
        int heart = PlayerPrefs.GetInt("Heart") + 5;
        PlayerPrefs.SetInt("Heart", heart);        
    }
    public void openYouWin()
    {
        isPaused = false;
        closeshopSmall();
        _panel.SetActive(true);
        _youWin.SetActive(true);
        float scale = CameraControllerMain._orthoSize / 5f; // First camera size 5f.
        _instantiatedObj = Instantiate(_youWinParticles, _youWinParticles.transform.position, Quaternion.identity);        
        _instantiatedObj.transform.localScale = new Vector2(scale, scale);
    }
    public void Collect()
    {
        _youWin.SetActive(false);
        _panel.SetActive(false);
        Destroy(_instantiatedObj);
        playGame();
    }



    
    public void openHelp()
    {
        if (_settingScreen.activeSelf == true)
        {
            _settingScreen.SetActive(false);
        }
        _helpScreen.transform.position = _settingScreen.transform.position;
        _helpScreen.SetActive(true);
    }
    public void closeHelp()
    {
        if (_settingScreen.activeSelf == false)
        {
            _settingScreen.SetActive(true);
        }
        _helpScreen.SetActive(false);
    }
    public void openQuit()
    {
        //_quitScreen.transform.position = _settingScreen.transform.position;
        if (_settingScreen.activeSelf == true)
        {
            _settingScreen.SetActive(false);
        }
        _quitScreen.SetActive(true);
    }
    public void closeQuit()
    {
        if (_settingScreen.activeSelf == false)
        {
            _settingScreen.SetActive(true);
        }
        _quitScreen.SetActive(false);
    }
    public void quit()
    {
        Application.Quit();
    }
    

    public void storeScene()
    {        
        SceneManager.LoadScene("StoreScene");
        
    }
    public void collectionScene()
    {
        SceneManager.LoadScene("CollectionScene");
    }
    
    
}
