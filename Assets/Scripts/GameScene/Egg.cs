using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//using System;

public enum EggTypes { Normal, EasterEast, EasterWest, Colorful, Lily, Flat, Daisy, Traditional, Spring, WaterColor, HandDrawn, Realistic, Flower, Goldens, GoldBlack, GoldWhite, Gift, Dragon, Special, Dracon };
public enum EggsNumbers { n0, n1, n2, n3, n4, n5, n6, n7, n8, n9 };
public enum Color_Magic {Yellow, Red, Purple, Blue, Green, Pink};
public class Egg : MonoBehaviour , IPointerDownHandler
{
    public EggTypes EggType;
    public EggsNumbers EggNumbers;
    public GameObject _parachute;
    public GameObject _rocket;
    public GameObject _ballons;
    public GameObject _airballon;
    public GameObject _partyhat;
    public GameObject _noelhat;

    [SerializeField] public GameObject _animeObject;
    [HideInInspector] private GameObject _catchSound;
    [HideInInspector] private GameObject _basketSound;
    [HideInInspector] private GameObject _poisonSound;
    [HideInInspector] private GameObject _hurt_PoisonSound;
    [SerializeField] private GameObject _shine;
    public List<GameObject> _magicSmoke = new List<GameObject>();
    //public SmokeController _smokeController;
    public GameObject instantiatedObj;
    bool _smoke;
    //Vector2 _touchPos;

    GameManager _gameManager;
    //CameraController _cameraController;
    StopMenuManager _stopMenuManager;
    JsonController _jsonController;
    public SpriteRenderer _spriteRenderer;
    public SpriteRenderer _animeSpriteRenderer;
    [SerializeField] public Animator _animator;
    Rigidbody2D _rigidbody;

    Vector2 _direction = Vector2.zero;
    //bool _dragging;
    bool _ontrigger;
    bool _oncollisionStay;
    public float _collisionTime=0;

    //float _eggMoveAngle;
    //float _moveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        checkEgginCollection();
    }

    // Update is called once per frame
    void Update()
    {
        if(_smoke == true)
        {
            instantiatedObj.transform.position = this.transform.position;
        }
    }
    private void Awake()
    {
        _gameManager = Object.FindObjectOfType<GameManager>();
        //_cameraController = Object.FindObjectOfType<CameraController>();
        _stopMenuManager = Object.FindObjectOfType<StopMenuManager>();
        _jsonController = Object.FindObjectOfType<JsonController>();
        _animator = GetComponent<Animator>();
        //_spriteRenderer = Object.FindObjectOfType<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
        //_smokeController = Object.FindObjectOfType<SmokeController>();


        _catchSound = GameObject.Find("AudioCompenent/Catch");
        _basketSound = GameObject.Find("AudioCompenent/Basket");
        _poisonSound = GameObject.Find("AudioCompenent/Poison");
        _hurt_PoisonSound = GameObject.Find("AudioCompenent/Hurt_Poison");

    }



    public void checkEgginCollection()
    {
        if (gameObject.tag == "Egg_Poison") 
        {
            int num = (int)this.EggNumbers;
            string color = System.Enum.GetName(typeof(Color_Magic), num);
            //print(color + "" + num);
            instantiatedObj = Instantiate(_magicSmoke[num], this.transform.position, Quaternion.identity);
            _smoke = true;
            _poisonSound.GetComponent<AudioSource>().Play();
            //this.gameObject.transform.Find("Smoke").transform.Find("Magic_Blue").gameObject.SetActive(true);
            return;
        }
        foreach (var egg in _gameManager._eggCollection)
        {
            if (egg.EggType == this.EggType.ToString() && egg.EggNumber == (int)this.EggNumbers && egg.HasEgg == false)
            {
                _shine.SetActive(true);                
            }
        }
        
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        //print("Catch");        
        if (gameObject.tag == "Egg_Poison") { return; }

        foreach (var egg in _gameManager._eggCollection)
        {
            if (egg.EggType == this.EggType.ToString() && egg.EggNumber == (int)this.EggNumbers && egg.HasEgg==false)
            {
                _catchSound.GetComponent<AudioSource>().Play();
                egg.HasEgg = true;
                egg.TotalEgg++;
                _jsonController.JsonSave(_gameManager._eggCollection);
                _stopMenuManager.openCollectEggScreen(this.EggType.ToString(), (int)this.EggNumbers);
                Destroy(gameObject);
            }
        }
        

    }




    private void OnTriggerExit2D(Collider2D collision)
    {
        if (gameObject.tag == "Egg_Poison")
        {
            Destroy(gameObject, 0.15f);
            Destroy(instantiatedObj, 0.15f);
            _gameManager._numberofBasketObject++;
            return; 
        }

        if (collision.gameObject.tag == "Bottom")
        {            
            if(_ontrigger == false)
            {                
                if (_gameManager._isRainEgg == false)
                {
                    _gameManager.updateHeart();
                }
                Destroy(gameObject, 0.15f);
                _ontrigger = true;
            }            
        }
    }
    private void OnCollisionStay2D(Collision2D target)
    {
        foreach (AnimatorControllerParameter parameter in _animator.parameters)
        {
            _animator.SetBool(parameter.name, false);
        }

        _collisionTime += Time.deltaTime;
        
        //print(gameObject.tag.ToString());

        if (target.gameObject.tag == "Basket" && gameObject.tag == "Egg_Poison")
        {
            if (!_oncollisionStay)
            {
                _hurt_PoisonSound.GetComponent<AudioSource>().Play();
                _gameManager.updateHeart();
                _oncollisionStay = true;
            }
            
            if (_collisionTime >= 0.3f)
            {
                Destroy(gameObject);
                Destroy(instantiatedObj);
                return;
            }
            
            return;
            
        }

        if (target.gameObject.tag == "Basket" && _collisionTime >=0.3f)
        {            
            int type = (int)this.EggType;
            int num = (int)this.EggNumbers;
            /*
            if(_gameManager._isRainEgg ==false) //&& _gameManager._heart<10)
            { _stopMenuManager.updateHeartBar(); }
            */
            _stopMenuManager.updateHeartBar();
            _gameManager.updateScore(type, num);
            foreach (var egg in _gameManager._tempEggCollection)
            {
                if (egg.EggType == this.EggType.ToString() && egg.EggNumber == (int)this.EggNumbers)
                {                    
                    egg.HasEgg = true;
                    egg.TotalEgg++;                    
                }
            }
            _basketSound.GetComponent<AudioSource>().Play();
            _gameManager._rabbit.GetComponent<Animator>().SetTrigger("Jump");
            Destroy(gameObject);            
        }
       

    }

    public class PoisonEgg : Egg
    {
        
        
    }

   













    /*
    public void moveFNC()
    {
        int rnd = Random.Range(0, 5);
        rnd = 0;
        switch (rnd)
        {
            case 0:
                print("ssss");
                _direction = Vector2.down;
                _direction.x = Random.Range(-_eggMoveAngle, _eggMoveAngle);
                _rigidbody.velocity = _direction * _moveSpeed;
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;

        }
            
        

    }
    */

    /*private void OnCollisionEnter2D(Collision2D target)
    {        
        foreach (AnimatorControllerParameter parameter in _animator.parameters)
        {
            _animator.SetBool(parameter.name, false);
        }
        if (target.gameObject.tag == "Stork")
        {
            print("Stork");
            this.transform.position = target.transform.position;
          
        }


    }*/





}
