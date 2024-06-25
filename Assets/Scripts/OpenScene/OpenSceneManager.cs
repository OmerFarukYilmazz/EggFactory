using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenSceneManager : MonoBehaviour
{
    public string _loadScene;
    [SerializeField] private float time = 5f;
    private void Update()
    {
        
        StartGame();      
    }
    private void Start()
    {        
        Screen.orientation = ScreenOrientation.Portrait;
        AudioListener.volume = 1;
        PlayerPrefs.SetInt("VolumeOn", 1);
        PlayerPrefs.SetInt("SoundOn", 1);
        //PlayerPrefs.SetString("isTimerActive", "true");
    }

    public void StartGame()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            SceneManager.LoadScene(_loadScene);
        }

    }
    
}
