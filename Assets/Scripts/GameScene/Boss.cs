using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    BossManager _bossManager;    
    StopMenuManager _stopMenuManager;
    //public float _dragonSpeed;
    private bool _directionRight;

    private float _fireCounter;
    private float _waitingCounter;
    private float _moveCounter;

    Vector2 target_position;    
    Vector2 v;

    [SerializeField] private GameObject _bossMoveSound;
    [SerializeField] private GameObject _dragonRoarSound;
    /*
    [SerializeField] private GameObject _frogMoveSound;
    [SerializeField] private GameObject _turtleMoveSound;
    [SerializeField] private GameObject _pandaMoveSound;
    [SerializeField] private GameObject _batMoveSound;
    */

    // Start is called before the first frame update
    void Start()
    {
        _bossMoveSound.GetComponent<AudioSource>().Play();
    }
    private void Awake()
    {
        _stopMenuManager = Object.FindObjectOfType<StopMenuManager>();

        _bossManager = Object.FindObjectOfType<BossManager>();
        _fireCounter = Random.Range(_bossManager._fireStart, _bossManager._fireFinish);
        //_fireCounter = Random.Range(PlayerPrefs.GetInt("Fire_Start"), PlayerPrefs.GetInt("Fire_Finish")); // old method
        _moveCounter = Random.Range(0, 5);

        v = (Vector2)_bossManager.targetRight.position - (Vector2)_bossManager.targetLeft.position;
        target_position = (Vector2)_bossManager.targetLeft.position + Random.value * v;

        //_dragonRoarSound = GameObject.Find("AudioCompenent/Dragon_Roar");
        //_bossMoveSound = GameObject.Find("AudioCompenent/Dragon_Fly");
    }

    // Update is called once per frame
    private void Update()
    {
        if (_bossManager._isBossActive)
        {
            moveObject(this.gameObject, _bossManager.bossAnimationType);
            
            if (_stopMenuManager._isGamePaused == true )
            {
                if(_bossMoveSound.GetComponent<AudioSource>().mute != true)
                {
                    _bossMoveSound.GetComponent<AudioSource>().mute = true;
                }
            }                
            else 
            {
                if (_bossMoveSound.GetComponent<AudioSource>().mute != false)
                {
                    _bossMoveSound.GetComponent<AudioSource>().mute = false;
                }
            }
        }

    }
    public void moveObject(GameObject _object, string animationType)
    {

        _object.GetComponent<Animator>().SetBool(animationType, true);
        if (_moveCounter > 0)
        {
            _moveCounter -= Time.deltaTime;
            _fireCounter -= Time.deltaTime;
            if (_fireCounter <= 0)
            {                

                _fireCounter = Random.Range(_bossManager._fireStart, _bossManager._fireFinish);
                StartCoroutine(fire(_object));
                /*
                _bossManager._firePos1 = _object.transform.Find("Visual").transform.Find("Fire");
                Instantiate(_bossManager.frog_egg, _bossManager._firePos1.position, _bossManager._firePos1.rotation);
                print("ss");
                //_fireCounter = Random.Range(PlayerPrefs.GetInt("Fire_Start"), PlayerPrefs.GetInt("Fire_Finish"));
                //Instantiate(_bossManager.fire, _bossManager._firePos1.position, _bossManager._firePos1.rotation);
                _fireCounter = Random.Range(_bossManager._fireStart, _bossManager._fireFinish);
                */
            }
            if (_directionRight)
            {
                /*dragon.transform.position = Vector3.MoveTowards(dragon.transform.position, targetRight.position, _dragonSpeed * Time.deltaTime);
                if (Vector3.Distance(dragon.transform.position, targetRight.position) < 0.1f)
                {
                    _directionRight = false;
                }*/

                _object.transform.position = Vector2.MoveTowards(_object.transform.position, target_position, _bossManager._dragonSpeed * Time.deltaTime);
                if (Vector2.Distance(_object.transform.position, target_position) < 0.1f)
                {
                    target_position = (Vector2)_bossManager.targetRight.position - Random.value * v;
                    _directionRight = false;
                }

            }
            else
            {
                /*dragon.transform.position = Vector3.MoveTowards(dragon.transform.position, targetLeft.position, _dragonSpeed * Time.deltaTime);
                if (Vector3.Distance(dragon.transform.position, targetLeft.position) < 0.1f)
                {
                    _directionRight = true;
                }*/

                _object.transform.position = Vector2.MoveTowards(_object.transform.position, target_position, _bossManager._dragonSpeed * Time.deltaTime);
                if (Vector2.Distance(_object.transform.position, target_position) < 0.1f)
                {
                    target_position = (Vector2)_bossManager.targetLeft.position + Random.value * v;
                    _directionRight = true;
                }
            }
            if (_moveCounter <= 0)
            {           
                if(_bossManager.bossAnimationType == "FlyRed" || _bossManager.bossAnimationType == "FlyGold" || _bossManager.bossAnimationType == "FlyRedTwoHead" || _bossManager.bossAnimationType == "FlyBlueTwoHead")
                {
                    _dragonRoarSound.GetComponent<AudioSource>().Play();
                }                
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

    IEnumerator fire (GameObject _object)
    {        
        int count = _bossManager._fireCounter;        
        
        Transform _firePos1 = _object.transform.Find("Visual").transform.Find("Fire");       

        float time;
        for (int i = 0; i < count; i++)
        {
            time = Random.Range(0.1f, 1f);
            yield return new WaitForSeconds(time);
            if (gameObject.name == "DragonRedTwoHead" || gameObject.name == "DragonBlueTwoHead")
            {
                int rnd = Random.Range(1, 4);
                if (rnd == 2)
                {
                    _firePos1 = _object.transform.Find("Visual").transform.Find("Fire (1)");
                    
                }
            }
            Instantiate(_bossManager.fire, _firePos1.position, _firePos1.rotation);


        }
        
        
    }
}
