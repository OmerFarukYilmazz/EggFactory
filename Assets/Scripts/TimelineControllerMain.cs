using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class TimelineControllerMain : MonoBehaviour
{    
    public PlayableDirector _playableDirector;
    public TimelineAsset _collectionScene;
    public TimelineAsset _mainScene;
    public TimelineAsset _gameScene;
    public Scene _currentScene;

    StopMenuManager _stopMenuManager;

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
        
    }
    public void play(String sceneName)
    {
        switch (sceneName)
        {
            case "MainScene":
                _playableDirector.playableAsset = _mainScene;
                break;
            case "CollectionScene":
                _playableDirector.playableAsset = _collectionScene;
                break;           
            case "GameScene":
                _playableDirector.playableAsset = _gameScene;
                break;

        }        
        _playableDirector.Play();
        //_playableDirector.SetGenericBinding()
        //StartCoroutine(PlayTimelineRoutine(_playableDirector));//, startAction));

    }
    void OnEnable()
    {
        // I canceled two camera and stop game
        _playableDirector.stopped += OnPlayableDirectorStopped;
    }

    void OnPlayableDirectorStopped(PlayableDirector aDirector)
    {
        if (_playableDirector == aDirector)
        {
            
            Debug.Log("PlayableDirector named " + aDirector.name + " is now stopped.");            
            //_gameManager._isAnimationPlaying = false;
            //_stopMenuManager._isGamePaused = false;
            //_cameraController._animationCamera.gameObject.SetActive(false);
            //_cameraController._mainCamera.gameObject.SetActive(true);
            
            StartCoroutine(startGameAgain());
            // Time.timeScale = 1f;
            //_gameManager.Game();
        }
            

    }
    IEnumerator startGameAgain()
    {
        yield return new WaitForSecondsRealtime(0.2f);        
        Time.timeScale = 1f;
        
    }

}
