using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossManager : MonoBehaviour
{   

    public bool _isBossActive;

    public GameObject fire;
    public GameObject frog_egg;
    public GameObject turtle_shell;
    public GameObject bat_wave;
    public GameObject panda_bambu;
    public GameObject rabbit_carrot;
    public GameObject duck_bathDuck;
    public GameObject koala_bough;
    public GameObject squirrel_acorn;

    public GameObject frog;
    public GameObject turtle;
    public GameObject bat;
    public GameObject panda;
    public GameObject rabbit;
    public GameObject duck;
    public GameObject koala;
    public GameObject squirrel;
    public GameObject dragonRed;
    public GameObject dragonGold;
    public GameObject dragonRedTwoHead;
    public GameObject dragonBlueTwoHead;   
        
    public string bossAnimationType;    
    [HideInInspector] public Animator _animator;
    [HideInInspector] public int _bossOrder;
    [HideInInspector] public int _fireStart;
    [HideInInspector] public int _fireFinish;
    [HideInInspector] public int _fireCounter;
    GameManager _gameManager;
    
    //[HideInInspector] public GameObject _bossObject;
    [SerializeField] public Transform targetLeft;
    [SerializeField] public Transform targetRight;
    [SerializeField] public Transform _firePos1;
    [SerializeField] public Transform _firePos2;

    public float _dragonSpeed = 2f;

    /*
    Vector2 target_position;
    Vector2 v;    

    private bool _directionRight;
    private float _fireCounter;    
    private float _waitingCounter;
    private float _moveCounter;
    float _randomTime;
    */

    private void Awake()
    {
        _gameManager = Object.FindObjectOfType<GameManager>();

        //targetLeft.parent = null;
        //targetRight.parent = null;
        //v = (Vector2)targetRight.position - (Vector2)targetLeft.position;
        //target_position = (Vector2)targetLeft.position + Random.value * v;
                
        

        //_moveCounter = Random.Range(0, 5);
        //_dragonRoarSound = GameObject.Find("AudioCompenent/Dragon_Roar");
        //_dragonFlySound = GameObject.Find("AudioCompenent/Dragon_Fly");
        //_fireCounter = Random.Range(1, 3);
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void checkBossOrder()
    {
        if (_isBossActive)
        {            
            switch (_bossOrder)
            {
                case (1):
                    frog.SetActive(true);
                    bossAnimationType = "Frog";
                    fire = frog_egg;                    
                    //_tempBoss = dragonRed;
                    //moveObject(dragonRed, bossAnimationType);
                    break;
                case (2):
                    turtle.SetActive(true);
                    bossAnimationType = "Turtle";
                    fire = turtle_shell;
                    //fire = 
                    //fire = 
                    //_tempBoss = dragonGold;
                    //moveObject(dragonGold, bossAnimationType);
                    break;
                case (3):
                    bat.SetActive(true);
                    bossAnimationType = "Bat";
                    fire = bat_wave;
                    //_tempBoss = dragonRedTwoHead;
                    //moveObject(dragonRedTwoHead, bossAnimationType);
                    break;
                case (4):
                    panda.SetActive(true);
                    bossAnimationType = "Panda";
                    fire = panda_bambu;
                    //_tempBoss = dragonBlueTwoHead;
                    //moveObject(dragonBlueTwoHead, bossAnimationType);
                    break;
                case (5):
                    duck.SetActive(true);
                    bossAnimationType = "Duck";
                    fire = duck_bathDuck;
                    //_tempBoss = dragonRed;
                    //moveObject(dragonRed, bossAnimationType);
                    break;
                case (6):
                    koala.SetActive(true);
                    bossAnimationType = "Koala";
                    fire = koala_bough;
                    //fire = 
                    //fire = 
                    //_tempBoss = dragonGold;
                    //moveObject(dragonGold, bossAnimationType);
                    break;
                case (7):
                    rabbit.SetActive(true);
                    bossAnimationType = "Rabbit";
                    fire = rabbit_carrot;
                    //_tempBoss = dragonRedTwoHead;
                    //moveObject(dragonRedTwoHead, bossAnimationType);
                    break;
                case (8):
                    squirrel.SetActive(true);
                    bossAnimationType = "Squirrel";
                    fire = squirrel_acorn;
                    //_tempBoss = dragonBlueTwoHead;
                    //moveObject(dragonBlueTwoHead, bossAnimationType);
                    break;
                case (9):
                    dragonRed.SetActive(true);
                    bossAnimationType = "FlyRed";
                    //_tempBoss = dragonRed;
                    //moveObject(dragonRed, bossAnimationType);
                    break;
                case (10):
                    dragonGold.SetActive(true);
                    bossAnimationType = "FlyGold";
                    //_tempBoss = dragonGold;
                    //moveObject(dragonGold, bossAnimationType);
                    break;
                case (11):
                    dragonRedTwoHead.SetActive(true);
                    bossAnimationType = "FlyRedTwoHead";
                    //_tempBoss = dragonRedTwoHead;
                    //moveObject(dragonRedTwoHead, bossAnimationType);
                    break;
                case (12):
                    dragonBlueTwoHead.SetActive(true);
                    bossAnimationType = "FlyBlueTwoHead";
                    print("s"); 
                    //_tempBoss = dragonBlueTwoHead;
                    //moveObject(dragonBlueTwoHead, bossAnimationType);
                    break;

            }            
            setBossPower();
        }
    }

    void setBossPower()
    {        
        int _levelTemp = (_gameManager._level - 1) % 200;        
        switch (_levelTemp/10)
        {
            case int i when (i >= 0 && i < 2):
                _fireStart = 3;
                _fireFinish = 6;
                _fireCounter = 1;
                //PlayerPrefs.SetInt("Fire_Start", 2);
                //PlayerPrefs.SetInt("Fire_Finish", 6);
                //_fireCounter = Random.Range(PlayerPrefs.GetInt("Fire_Start"), PlayerPrefs.GetInt("Fire_Finish"));
                _dragonSpeed = 2f;
                break;
            case int i when (i >= 2 && i < 4):
                _fireStart = 2;
                _fireFinish = 6;
                _fireCounter = 1;
                //PlayerPrefs.SetInt("Fire_Start", 2);
                //PlayerPrefs.SetInt("Fire_Finish", 5);
                //_fireCounter = Random.Range(PlayerPrefs.GetInt("Fire_Start"), PlayerPrefs.GetInt("Fire_Finish"));
                _dragonSpeed = 2.5f;
                break;
            case int i when (i >= 4 && i < 8):
                _fireStart = 2;
                _fireFinish = 6;
                _fireCounter = 2;
                //PlayerPrefs.SetInt("Fire_Start", 1);
                //PlayerPrefs.SetInt("Fire_Finish", 5);
                //_fireCounter = Random.Range(PlayerPrefs.GetInt("Fire_Start"), PlayerPrefs.GetInt("Fire_Finish"));
                _dragonSpeed = 3f;
                break;
            case int i when (i >= 8 && i < 12):
                _fireStart = 2;
                _fireFinish = 5;
                _fireCounter = 2;
                //PlayerPrefs.SetInt("Fire_Start", 1);
                //PlayerPrefs.SetInt("Fire_Finish", 5);
                //_fireCounter = Random.Range(PlayerPrefs.GetInt("Fire_Start"), PlayerPrefs.GetInt("Fire_Finish"));
                _dragonSpeed = 3.5f;
                break;
            case int i when (i >= 12 && i < 16):
                _fireStart = 2;
                _fireFinish = 5;
                _fireCounter = 2;
                //PlayerPrefs.SetInt("Fire_Start", 1);
                //PlayerPrefs.SetInt("Fire_Finish", 4);
                //_fireCounter = Random.Range(PlayerPrefs.GetInt("Fire_Start"), PlayerPrefs.GetInt("Fire_Finish"));
                _dragonSpeed = 4f;
                break;
            case int i when (i >= 16 && i < 18):
                _fireStart = 1;
                _fireFinish = 5;
                _fireCounter = 2;
                //PlayerPrefs.SetInt("Fire_Start", 1);
                //PlayerPrefs.SetInt("Fire_Finish", 3);
                //_fireCounter = Random.Range(PlayerPrefs.GetInt("Fire_Start"), PlayerPrefs.GetInt("Fire_Finish"));
                _dragonSpeed = 4.5f;
                break;
            case int i when (i >= 18 && i < 20):
                _fireStart = 1;
                _fireFinish = 4;
                _fireCounter = 3;
                //PlayerPrefs.SetInt("Fire_Start", 0);
                //PlayerPrefs.SetInt("Fire_Finish", 3);
                //_fireCounter = Random.Range(PlayerPrefs.GetInt("Fire_Start"), PlayerPrefs.GetInt("Fire_Finish"));
                _dragonSpeed = 5f;
                break;


        }
    }

    



    /*
    public void moveObject(GameObject _object, string animationType)
    {
        
        //_object.GetComponent<Animator>().SetBool(animationType, true);
        if (_moveCounter > 0)
        {            
            _moveCounter -= Time.deltaTime;
            _fireCounter -= Time.deltaTime;
            if (_fireCounter <= 0)
            {
                if(_bossOrder == 3 || _bossOrder == 4)
                {
                    int rnd = Random.Range(PlayerPrefs.GetInt("Fire_Start"), PlayerPrefs.GetInt("Fire_Finish"));
                    if (rnd == 2)
                    {
                        _firePos2 = _object.transform.Find("Visual").transform.Find("Fire (1)");
                        Instantiate(fire, _firePos2.position, _firePos2.rotation);
                    }                    
                }
                _firePos1 = _object.transform.Find("Visual").transform.Find("Fire");
                Instantiate(fire, _firePos1.position, _firePos1.rotation);                
                _fireCounter = Random.Range(PlayerPrefs.GetInt("Fire_Start"), PlayerPrefs.GetInt("Fire_Finish"));
            }
            if (_directionRight)
            {              
                _object.transform.position = Vector2.MoveTowards(_object.transform.position, target_position, _dragonSpeed * Time.deltaTime);
                if (Vector2.Distance(_object.transform.position, target_position) < 0.1f)
                {
                    target_position = (Vector2)targetRight.position - Random.value * v;
                    _directionRight = false;
                }

            }
            else
            {
                

                _object.transform.position = Vector2.MoveTowards(_object.transform.position, target_position, _dragonSpeed * Time.deltaTime);
                if (Vector2.Distance(_object.transform.position, target_position) < 0.1f)
                {
                    target_position = (Vector2)targetLeft.position + Random.value * v;
                    _directionRight = true;
                }
            }
            if (_moveCounter <= 0)
            {
                //_dragonFlySound.GetComponent<AudioSource>().Play();
                _dragonRoarSound.GetComponent<AudioSource>().Play();
                _waitingCounter = Random.Range(0, 2);
            }
        }
        else
        {
            //_dragonFlySound.GetComponent<AudioSource>().Pause();
            _waitingCounter -= Time.timeScale;
            if (_waitingCounter < 0)
            {
                _moveCounter = Random.Range(0, 5);
            }
        }
    }
    */
    /*dragon.transform.position = Vector3.MoveTowards(dragon.transform.position, targetLeft.position, _dragonSpeed * Time.deltaTime);
                if (Vector3.Distance(dragon.transform.position, targetLeft.position) < 0.1f)
                {
                    _directionRight = true;
                }*/


}
