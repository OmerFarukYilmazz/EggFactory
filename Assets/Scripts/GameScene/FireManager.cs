using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FireManager : MonoBehaviour
{
    CameraController _cameraController;
    GameManager _gameManager;
    BossManager _bossManager;
    [HideInInspector] public Animator _animator;
    [HideInInspector] GameObject _flameSound;
    [HideInInspector] GameObject _fireBall_ImpactSound;
    [HideInInspector] GameObject _fireBallSound;
    [HideInInspector] GameObject _explosionSound;

    [SerializeField] private GameObject _explode;

    public bool _onCollision;
    public float _fireSpeed;
    bool _ontrigger;

    string basket_touch;
    string egg_touch;
    string gift_touch;

    private void Awake()
    {
        _cameraController = Object.FindObjectOfType<CameraController>();
        _gameManager = Object.FindObjectOfType<GameManager>();
        _bossManager = Object.FindObjectOfType<BossManager>();

        _fireBallSound = GameObject.Find("AudioCompenent/Fireball");
        _fireBall_ImpactSound = GameObject.Find("AudioCompenent/Fireball_Impact");
        _flameSound = GameObject.Find("AudioCompenent/Flame");
        _explosionSound = GameObject.Find("AudioCompenent/Explode");

        setSettings();
    }
    // Start is called before the first frame update
    void Start()
    {
        moveFire();
        _fireBallSound.GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position += new Vector3(0f, -_fireSpeed * Time.deltaTime , 0f);  // for try.
    }

    void setSettings()
    {
        switch (_bossManager.bossAnimationType)
        {
            case "Frog":
                GetComponent<Animator>().SetBool("Frog", true);
                //basket_touch = "basket_frogEgg";
                break;
            case "Turtle":
                GetComponent<Animator>().SetBool("Turtle", true);                
                break;
            case "Bat":
                GetComponent<Animator>().SetBool("Bat", true);
                break;
            case "Panda":
                GetComponent<Animator>().SetBool("Panda", true);
                break;
            case "Duck":
                GetComponent<Animator>().SetBool("Duck", true);                
                break;
            case "Koala":
                GetComponent<Animator>().SetBool("Koala", true);
                break;
            case "Rabbit":
                GetComponent<Animator>().SetBool("Rabbit", true);
                break;
            case "Squirrel":
                GetComponent<Animator>().SetBool("Squirrel", true);
                break;
            case "FlyRed":
            case "FlyGold":
            case "FlyRedTwoHead":
            case "FlyBlueTwoHead":
                GetComponent<Animator>().SetBool("Fire", true);
                basket_touch = "basket_fire";
                egg_touch = "Egg_Fire";
                gift_touch = "Gift_Fire";
                break;
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Basket")
        {
            _fireBall_ImpactSound.GetComponent<AudioSource>().Play();    
            if(basket_touch != null || basket_touch == "")
            {
                collision.gameObject.GetComponent<Animator>().Play(basket_touch, -1, 0f);
            }            
            // StartCoroutine(_cameraController.Shake(0.2f, 0.1f)); // we use in updateHeart
            Destroy(gameObject);
            _gameManager.updateHeart();
        }
        if (collision.gameObject.tag == "Egg" || collision.gameObject.tag == "Egg_Poison")
        {
            _flameSound.GetComponent<AudioSource>().Play();
            if(egg_touch != null || egg_touch == "")
            {
                collision.gameObject.GetComponent<Animator>().Play(egg_touch, -1, 0f);
            }
            
            print(collision.gameObject.tag);

            if (collision.gameObject.tag == "Egg_Poison")
            {
                StartCoroutine(expolosion(collision.gameObject));                
            }
            Destroy(collision.gameObject, 0.5f);                      
        }
        if (collision.gameObject.tag == "Gift")
        {
            _flameSound.GetComponent<AudioSource>().Play();
            if(gift_touch != null || gift_touch == "")
            {
                collision.gameObject.GetComponent<Animator>().Play(gift_touch, -1, 0f);
            }            
            Destroy(collision.gameObject,0.5f);
        }
    }

    IEnumerator expolosion(GameObject _gameObject)
    {
        Vector2 _temp = _gameObject.transform.position;
        yield return new WaitForSeconds(0.3f);
        _explosionSound.GetComponent<AudioSource>().Play();
        Instantiate(_explode, _temp, Quaternion.identity);
        //print("sasdaasdasds");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bottom")
        {
            if (_ontrigger == false)
            {                
                Destroy(gameObject);
                _ontrigger = true;
            }
        }
    }

    private void moveFire()
    {        
        GetComponent<Rigidbody2D>().velocity = Vector2.down * _fireSpeed;
    }





    /*
    private void OnCollisionEnter2D (Collision2D collision)
    {
        if (collision.gameObject.tag == "Basket" )
        {
            collision.gameObject.GetComponent<Animator>().Play("basket_fire", -1, 0f);
            // StartCoroutine(_cameraController.Shake(0.2f, 0.1f)); // we use in updateHeart
            Destroy(gameObject);
            _gameManager.updateHeart();            
        }
        if(collision.gameObject.tag == "Egg")
        {
            collision.gameObject.GetComponent<Animator>().Play("Egg_Fire", -1, 0f);
        }
        if (collision.gameObject.tag == "Gift")
        {
            gameObject.GetComponent<Animator>().Play("Gift_Fire", -1, 0f);
        }
        
    }
    */
    /*private void OnTriggerExit2D(Collider2D collision)
    {
        print("ss");
        if (collision.gameObject.tag == "Basket")
        {
            StartCoroutine(_cameraController.Shake(0.2f, 0.1f));
            _gameManager.updateHeart();
            Destroy(gameObject, 0.15f);
        }
    }*/


}
