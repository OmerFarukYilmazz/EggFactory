using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public enum GiftTypes { Diamond_Blue, Diamond_Green, Diamond_Pink, Skull_1, Skull_2, Heart };
// public enum GiftNumbers { n0, n1, n2, n3, n4, n5, n6, n7, n8, n9 };

public class Gift : MonoBehaviour , IPointerDownHandler
{
    public GiftTypes GiftTypes;

    GameManager _gameManager;
    JsonController _jsonController;
    StopMenuManager _stopMenuManager;
    Rigidbody2D _rigidbody;
    public SpriteRenderer _spriteRenderer;
    //[SerializeField] public SpriteRenderer _spriteRenderer;
    [SerializeField] private GameObject _basketSound;
    [SerializeField] private GameObject _catchSound;
    [SerializeField] private GameObject _powerUpSound;
    [SerializeField] private GameObject _shine;

    Vector2 _direction = Vector2.zero;    
    float _collisionTime = 0;
    public float _giftMoveAngle = 0.1f;
    public float _moveSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        checkItemCollection();
        moveFnc();        
    }

    private void Awake()
    {
        _gameManager = Object.FindObjectOfType<GameManager>();
        _jsonController = Object.FindObjectOfType<JsonController>();
        _stopMenuManager = Object.FindObjectOfType<StopMenuManager>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _moveSpeed = PlayerPrefs.GetFloat("GiftSpeed");
        _giftMoveAngle = PlayerPrefs.GetFloat("GiftAngle");
        //_spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _basketSound = GameObject.Find("AudioCompenent/Basket");
        _catchSound = GameObject.Find("AudioCompenent/Catch");
        _powerUpSound = GameObject.Find("AudioCompenent/PowerUp");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void checkItemCollection()
    {
        if(this.GiftTypes.ToString() == "Heart")
        {
            return;
        }        
        foreach (var gift in _gameManager._itemCollection)
        {
            if(gift.Item == this.GiftTypes.ToString())
            {
                if(gift.HasItem == false)
                {
                    _shine.SetActive(true);
                }
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //print("Catch");
        if(this.GiftTypes.ToString() == "Heart")
        {
            _catchSound.GetComponent<AudioSource>().Play();
            _powerUpSound.GetComponent<AudioSource>().Play();
            _gameManager._heart++;            
            PlayerPrefs.SetInt("Heart", _gameManager._heart);            
            _gameManager._heartText.text = _gameManager._heart.ToString();
            //_stopMenuManager.openCollectGiftScreen(this.GiftTypes.ToString()); 
            Destroy(gameObject);
            return;
        }

        foreach (var gift in _gameManager._itemCollection)
        {
            if (gift.Item == this.GiftTypes.ToString())
            {
                _catchSound.GetComponent<AudioSource>().Play();
                if(gift.HasItem == false) { _stopMenuManager.openCollectGiftScreen(this.GiftTypes.ToString()); }                
                gift.HasItem = true;
                gift.TotalItem++;             
                _jsonController.JsonSaveItem(_gameManager._itemCollection);
                Destroy(gameObject);
            }
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BasketArea")
        {
             GetComponent<Rigidbody2D>().sharedMaterial = null;            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Bottom")
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionStay2D(Collision2D target)
    {
        _collisionTime += Time.deltaTime;       
        if (target.gameObject.tag == "Basket" && _collisionTime >= 0.1f)
        {
            _basketSound.GetComponent<AudioSource>().Play();
            int type = (int)this.GiftTypes;           
            _gameManager.updateScore(1, type);            
            _gameManager._rabbit.GetComponent<Animator>().SetTrigger("Jump");
            Destroy(gameObject);
        }

    }
    public void moveFnc()
    {        
        _direction = Vector2.down;
        _direction.x = Random.Range(-_giftMoveAngle, _giftMoveAngle);
        _rigidbody.velocity = _direction * _moveSpeed;
    }
}
