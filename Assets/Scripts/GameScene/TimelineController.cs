using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;
public class TimelineController : MonoBehaviour
{
    public SpriteRenderer _EggAnime;
    public PlayableDirector _playableDirector;
    public PlayableDirector _playableDirectorMainScene;
    //private Action startAction;
    public GameManager _gameManager;
    [SerializeField] public StopMenuManager _stopMenuManager;
    [SerializeField] public CameraController _cameraController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //startAction = startGame;

    }
    private void Awake()
    {
        _gameManager = UnityEngine.Object.FindObjectOfType<GameManager>();
        _stopMenuManager = UnityEngine.Object.FindObjectOfType<StopMenuManager>();
        _cameraController = UnityEngine.Object.FindObjectOfType<CameraController>();
    }
    public void play()
    {
        _playableDirector.Play();        
        //StartCoroutine(PlayTimelineRoutine(_playableDirector));//, startAction));
    }

    void OnEnable()
    {
        // I canceled two camera and stop game
        //_playableDirector.stopped += OnPlayableDirectorStopped;
    }

    void OnPlayableDirectorStopped(PlayableDirector aDirector)
    {
        if (_playableDirector == aDirector)
        {
            
            Debug.Log("PlayableDirector named " + aDirector.name + " is now stopped.");            
            _gameManager._isAnimationPlaying = false;
            _stopMenuManager._isGamePaused = false;
            _cameraController._animationCamera.gameObject.SetActive(false);
            _cameraController._mainCamera.gameObject.SetActive(true);
            
            StartCoroutine(startGameAgain());
            // Time.timeScale = 1f;
            _gameManager.Game();
        }
            

    }
    IEnumerator startGameAgain()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 1f;
    }







    // 1) if used gametime this method work because used the waitforseconds method for learning when timeline is finished.

    /*private IEnumerator PlayTimelineRoutine(PlayableDirector playableDirector)//, Action onComplete)
    {
        playableDirector.Play();
        yield return new WaitForSeconds((float)playableDirector.duration);
        print("sss");
        Time.timeScale = 1f;
        _gameManager._isAnimationPlaying = false;
        _stopMenuManager._isGamePaused = false;
        _cameraController._animationCamera.gameObject.SetActive(false);
        _cameraController._mainCamera.gameObject.SetActive(true);

        //_gameManager.Game();
    }*/


    // 2) if u use action event enter code here. ant fix enumator vs.
    public void startGame()
    {
        
    }

}
