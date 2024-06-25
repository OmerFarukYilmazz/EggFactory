using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public enum PoisonNumbers { n0, n1, n2, n3, n4, n5, n6, n7, n8, n9 };
public class PoisonEgg : MonoBehaviour, IPointerDownHandler{
    

    public PoisonNumbers EggNumbers;
    public GameObject _parachute;
    public GameObject _rocket;
    public GameObject _ballons;
    public GameObject _airballon;
    public GameObject _partyhat;
    public GameObject _noelhat;

    [SerializeField] public GameObject _animeObject;
    [HideInInspector] private GameObject _catchSoundError;
    [HideInInspector] private GameObject _basketSound;
    [SerializeField] private GameObject _shine;
    

    GameManager _gameManager;    
    StopMenuManager _stopMenuManager;
    JsonController _jsonController;
    public SpriteRenderer _spriteRenderer;
    public SpriteRenderer _animeSpriteRenderer;
    [SerializeField] public Animator _animator;
    Rigidbody2D _rigidbody;

    Vector2 _direction = Vector2.zero;
    bool _ontrigger;
    public float _collisionTime = 0;

    public PoisonEgg(GameObject catchSoundError)
    {
        _catchSoundError = catchSoundError;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        _gameManager = Object.FindObjectOfType<GameManager>();        
        _stopMenuManager = Object.FindObjectOfType<StopMenuManager>();
        _jsonController = Object.FindObjectOfType<JsonController>();
        _animator = GetComponent<Animator>();     
        _rigidbody = GetComponent<Rigidbody2D>();

        _catchSoundError = GameObject.Find("AudioCompenent/Catch");
        _basketSound = GameObject.Find("AudioCompenent/Basket");
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        
        /*
        foreach (var egg in _gameManager._eggCollection)
        {
            if (egg.EggType == this.EggType.ToString() && egg.EggNumber == (int)this.EggNumbers && egg.HasEgg == false)
            {
                _catchSound.GetComponent<AudioSource>().Play();
                egg.HasEgg = true;
                egg.TotalEgg++;
                _jsonController.JsonSave(_gameManager._eggCollection);
                _stopMenuManager.openCollectEggScreen(this.EggType.ToString(), (int)this.EggNumbers);
                Destroy(gameObject);
            }
        }
        */

    }


    private void OnCollisionStay2D(Collision2D target)
    {
        /*_collisionTime += Time.deltaTime;
        foreach (AnimatorControllerParameter parameter in _animator.parameters)
        {
            _animator.SetBool(parameter.name, false);
        }*/

        if (target.gameObject.tag == "Basket")
        {
            if (_ontrigger == false)
            {
                _gameManager.updateHeart();               
                Destroy(gameObject, 0.15f);
                _ontrigger = true;
            }            
            //_basketSound.GetComponent<AudioSource>().Play();
            //_gameManager._rabbit.GetComponent<Animator>().SetTrigger("Jump");
            Destroy(gameObject);
        }


    }
    
}
