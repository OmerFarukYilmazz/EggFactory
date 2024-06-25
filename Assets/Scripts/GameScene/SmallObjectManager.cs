using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SmallObjectManager : MonoBehaviour
{
    CameraController _cameraController;
    GameManager _gameManager;
    [HideInInspector] public Animator _animator;
    [HideInInspector] GameObject _touchSound;
    [HideInInspector] GameObject _smallObject_ImpactSound;
    [HideInInspector] GameObject _smallObjectSound;
    [HideInInspector] GameObject _explosionSound;

    [SerializeField] private GameObject _explode;

    public bool _onCollision;
    public float _fireSpeed;
    bool _ontrigger;

    private void Awake()
    {
        _cameraController = Object.FindObjectOfType<CameraController>();
        _gameManager = Object.FindObjectOfType<GameManager>();
        _smallObjectSound = GameObject.Find("AudioCompenent/Fireball");
        _smallObject_ImpactSound = GameObject.Find("AudioCompenent/Fireball_Impact");
        _touchSound = GameObject.Find("AudioCompenent/Flame");
        _explosionSound = GameObject.Find("AudioCompenent/Explode");
    }
    // Start is called before the first frame update
    void Start()
    {
        moveFire();
        _smallObjectSound.GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position += new Vector3(0f, -_fireSpeed * Time.deltaTime , 0f);  // for try.
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Basket")
        {
            _smallObject_ImpactSound.GetComponent<AudioSource>().Play();
            //collision.gameObject.GetComponent<Animator>().Play("basket_fire", -1, 0f);
            // StartCoroutine(_cameraController.Shake(0.2f, 0.1f)); // we use in updateHeart
            Destroy(gameObject);
            _gameManager.updateHeart();
        }
        if (collision.gameObject.tag == "Egg" || collision.gameObject.tag == "Egg_Poison")
        {
            _touchSound.GetComponent<AudioSource>().Play();
            //collision.gameObject.GetComponent<Animator>().Play("Egg_Fire", -1, 0f);
            print(collision.gameObject.tag);

            if (collision.gameObject.tag == "Egg_Poison")
            {
                StartCoroutine(expolosion(collision.gameObject));
                print("ssadasdfsafasf");
            }
            Destroy(collision.gameObject, 0.5f);                      
        }
        if (collision.gameObject.tag == "Gift")
        {
            _touchSound.GetComponent<AudioSource>().Play();
            //collision.gameObject.GetComponent<Animator>().Play("Gift_Fire", -1, 0f);
            Destroy(collision.gameObject,0.5f);
        }
    }

    IEnumerator expolosion(GameObject _gameObject)
    {
        Vector2 _temp = _gameObject.transform.position;
        yield return new WaitForSeconds(0.3f);
        _explosionSound.GetComponent<AudioSource>().Play();
        Instantiate(_explode, _temp, Quaternion.identity);
        print("sasdaasdasds");
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
