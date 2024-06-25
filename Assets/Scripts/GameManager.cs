using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using Newtonsoft.Json;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Egg")]
    [HideInInspector] public int _height;
    [HideInInspector] public int _width;
    [HideInInspector] public Vector2 _boardCenter;
    [SerializeField] private Board _boardPrefab;
    // [SerializeField] private Egg _eggPrefab;
    [HideInInspector] public List<EggsManager> _eggCollection = new List<EggsManager>();
    [HideInInspector] public List<EggsManager> _tempEggCollection = new List<EggsManager>();
    public Egg[] _eggsList;    
    [HideInInspector] public float _respawnTime = 1.6f;
    private bool _multipyReSpawn;
    private bool _changeableRespawn;
    //List<GameObject> _spawnedEggs = new List<GameObject>();
    public Egg[] _spawnedEggs = new Egg[2];
    [HideInInspector] int _counterEgg;

    /*
    public int BasketTouchId;
    public int CatchTouchId;    
    */

    JsonController _jsonController;
    StopMenuManager _stopMenuManager;
    LevelScreenManager _levelController;
    TimelineController _timelineController;
    TimelineControllerMain _timelineControllerMain;
    CameraController _cameraController;
    BossManager _bossManager;

    [Header("Baskets")]
    [SerializeField] GameObject Basket;
    [SerializeField] GameObject Big_Basket;
    [SerializeField] GameObject Backpack;
    [SerializeField] GameObject Bag;

    [Header("Boss/Rabbit/Gift")]
    [SerializeField] public Gift _giftObject;
    [SerializeField] public GameObject _rabbit;   
    [HideInInspector] public List<ItemManager> _itemCollection = new List<ItemManager>();

    [Header("Settings")]
    public Image _background;
    [SerializeField] public TMPro.TMP_Text _scoreText;
    [SerializeField] public TMPro.TMP_Text _coinText;
    [SerializeField] public TMPro.TMP_Text _timeText;
    [SerializeField] public TMPro.TMP_Text _heartText;
    public int _heart = 0;
    public int _score = 0;
    public int _coin = 0;
    public int _numberofBasketObject = 0;
    public float _timer = 60f;
    public int _level;
    private int randomBalanceforTotal =90;

    [HideInInspector] public bool _isRainEgg;
    [HideInInspector] public bool _gameOver ;
    [HideInInspector] public bool _isAnimationPlaying;
    [HideInInspector] public bool _isTimerActive = true;
    [HideInInspector] public bool isGameReady;
    

    [SerializeField] AudioClip[] _backgroundSongClip;
    [SerializeField] AudioSource _backgrodunSongSource;

    [SerializeField] private GameObject _showPanel;
    
    Scene _currentScene;
    // Start is called before the first frame update
    void Start()
    {
        _currentScene = SceneManager.GetActiveScene();
        Screen.orientation = ScreenOrientation.Portrait;
        //AdMobManager.BannerGizle();
        if (PlayerPrefs.GetString("SaveEggFirstTime") == "")
        {
            saveEggCollection();
            PlayerPrefs.SetString("SaveEggFirstTime", "true");
        }

        if (PlayerPrefs.GetString("ShowTimelineGame") == "false") { Game(); }

        if (PlayerPrefs.GetString("ShowTimelineGame") == "true" || PlayerPrefs.GetString("ShowTimelineGame") == "")
        {
            try
            {
                StartCoroutine(_showAnim());
                PlayerPrefs.SetString("ShowTimelineGame", "false");
            }
            catch { };

        }
        loadEggCollection();
        loadTempEggCollection();
        loadItemCollection();
            
        isGameReady = true;
        
       
    }    
    // Update is called once per frame
    void Update()
    {
        if (_isTimerActive == true)
        {
            timerGame();
        }
        //touchControl();        
    }
    private void Awake()
    {
        _jsonController = UnityEngine.Object.FindObjectOfType<JsonController>();
        _stopMenuManager = UnityEngine.Object.FindObjectOfType<StopMenuManager>();
        _levelController = UnityEngine.Object.FindObjectOfType<LevelScreenManager>();
        _timelineController = UnityEngine.Object.FindObjectOfType<TimelineController>();
        _cameraController = UnityEngine.Object.FindObjectOfType<CameraController>();
        _bossManager = UnityEngine.Object.FindObjectOfType<BossManager>();
        _timelineControllerMain = UnityEngine.Object.FindObjectOfType<TimelineControllerMain>();    
        
        

        
        if (PlayerPrefs.GetInt("CurrentLevel") > 0)
        {
            _level = PlayerPrefs.GetInt("CurrentLevel");
        }
        else
        {
            _level = PlayerPrefs.GetInt("Level");
        }
        
        _score = PlayerPrefs.GetInt("TotalScore");        
        _coin = PlayerPrefs.GetInt("TotalCoin");
        _timer = PlayerPrefs.GetFloat("Time");
        if(_level % 10 == 0)
        {
            PlayerPrefs.SetFloat("Time" , _timer + 40f);
            _timer = PlayerPrefs.GetFloat("Time");
        }
        _heart = PlayerPrefs.GetInt("Heart");

        
        setBasketType();
        //GenerateBoard();
        setRespawnTime();
        checkSetting();
    }
    // we didn't use touchControlFunction
    /*
    void touchControl()
    {
        foreach (UnityEngine.Touch touch in Input.touches)
        {
            if (touch.position.x < 720 && touch.position.y < 150)
            {
                BasketTouchId = touch.fingerId;
                //print("Pan"+PanTouchId);
            }   
            print("Id" + touch.fingerId);
            //print("Loc X" + touch.position.x);
            //print("Loc Y" + touch.position.y);
        }
    }
    */

    IEnumerator _showAnim()
    {
        //print("sss");        
        //Time.timeScale = 0f;         
        GameObject _handHeader = UnityEngine.Object.FindObjectOfType<DontDestroy>().gameObject;
        GameObject _hand = _handHeader.transform.Find("Hand").gameObject;
        _hand.SetActive(true);
        _hand.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("GameAnimation/Canvas") as RuntimeAnimatorController;
        _showPanel.SetActive(true);        

        _timelineControllerMain.play(_currentScene.name.ToString()); // timeline here.
        GameObject _showEggScreen = _showPanel.transform.Find("ShowEggCollect").gameObject;
        _showEggScreen.SetActive(true);
        yield return new WaitForSeconds(4f);        
        //yield return new WaitForSecondsRealtime(4f);

        _showEggScreen.SetActive(false);
        GameObject _showHeartScreen = _showPanel.transform.Find("ShowHeartCollect").gameObject;
        _showHeartScreen.SetActive(true);
        //yield return new WaitForSecondsRealtime(4f);
        yield return new WaitForSeconds(4f);        
        _showHeartScreen.SetActive(false);
        GameObject _showGiftScreen = _showPanel.transform.Find("ShowGiftCollect").gameObject;
        _showGiftScreen.SetActive(true);
        //yield return new WaitForSecondsRealtime(4f);
        yield return new WaitForSeconds(4f);
        _showGiftScreen.SetActive(false);
        _hand.GetComponent<Image>().enabled = false;
        //_hand.GetComponent<Image>().color = new Color(_hand.GetComponent<Image>().color.r, _hand.GetComponent<Image>().color.g, _hand.GetComponent<Image>().color.b, 0f);
        GameObject _showBasketScreen = _showPanel.transform.Find("ShowBasket").gameObject;
        _showBasketScreen.SetActive(true);        
        //yield return new WaitForSecondsRealtime(3f);
        yield return new WaitForSeconds(3f);
        _showBasketScreen.SetActive(false); 
        _hand.SetActive(false);
        _hand.GetComponent<Image>().enabled = true;        
        _showPanel.SetActive(false);
        _hand.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("GameAnimation/Hand") as RuntimeAnimatorController;
        Game();
    }
    public void GenerateBoard()
    {
        _boardCenter = new Vector2((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f);
        var _board = Instantiate(_boardPrefab, new Vector2(0, 0), Quaternion.identity);
        _board._spriteRenderer.size = new Vector2(_width, _height);
        

    }
    void setBackground()
    {
        switch (PlayerPrefs.GetInt("BossOrder"))
        {
            case (0):
                _background.sprite = Resources.Load<Sprite>("Backgrounds/Background");
                break;
            case (1):
                _background.sprite = Resources.Load<Sprite>("Backgrounds/FrogLand");
                break;
            case (2):
                _background.sprite = Resources.Load<Sprite>("Backgrounds/TurtleLand");
                break;
            case (3):
                _background.sprite = Resources.Load<Sprite>("Backgrounds/BatLand");
                break;
            case (4):
                _background.sprite = Resources.Load<Sprite>("Backgrounds/PandaLand");
                break;
            case (5):
                _background.sprite = Resources.Load<Sprite>("Backgrounds/DuckLand");
                break;
            case (6):
                _background.sprite = Resources.Load<Sprite>("Backgrounds/KoalaLand");
                break;
            case (7):
                _background.sprite = Resources.Load<Sprite>("Backgrounds/RabbitLand");
                break;
            case (8):
                _background.sprite = Resources.Load<Sprite>("Backgrounds/SquirrelLand");
                break;
            case (9):
            case (10):
            case (11):
            case (12):
                _background.sprite = Resources.Load<Sprite>("Backgrounds/DragonLand");
                break;
           
        }




        /*
        switch (PlayerPrefs.GetString("bossAnimationType"))
        {
            case (""):
            case (null):
                _background.sprite = Resources.Load<Sprite>("Backgrounds/Background");
                break;
            case ("Frog"):
                _background.sprite = Resources.Load<Sprite>("Backgrounds/FrogLand");
                break;
            case "Turtle":
                _background.sprite = Resources.Load<Sprite>("Backgrounds/TurtleLand");
                break;
            case "Bat":
                _background.sprite = Resources.Load<Sprite>("Backgrounds/BatLand");
                break;
            case "Panda":
                _background.sprite = Resources.Load<Sprite>("Backgrounds/PandaLand");
                break;
            case "FlyRed":
            case "FlyGold":
            case "FlyRedTwoHead":
            case "FlyBlueTwoHead":
                _background.sprite = Resources.Load<Sprite>("Backgrounds/DragonLand");
                break;
        }
        */

    }
    public void setBasketType()
    {
        if (PlayerPrefs.GetInt("BasketCounter") <= 0 && PlayerPrefs.GetString("BasketType") != "Basket")
        {           
            PlayerPrefs.SetString("BasketType", "Basket");
        }
        string basketType = PlayerPrefs.GetString("BasketType");
        switch (basketType)
        {
            case ("Basket"):
                Basket.SetActive(true);
                break;
            case ("Big_Basket"):
                Big_Basket.SetActive(true);
                break;
            case ("Backpack"):
                Backpack.SetActive(true);
                break;
            case ("Bag"):
                Bag.SetActive(true);
                break;            
                
        }
        //transform.Find("Visual").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Product/" + basketType);
    }
    void checkSetting()
    {
        _coinText.text = _coin.ToString();
        _scoreText.text = _score.ToString();
        _heartText.text = _heart.ToString();
        _rabbit.GetComponent<Animator>().SetTrigger("Hi");
    }  
    void setRespawnTime()
    {
        int _levelTemp = (_level - 1) % 200; // for level loop
        //print(_levelTemp / 10);
        switch (_levelTemp / 10)
        {
            case int i when (i >= 0 && i < 1):
                PlayerPrefs.SetFloat("RespwanTime", 1.6f);                
                PlayerPrefs.SetFloat("GiftSpeed", 2f);
                PlayerPrefs.SetFloat("GiftAngle", 0.1f);
                randomBalanceforTotal = 50;
                PlayerPrefs.SetFloat("Time", 50);
                break;
            case int i when (i >= 1 && i < 2):
                PlayerPrefs.SetFloat("RespwanTime", 1.5f);                
                PlayerPrefs.SetFloat("GiftSpeed", 3f);
                PlayerPrefs.SetFloat("GiftAngle", 0.2f);
                randomBalanceforTotal = 55;
                PlayerPrefs.SetFloat("Time", 60);
                break;
            case int i when (i >= 2 && i < 4):
                PlayerPrefs.SetFloat("RespwanTime", 1.4f);
                PlayerPrefs.SetFloat("GiftSpeed", 4f);
                PlayerPrefs.SetFloat("GiftAngle", 0.3f);
                PlayerPrefs.SetFloat("Time", 70);
                randomBalanceforTotal = 60;                
                break;
            case int i when (i >= 4 && i < 9):
                PlayerPrefs.SetFloat("RespwanTime", 1.35f);
                PlayerPrefs.SetFloat("GiftSpeed", 5f);
                PlayerPrefs.SetFloat("GiftAngle", 0.4f);
                PlayerPrefs.SetFloat("Time", 80);
                randomBalanceforTotal = 65;
                _changeableRespawn = true;
                break;
            case int i when (i >= 9 && i < 11):
                PlayerPrefs.SetFloat("RespwanTime", 1.3f);
                PlayerPrefs.SetFloat("GiftSpeed", 6f);
                PlayerPrefs.SetFloat("GiftAngle", 0.5f);
                PlayerPrefs.SetFloat("Time", 85);
                randomBalanceforTotal = 70;
                _changeableRespawn = true;                
                break;
            case int i when (i >= 11 && i < 14):
                PlayerPrefs.SetFloat("RespwanTime", 1.2f);                               
                PlayerPrefs.SetFloat("GiftSpeed", 7f);
                PlayerPrefs.SetFloat("GiftAngle", 0.6f);
                PlayerPrefs.SetFloat("Time", 90);
                randomBalanceforTotal = 75;
                _changeableRespawn = true;
                _multipyReSpawn = true;
                break;
            case int i when (i >= 14 && i < 19):
                PlayerPrefs.SetFloat("RespwanTime", 1.1f);
                PlayerPrefs.SetFloat("GiftSpeed", 8f);
                PlayerPrefs.SetFloat("GiftAngle", 0.7f);
                PlayerPrefs.SetFloat("Time", 95);
                randomBalanceforTotal = 80;                
                _changeableRespawn = true;
                _multipyReSpawn = true;
                break;
            case int i when (i >= 19 && i < 20):
                PlayerPrefs.SetFloat("RespwanTime", 1.0f);                
                PlayerPrefs.SetFloat("GiftSpeed", 9f);
                PlayerPrefs.SetFloat("GiftAngle", 0.8f);
                PlayerPrefs.SetFloat("Time", 100);
                randomBalanceforTotal = 85;
                _changeableRespawn = true;
                _multipyReSpawn = true;                
                break;

        }
        _respawnTime = PlayerPrefs.GetFloat("RespwanTime");

    }


    public void Game()
    {       
        StartCoroutine(eggsWawe());

        /*
        _bossManager._isBossActive = true;
        _bossManager._bossOrder = 12;
        _bossManager.checkBossOrder();
        */

        // for boss active
        _backgrodunSongSource.clip = _backgroundSongClip[0];

        
        int _levelTemp = (_level) % 40; // for level loop
        //print(_levelTemp / 10);
        switch (_levelTemp / 4)
        {
            case int i when (i == 0):                
                _bossManager._bossOrder = 0;
                if (_levelTemp == 0) { _bossManager._bossOrder = 12; } // for the last boss.
                break;
            case int i when (i == 1 || i == 11 ):
                _bossManager._bossOrder = 1;
                break;
            case int i when (i == 2 || i == 12 ):
                _bossManager._bossOrder = 2;
                break;
            case int i when (i == 3 || i == 13 ):
                _bossManager._bossOrder = 3;
                break;
            case int i when (i == 4 || i == 14 ):
                _bossManager._bossOrder = 4;
                break;
            case int i when (i == 5 || i == 15 ):
                _bossManager._bossOrder = 5;
                break;
            case int i when (i == 6 || i == 16 ):
                _bossManager._bossOrder = 6;
                break;
            case int i when (i == 7 || i == 17 ):
                _bossManager._bossOrder = 7;
                break;
            case int i when (i == 8 || i == 18 ):
                _bossManager._bossOrder = 8;
                break;
            case int i when (i == 9 ):
                _bossManager._bossOrder = 9;
                break;
            case int i when (i == 10 ):
                _bossManager._bossOrder = 10;
                break;
            case int i when (i == 19):
                _bossManager._bossOrder = 11;
                break;            
            
        }
        

        PlayerPrefs.SetInt("BossOrder", _bossManager._bossOrder);

        if (_levelTemp % 2 == 0 && _levelTemp !=2)
        {
            _backgrodunSongSource.clip = _backgroundSongClip[1];
            _bossManager._isBossActive = true;
            _bossManager.checkBossOrder();
        }        
        setBackground();
        //GameObject.FindGameObjectWithTag("Music").GetComponent<MusicClass>().PlayMusic();
        _backgrodunSongSource.Play();

        //_rabbit.GetComponent<Animator>().SetBool("Hi", true);        
        /*if (_gameOver)
        {
            _stopMenuManager.openlevelComplete();            
        }*/
    }
    IEnumerator eggsWawe()
    {
        while (!_gameOver && _stopMenuManager._isGamePaused == false)
        {
            if(_changeableRespawn && _isRainEgg==false)
            {
                _respawnTime = UnityEngine.Random.Range(PlayerPrefs.GetFloat("RespwanTime") - 0.3f, PlayerPrefs.GetFloat("RespwanTime") + 0.3f);                
            }

            yield return new WaitForSeconds(_respawnTime);            
            int randomNum = UnityEngine.Random.Range(0, 100);
            //SpawnGift();
            //SpawnPoison();
            
            
            switch (randomNum)
            {
                case int i when (i >= 90):
                    SpawnGift();
                    break;
                case int i when (i >= 80 && i < 90):
                    SpawnPoison();
                    break;
                case int i when (i < 80):
                    SpawnEgg();
                    if (_multipyReSpawn && _isRainEgg == false)
                    {
                        int randomN= UnityEngine.Random.Range(0, 11);
                        if (randomN > 8)
                        {
                            yield return new WaitForSeconds(_respawnTime / 2);
                            SpawnEgg();
                        }
                    }
                    break;

            }    
            
            
        }
            
    }
    IEnumerator eggsRain()
    {       
        _isRainEgg = true;
        _respawnTime = (_respawnTime - 0.7f)/2;        
        yield return new WaitForSeconds(3f);
        _respawnTime = PlayerPrefs.GetFloat("RespwanTime");
        yield return new WaitForSeconds(1.7f);
        _isRainEgg = false;
    }
    

    public void SpawnEgg()
    {        
        _counterEgg++;
        int _spawnedEggorder = _counterEgg % 2;
        //int spawntime = Random.Range(1, 10);
        int _x = UnityEngine.Random.Range(-2,3);
        int randomSpecial = UnityEngine.Random.Range(1, 101);
        int randomEgg;
        int level = _level % 200;
        switch (level)
        {
            case int i when (i > 0 && i < 10):
                //print("a");
                randomEgg = UnityEngine.Random.Range(0, 2);
                var _eggNormal = Instantiate(_eggsList[randomEgg], new Vector2(_x, _height - 1), Quaternion.identity);
                // AnimeEgg(_eggNormal);
                _spawnedEggs[_spawnedEggorder] = _eggNormal;
                break;
            case int i when (i >= 10 || i==0):
                if (randomSpecial > randomBalanceforTotal)
                {
                    randomEgg = 2;
                    int randomEggNum = UnityEngine.Random.Range(0, 10);
                    //print("b");
                    var _egg  = Instantiate(_eggsList[randomEgg], new Vector2(_x, _height - 1), Quaternion.identity);
                    int value = level / 10;
                    string type = Enum.GetName(typeof(EggTypes), value);                    
                    _egg._spriteRenderer.sprite = Resources.Load<Sprite>("Eggs/" + type + "/" + type + " " + randomEggNum);
                    while (_egg._spriteRenderer.sprite == null)
                    {                        
                        randomEggNum = UnityEngine.Random.Range(0, 10);
                        _egg._spriteRenderer.sprite = Resources.Load<Sprite>("Eggs/" + type + "/" + type + " " + randomEggNum);
                    }
                    string num = Enum.GetName(typeof(EggsNumbers), randomEggNum);
                    _egg._spriteRenderer.size = new Vector2(1, 1);
                    _egg.EggType = (EggTypes)Enum.Parse(typeof(EggTypes), type);
                    _egg.EggNumbers = (EggsNumbers)Enum.Parse(typeof(EggsNumbers), num);
                    AnimeEgg(_egg);
                    _spawnedEggs[_spawnedEggorder] = _egg;
                    AnimeScreen(_egg,type,randomEggNum);
                    
                    /*if (randomSpecial > 90 && _stopMenuManager._isGamePaused==false)
                    {
                        Destroy(_egg.gameObject);                       
                        Time.timeScale = 0f;
                        _isAnimationPlaying = true;
                        _stopMenuManager._isGamePaused = true;
                        _timelineController.play();                        
                        _timelineController._EggAnime.sprite = Resources.Load<Sprite>("Eggs/" + type + "/" + type + " " + randomEggNum);                        
                    }*/
                    /*if (randomSpecial > 90)
                    {
                        StartCoroutine(eggsRain());
                    }*/
                    
                    /*foreach (int s in Enum.GetValues(typeof(EggsNumbers)))
                        print(s + "...");*/
                    //break;                              
                }
                else
                {
                    //print(randomSpecial+ ".");  
                    randomEgg = UnityEngine.Random.Range(0, 2);
                    var _egg = Instantiate(_eggsList[randomEgg], new Vector2(_x, _height - 1), Quaternion.identity);
                    AnimeEgg(_egg);
                    _spawnedEggs[_spawnedEggorder] = _egg;
                    
                }                
                break;
                
        }
        /*int rand = UnityEngine.Random.Range(0, 10);
        if (rand == 1)
        {
            var _egg = Instantiate(_eggsList[randomEgg], new Vector2(_x, _height - 1), Quaternion.identity);
        }*/



    }
    public void SpawnGift()
    {
        int _x = UnityEngine.Random.Range(-2, 3);
        var _gift = Instantiate(_giftObject, new Vector2(_x, _height - 1), Quaternion.identity);

        int giftCount = Enum.GetNames(typeof(GiftTypes)).Length;
        int rndValue = UnityEngine.Random.Range(0, giftCount);
        string type = Enum.GetName(typeof(GiftTypes), rndValue);        
        _gift._spriteRenderer.sprite = Resources.Load<Sprite>("Gift/" + type);
        // _gift.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Gift/" + type); // before we add visual gameobject this is ok.
        _gift.GiftTypes = (GiftTypes)Enum.Parse(typeof(GiftTypes), type);
    }
    public void SpawnPoison()
    {        
        int _x = UnityEngine.Random.Range(-2, 3);
        int randomEggNum = UnityEngine.Random.Range(0, 6);
        //print("b");
        var _poisonEgg = Instantiate(_eggsList[3], new Vector2(_x, _height - 1), Quaternion.identity);        
        //string type = Enum.GetName(typeof(EggTypes), value);
        _poisonEgg._spriteRenderer.sprite = Resources.Load<Sprite>("PoisonEgg/Poison_Egg" + " " + randomEggNum);
        while (_poisonEgg._spriteRenderer.sprite == null)
        {
            randomEggNum = UnityEngine.Random.Range(0, 10);
            _poisonEgg._spriteRenderer.sprite = Resources.Load<Sprite>("PoisonEgg/Poison_Egg" + " " + randomEggNum);
        }
        string num = Enum.GetName(typeof(EggsNumbers), randomEggNum);
        _poisonEgg._spriteRenderer.size = new Vector2(1, 1);        
        _poisonEgg.EggNumbers = (EggsNumbers)Enum.Parse(typeof(EggsNumbers), num);        
        AnimeEgg(_poisonEgg);
    }
    

    public void AnimeEgg(Egg _egg)
    {
        //_egg.moveFNC(); //moveFNC();        
        int random = UnityEngine.Random.Range(0, 140);     

        switch (random)
        {
            case int i when (i >= 100 && i < 110 && _level >= 35):
                _egg._noelhat.SetActive(true);
                _egg._noelhat.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Animation/Noelhat " + UnityEngine.Random.Range(1, 8));
                Instantiate(_stopMenuManager._snowParticles, _stopMenuManager._snowParticles.transform.position, Quaternion.identity);
                //StartCoroutine(move_partyhat(_egg, _egg._noelhat));
                break;
            case int i when (i >= 90 && i < 100 && _level >= 35):
                _egg._animator.SetBool("Spiral", true);
                move_Spriral(_egg);
                break;
            case int i when (i >= 80 && i < 90 && _level >= 30):                
                _egg._animator.SetBool("Turn", true);
                StartCoroutine(move_Turn(_egg));
                break;
            case int i when (i >= 70 && i < 80 && _level >= 30):
                _egg._partyhat.SetActive(true);
                _egg._partyhat.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Animation/Partyhat " + UnityEngine.Random.Range(1, 10));
                StartCoroutine(move_partyhat(_egg, _egg._partyhat));
                break;
            case int i when (i >= 60 && i < 70 && _level >= 25):
                _egg._animeObject.SetActive(true);
                _egg._animator.SetBool("Effect", true);
                StartCoroutine(move_Effect(_egg));                
                break;
            case int i when (i >= 50 && i < 60 && _level >= 25):
                _egg._airballon.SetActive(true);
                _egg._airballon.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Animation/Airballoon " + UnityEngine.Random.Range(1, 6));
                StartCoroutine(move_Airballoon(_egg));
                break;
            // turn with parachute object
            case int i when (i >= 40 && i < 50 && _level >= 20):
                _egg._parachute.SetActive(true);
                _egg._animator.SetBool("Turn", true);
                _egg.gameObject.GetComponent<Rigidbody2D>().gravityScale = UnityEngine.Random.Range(0.3f, 0.7f);
                break;
            case int i when (i >= 30 && i < 40 && _level >= 20):
                _egg._animeObject.SetActive(true);
                _egg._rocket.SetActive(true);
                _egg._rocket.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Animation/Rocket " + UnityEngine.Random.Range(1, 5)); 
                _egg._animator.SetBool("Rocket", true);                
                _egg.gameObject.GetComponent<Rigidbody2D>().gravityScale = UnityEngine.Random.Range(2, 4);
                break;          
            case int i when (i >= 20 && i < 30 && _level >= 15):
                _egg._animeObject.SetActive(true);
                _egg._animator.SetBool("Helicopter", true);
                move_Helicopter(_egg);
                break;
            case int i when (i >= 10 && i < 20 && _level >= 15):
                _egg._ballons.SetActive(true);
                _egg._ballons.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Animation/Balloon " + UnityEngine.Random.Range(1, 5));
                StartCoroutine(move_balloon(_egg));
                break;
            case int i when (i < 10  && _level >= 10):
                _egg._parachute.SetActive(true);
                _egg._animator.SetBool("Parachute", true);
                _egg._parachute.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Animation/Parachute " + UnityEngine.Random.Range(1, 4));
                _egg.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0.3f;
                break;
        }

    }
    IEnumerator move_Effect(Egg _egg)
    {
        /*
        Random random = new Random();
        bool randomBool = random.Next(2) == 1;
        */
        int randomXpos = UnityEngine.Random.Range(-2, 3);
        yield return new WaitForSeconds(0.5f);
        try
        {
            if (_egg != null)
            {
                _egg.gameObject.transform.position = new Vector2(randomXpos, _egg.transform.position.y);
            }
        }
        catch { };
    }
    IEnumerator move_Turn(Egg _egg)
    {        
        int count = 2;
        while (count>0 )
        {            
            yield return new WaitForSeconds(0.35f);
            count--;
            int randomXpos = UnityEngine.Random.Range(-2, 3);
            try
            {
                if (_egg != null)
                {
                    _egg.gameObject.transform.position = new Vector2(randomXpos, _egg.transform.position.y);
                }
            }
            catch { };        
        }

        
    }
    void move_Helicopter(Egg _egg)
    {
        int randomForce = UnityEngine.Random.Range(-15, 15);
        try
        {
            if (_egg != null)
            {
                _egg.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(randomForce, 0), ForceMode2D.Impulse);
            }
        }
        catch { };
        
         
        
        /*        
        _direction = Vector2.down;
        _direction.x = Random.Range(-_eggMoveAngle, _eggMoveAngle);
        _rigidbody.velocity = _direction * _moveSpeed;
        */
    }
    void move_Spriral(Egg _egg)
    {
        int randomForceX = UnityEngine.Random.Range(-15, 15);
        try
        {
            if (_egg != null)
            {
                _egg.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(randomForceX, 30), ForceMode2D.Impulse);
            }
        }
        catch { };
        
        
    }
    IEnumerator move_Airballoon(Egg _egg)   
    {              
        float time =2f; 
        while (time>0)
        {
            yield return new WaitForSeconds(0.3f);
            //if(_egg.gameObject.activeInHierarchy == false && _egg._collisionTime !=0) { break; }
            int randomForceX = UnityEngine.Random.Range(-15, 15);
            try
            {
                if (_egg != null)
                {
                    _egg.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(randomForceX, 5), ForceMode2D.Impulse);
                    time -= 0.3f;
                }
            }
            catch { };                      
        }        

        /*
        Vector3 pointA = new Vector3(-2, 0, 0);
        Vector3 pointB = new Vector3(2, 0, 0);
        float speed = 1;

        _egg.transform.position = pointA;
        Tweener tweener = transform.DOMove(pointB, speed);
        tweener.SetEase(Ease.InOutQuad);
        tweener.SetLoops(int.MaxValue, LoopType.Yoyo);
        */

    }
    IEnumerator move_balloon(Egg _egg)
    {
        float time = UnityEngine.Random.Range(0.5f, 1f);        
        while (time > 0)
        {
            float rnd = UnityEngine.Random.Range(0.1f, 0.2f);
            yield return new WaitForSeconds(rnd);
            try
            {
                if (_egg != null)
                {
                    int randomForceX = UnityEngine.Random.Range(-15, 15);
                    _egg.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(randomForceX, Math.Abs(randomForceX)), ForceMode2D.Impulse);
                    time -= rnd;
                }
            }
            catch { };
                 
        }
    }
    IEnumerator move_partyhat(Egg _egg, GameObject _hat)
    {        
        float rnd = UnityEngine.Random.Range(0.1f, 0.3f);
        yield return new WaitForSeconds(rnd);
        try
        {
            if (_egg.gameObject != null && _egg._collisionTime == 0)
            {
                int randomForceX = UnityEngine.Random.Range(-15, 15);
                _egg.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(randomForceX, 5), ForceMode2D.Impulse);
                _hat.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-randomForceX, -5), ForceMode2D.Impulse);
                _hat.gameObject.GetComponent<SpriteRenderer>().sortingOrder = -2;
            }
        }
        catch { };
              
    }


    public void AnimeScreen(Egg _eggAnime,string type, int randomEggNum)
    {        
        int randomNum = UnityEngine.Random.Range(1, 101);
        switch (randomNum)
        {
            case int i when (i <= 20):
                if (_isRainEgg == true)
                {
                    return;
                }
                StartCoroutine(eggsRain());                
                return;
            case int i when (i > 20 && i < 40):
                // I canceled two camera and stop game 
                /*
                Time.timeScale = 0f;
                _isAnimationPlaying = true;
                _stopMenuManager._isGamePaused = true;
                */                
                
                try
                {
                    _timelineController.play();
                    _timelineController._EggAnime.sprite = Resources.Load<Sprite>("Eggs/" + type + "/" + type + " " + randomEggNum);
                    if (_eggAnime != null)
                    {
                        Destroy(_eggAnime.gameObject);
                    }
                }
                catch { };
                return;               
        }
    }
    IEnumerator startGameAgain()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 1f;
    }


    public void updateScore(int types, int num)
    {
        _numberofBasketObject++;
        if (types > 0)
        {
            updateCoin(20);
            _score += 5; //+ num;            
        }
        else
        {
            updateCoin(10);
            _score += 2;
        }        
        _scoreText.text = _score.ToString();       
        PlayerPrefs.SetInt("TotalScore", _score);       
    }
    public void updateCoin(int plusScore)
    {        
        _coin += plusScore / 10;
        _coinText.text = _coin.ToString();
        PlayerPrefs.SetInt("TotalCoin", _coin);        
    }
    public void updateHeart()   
    {
        if (_heart >= 1)
        {            
            _heart--;
            PlayerPrefs.SetInt("Heart", _heart);
            _heartText.text = PlayerPrefs.GetInt("Heart").ToString();            
            StartCoroutine(_cameraController.Shake(0.2f, 0.1f));
            //StartCoroutine(_cameraController.setCamPos(0.3f));
        }

        if (_heart == 0 && _gameOver == false)
        {
            _gameOver = true;
            Time.timeScale = 0f;
            StartCoroutine(gameover());
            return;
        }

        /*if (_heart <= 0 && _gameOver == false)
        {
            //_heart--;
            PlayerPrefs.SetInt("Heart", 0);
            _heartText.text = PlayerPrefs.GetInt("Heart").ToString();

            //_gameOver = true;

            
            
        }*/

    }
    public void timerGame()
    {
        if (_stopMenuManager._isGamePaused)
        {
            return;
        }
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
        }
        else
        {
            _timer = 0;
            _gameOver = true;
            _stopMenuManager._isGamePaused = true;
            StartCoroutine(gameover());
        }
        float minutes = Mathf.FloorToInt(_timer / 60);
        float seconds = Mathf.FloorToInt(_timer % 60);
        _timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

    }
    IEnumerator gameover()
    {        
        yield return new WaitForSecondsRealtime(0.5f);
        if (_heart > 0 && _timer <= 0)
        {
            _stopMenuManager.openlevelComplete();
            _levelController.updateLevelScreen();
        }
        else
        {
            _stopMenuManager.openFailed();
        }
    }


    public void saveEggCollection()
    {       
        bool hasEggFirst = false;
        int totalEgg = 0;
        string[] names = (System.Enum.GetNames(typeof(EggTypes)));
        int[] numbers = ((int[])System.Enum.GetValues(typeof(EggsNumbers)));
        
        for(int i = 0; i < names.Length; i++)
        {
            for(int j = 0; j < numbers.Length; j++)
            {
                _eggCollection.Add(new EggsManager(names[i], numbers[j], hasEggFirst, totalEgg));
            }
        }
        _jsonController.JsonSave(_eggCollection);
        _jsonController.JsonSaveOrginal(_eggCollection);
        /*_eggsManager = new EggsManager(name, number,hasEgg);
        eggtypeList.Add(new EggsManager(name, number, hasEgg));        
        _eggsManager = new EggsManager(name1, number1, hasEgg1);
        eggtypeList.Add(new EggsManager(name1, number1, hasEgg1));*/

        /*var Json = JsonConvert.SerializeObject(eggtypeList);

        Debug.Log(Json);*/

        /*_eggsCollection =  System.Enum.GetValues(typeof(EggTypes))
                            .Cast<EggTypes>()
                            .ToList();*/
    }
    public void loadEggCollection()
    {
        _jsonController.JsonLoad();
        _eggCollection = _jsonController._eggtypeList;        
    }
    public void loadTempEggCollection()
    {
        _jsonController.JsonLoadOrginal();
        _tempEggCollection = _jsonController._eggtypeList;

    }


    public void loadItemCollection()
    {
        _jsonController.JsonLoadItem();
        _itemCollection = _jsonController._itemmanager;
    }



    // we don't use this functions
    public void stopGame(bool _isStop)
    {
        if (_isStop)
        {
            for (int i = 0; i < 2; i++)
            {
                print(_spawnedEggs[i]);
                if (_spawnedEggs[i] != null)
                {
                    _spawnedEggs[i].gameObject.SetActive(false);
                }
            }

        }
        else
        {
            for (int i = 0; i < 2; i++)
            {
                print(_spawnedEggs[i]);
                if (_spawnedEggs[i] != null)
                {
                    _spawnedEggs[i].gameObject.SetActive(false);
                }
            }

        }

    }
}