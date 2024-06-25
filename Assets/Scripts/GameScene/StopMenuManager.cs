using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Reflection;

public class StopMenuManager : MonoBehaviour
{
    GameManager _gameManager;
    public AudioSource _audioSource;
    public AudioSource _clickforVolumeandSound;

    [SerializeField] private Toggle _volume, _sound;
    //[SerializeField] public GameObject _pauseMenu;
    [SerializeField] public GameObject _settingPanel;
    [SerializeField] public GameObject _gameOverPanel;
    [SerializeField] public GameObject _panel;
    [SerializeField] public Button _stopButton; // when start game again we have time 0.5f and user can click setting button in this time.

    [SerializeField] public GameObject _levelScreen;
    [SerializeField] public GameObject _helpScreen;
    [SerializeField] public GameObject _settingScreen;
    [SerializeField] public GameObject _collectedScreen;
    [SerializeField] public GameObject _shopSmallScreen;
    [SerializeField] public GameObject _buyHeartScreen;
    [SerializeField] public GameObject _failedScreen;
    [SerializeField] public GameObject _failedCoinScreen;
    [SerializeField] public TMP_Text _failedCoinText;    
    [SerializeField] public GameObject _exitLevelScreen;
    [SerializeField] public GameObject _exitLevelNormalScreen;
    [SerializeField] public GameObject _youWin;

    [SerializeField] public Canvas _canvas;
    [SerializeField] public Image _levelBackground;
  
    private GameObject instantiatedObj;
    private GameObject _instantiatedObj;

    [SerializeField] GameObject _newEggParticles;
    [SerializeField] GameObject _levelUpParticles;
    [SerializeField] GameObject _youWinParticles;
    public GameObject _snowParticles;
    [SerializeField] GameObject _collectedObjectSprite;
    [SerializeField] GameObject _collectedGiftSprite;
    [SerializeField] GameObject _collectedNewEgg;
    [SerializeField] GameObject _collectedNewItem;
   
    [SerializeField] Image _heartBar;
    [SerializeField] private AudioSource _newItemSound;
    [SerializeField] private AudioSource _levelUpSound;
    [SerializeField] private AudioSource _powerUpSound;


    [SerializeField] public bool _isGamePaused;
    [SerializeField] public bool _isnewlevel;
    [HideInInspector] private int _counterforGiveHeart = 0;
    public bool isPaused; // for only ad
    // Update is called once per frame
    void Update()
    {
        if (isPaused)
        {
            if (bool.Parse(PlayerPrefs.GetString("isRewardedClose")))
            {                
                PlayerPrefs.SetString("isRewardedClose", "False");
                //StartCoroutine(closeRewardedVideo());
                     
                //Get the method information using the method info class
                MethodInfo mi = this.GetType().GetMethod("openYouWin");

                //Invoke the method
                // (null- no parameter for the method call
                // or you can pass the array of parameters...)
                mi.Invoke(this, null);
                Time.timeScale = 0f;

                // first mehtod without time delay   
                //this.Invoke("Print", "A message to print (1)", 0f); // code-completes // ınvoke with parameters and time
                //Invoke("Print", "A message to print (2)", 1f); // doesn't code-complete but still works

                // second method
                //Invoke("openYouWin", 0.01f);
                return;
                
            }

            if (bool.Parse(PlayerPrefs.GetString("isNewLevel")))
            {                
                PlayerPrefs.SetString("isNewLevel", "False");
                //SceneManager.LoadScene("GameScene");
                
                //Get the method information using the method info class
                MethodInfo mi = this.GetType().GetMethod("uploadNewLevel");

                //Invoke the method
                // (null- no parameter for the method call
                // or you can pass the array of parameters...)
                mi.Invoke(this, null);
                Time.timeScale = 0f;
                
            }
        }
    }
    private void Awake()
    {        
        _gameManager = Object.FindObjectOfType<GameManager>();            
        _counterforGiveHeart = PlayerPrefs.GetInt("HeartBarCounter");

        checkSetttings();
    }
    public void Start()
    {       
        _collectedScreen.transform.localScale = Vector2.zero;
        _levelScreen.transform.localScale = Vector2.zero;
    }
    public void checkSetttings()
    {
        try
        {
            GameObject musicClass = UnityEngine.Object.FindObjectOfType<MusicClass>().gameObject;
            musicClass.GetComponent<AudioSource>().enabled = false;
        }
        catch { };
        

        bool soundBool = (PlayerPrefs.GetInt("SoundOn") == 1) ? true : false;
        if (soundBool) { _sound.isOn = true; _audioSource.mute = false; }
        else { _sound.isOn = false; _audioSource.mute = true; }

        switch(PlayerPrefs.GetInt("VolumeOn"))
        {
            case 1:
                {
                    _volume.isOn = true;
                    AudioListener.volume = 1;
                    break;
                }
            case 0:
                {
                    _volume.isOn = false;
                    AudioListener.volume = 0;
                    break;
                }
        }

        _heartBar.sprite = Resources.Load<Sprite>("HeartBar/HeartLine_" + _counterforGiveHeart);
    }
    public void updateHeartBar()
    {
        _counterforGiveHeart++;
        if (_counterforGiveHeart>=10)
        {
            _heartBar.sprite = Resources.Load<Sprite>("HeartBar/HeartLine_" + _counterforGiveHeart);
            _powerUpSound.GetComponent<AudioSource>().Play();
            _gameManager._heart++;
            PlayerPrefs.SetInt("Heart", _gameManager._heart);
            _gameManager._heartText.text = _gameManager._heart.ToString();
            _counterforGiveHeart = 0;            
        }
        PlayerPrefs.SetInt("HeartBarCounter", _counterforGiveHeart);
        _heartBar.sprite = Resources.Load<Sprite>("HeartBar/HeartLine_" + _counterforGiveHeart);
    }
    /*IEnumerator closeRewardedVideo()
    {
        Time.timeScale = 1f;
        yield return new WaitForSeconds(0.01f);
        //Get the method information using the method info class
        MethodInfo mi = this.GetType().GetMethod("openYouWin");

        //Invoke the method
        // (null- no parameter for the method call
        // or you can pass the array of parameters...)
        mi.Invoke(this, null);
    }*/
    public void volumeOnOff()
    {
        if (_gameManager.isGameReady)
        {
            _clickforVolumeandSound.Play();
        }
        if (_volume.isOn)
        {
            AudioListener.volume = 1;
            PlayerPrefs.SetInt("VolumeOn", 1);
        }
        else
        {
            AudioListener.volume = 0;
            PlayerPrefs.SetInt("VolumeOn", 0);
        }           
    }
    public void soundOnOff()
    {
        if (_gameManager.isGameReady)
        {
            _clickforVolumeandSound.Play();
        }
        if (_sound.isOn)
        {            
            _audioSource.mute = false;
            PlayerPrefs.SetInt("SoundOn", 1);

        }
        else
        {            
            _audioSource.mute = true;
            PlayerPrefs.SetInt("SoundOn", 0);
        }
    }


    public void openSettingMenu()
    {
        _audioSource.Stop();
        if (_isGamePaused == true) { return; } // for protect
        _stopButton.interactable = false;
        Time.timeScale = 0; 
        _isGamePaused = true;
        _settingPanel.SetActive(true);
        _settingScreen.SetActive(true);
        StartCoroutine(MenuDown(_settingScreen));
        //_gameManager.stopGame(true);
    }
    public void closeSettingMenu()
    {
        _audioSource.Play();
        // Time.timeScale = 1;
        _isGamePaused = false;        
        _settingPanel.SetActive(false);
        _helpScreen.SetActive(false);
        StartCoroutine(MenuUp(_settingScreen));
        //_gameManager.stopGame(false);
    }
    IEnumerator MenuDown(GameObject _object)
    {
        float posy = _object.GetComponent<RectTransform>().position.y;
        float posyStatic = _object.GetComponent<RectTransform>().position.y;        
        while (posy >= posyStatic - 1000f* _canvas.transform.localScale.x)
        {            
            yield return new WaitForSecondsRealtime(0.001f);
            _object.GetComponent<RectTransform>().position = new Vector2(_object.GetComponent<RectTransform>().position.x, posy);
            posy -= 50f;            
        }
        //Time.timeScale = 0;
    }
    IEnumerator MenuUp(GameObject _object)
    {
        float posy = _settingScreen.GetComponent<RectTransform>().position.y;
        float posyStatic = _settingScreen.GetComponent<RectTransform>().position.y;        
        while (posy <= posyStatic + 1000f * _canvas.transform.localScale.x)
        {
            yield return new WaitForSecondsRealtime(0.001f);
            _object.GetComponent<RectTransform>().position = new Vector2(_settingScreen.GetComponent<RectTransform>().position.x, posy);
            posy += 50f;
        }
        yield return new WaitForSecondsRealtime(0.004f);
        _object.SetActive(false);
        StartCoroutine(startGameAgain());
    }

    
    
    public void openHelp()
    {
        _helpScreen.transform.position = _settingScreen.transform.position;
        _helpScreen.SetActive(true);
        _settingScreen.SetActive(false);
    }
    public void closeHelp()
    {
        _helpScreen.SetActive(false);
        _settingScreen.SetActive(true); ;
    }
    public void mainMenu()
    {
        SceneManager.LoadScene("MainScene");
    }


    public void openlevelComplete()
    {
        _audioSource.Stop();
        _levelUpSound.Play();
        _levelScreen.SetActive(true);
        _levelScreen.LeanScale(Vector2.one, 0.8f).setIgnoreTimeScale(true);        
        Time.timeScale = 0f;
        _isGamePaused = true;
        isPaused = true;
        _gameOverPanel.SetActive(true);        
        instantiatedObj = Instantiate(_levelUpParticles, new Vector2(0, 0), Quaternion.identity);
        if(PlayerPrefs.GetInt("BasketCounter") > 0)
        {
            int count = PlayerPrefs.GetInt("BasketCounter") - 1;
            PlayerPrefs.SetInt("BasketCounter", count);
        }
    }
    public void nextLevel()
    {
        _levelUpSound.Stop();
        bool isAdReady = AdMobManager.InterstitialHazirMi();
        if (isAdReady)
        {
            AdMobManager.InsterstitialGoster();
        }
        else
        {
            AdMobManager.InterstitialReklamAl();
        }    
              
        //Destroy(instantiatedObj);
    }   
    
    public void uploadNewLevel()
    {        
        _levelScreen.LeanScale(Vector2.zero, 1f).setEaseInBack().setIgnoreTimeScale(true).setOnComplete(() =>
        {
            _levelScreen.SetActive(false);
            _isGamePaused = false;
            _gameOverPanel.SetActive(false);
            Time.timeScale = 1f;
            if (PlayerPrefs.GetInt("Heart") <= 0) { PlayerPrefs.SetInt("Heart", 1); } // for control.
            SceneManager.LoadScene("GameScene");               
        });
        
    }


    public void openCollectEggScreen(string _eggType, int _eggNumber)
    {
        //_newItemSound.Play();
        _stopButton.interactable = false;
        _collectedScreen.SetActive(true);
        _collectedNewEgg.SetActive(true);
        _collectedObjectSprite.SetActive(true);
        _collectedScreen.LeanScale(Vector2.one, 0.8f).setIgnoreTimeScale(true);

        Time.timeScale = 0f;
        
        _isGamePaused = true;        
        _panel.SetActive(true);        
        instantiatedObj = Instantiate(_newEggParticles,new Vector2(0,0), Quaternion.identity);       
        _collectedObjectSprite.GetComponent<Image>().sprite = Resources.Load<Sprite>("Eggs/" + _eggType + "/" + _eggType + " " + _eggNumber);        
        
    }
    public void closeCollectedScreen()
    {
        //_newItemSound.Stop();
        StartCoroutine(startGameAgain());
        
        _collectedScreen.LeanScale(Vector2.zero, 1f).setEaseInBack().setIgnoreTimeScale(true).setOnComplete(() =>
        {
            

            _collectedScreen.SetActive(false);
            _collectedNewEgg.SetActive(false);
            _collectedObjectSprite.SetActive(false);
            _collectedNewItem.SetActive(false);
            _collectedGiftSprite.SetActive(false);

        });  
        
        _isGamePaused = false;
        _panel.SetActive(false);        
        Destroy(instantiatedObj);        
    }
    public void openCollectGiftScreen(string _giftType)
    {
        //_newItemSound.Play();
        _stopButton.interactable = false;
        _collectedScreen.SetActive(true);
        _collectedNewItem.SetActive(true);
        _collectedGiftSprite.SetActive(true);

        _collectedScreen.LeanScale(Vector2.one, 0.8f).setIgnoreTimeScale(true);

        Time.timeScale = 0f;
        
        _isGamePaused = true;
        _panel.SetActive(true);
        instantiatedObj = Instantiate(_newEggParticles, new Vector2(0, 0), Quaternion.identity);
        // first method use recttransform change height or widht
        /*
        RectTransform rt = _collectedObjectSprite.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(170f, 170f);
        */
        _collectedGiftSprite.GetComponent<Image>().sprite = Resources.Load<Sprite>("Gift/" + _giftType );
    }
   


    public void openFailed()
    {
        //Time.timeScale = 0f;
        _audioSource.Stop();
        _isGamePaused = true;
        _gameOverPanel.SetActive(true);
        _failedScreen.SetActive(true);            
        StartCoroutine(closeFailed(_failedScreen));
    }
    IEnumerator closeFailed(GameObject _object)
    {        
        yield return new WaitForSecondsRealtime(2f);
        _object.SetActive(false);
        yield return new WaitForSecondsRealtime(0.1f);
        openbuyHeart();
    }
    public void openbuyHeart()
    {        
        _buyHeartScreen.SetActive(true);        
    }
    public void buyHeartUseCoin()
    {
        if (PlayerPrefs.GetInt("TotalCoin") > 100)
        {            
            int total = PlayerPrefs.GetInt("TotalCoin") - 100;
            PlayerPrefs.SetInt("TotalCoin", total);            
            //_coinText.text = total.ToString();
            int heart = PlayerPrefs.GetInt("Heart") + 5;
            PlayerPrefs.SetInt("Heart", heart);
            _gameManager._heart = heart;
            _gameManager._heartText.text = heart.ToString();

            _buyHeartScreen.SetActive(false);            
            openYouWin();
        }
        else
        {            
            _failedCoinText.text = PlayerPrefs.GetInt("TotalCoin").ToString();
            _buyHeartScreen.SetActive(false);
            openFailedCoin();
        }
    }
    public void openFailedCoin()
    {               
        _failedCoinScreen.SetActive(true);
        StartCoroutine(closeFailed(_failedCoinScreen));
    }    
    public void buyHeartUseAdMob()
    {
        //_isGamePaused = true;
        isPaused = true; // for admob
        bool isAdReady = AdMobManager.RewardedReklamHazirMi();
        if (isAdReady)
        {            
            AdMobManager.RewardedReklamGoster(RewardedAdHeart);
        }
        else
        {            
            AdMobManager.RewardedReklamAl();
        }
    }
    public void RewardedAdHeart(GoogleMobileAds.Api.Reward odul)
    {        
        int heart = PlayerPrefs.GetInt("Heart") + 5;
        PlayerPrefs.SetInt("Heart", heart);        
        _gameManager._heart = heart;
        _gameManager._heartText.text = heart.ToString();        
    }
    public void openYouWin()
    {                     
        _buyHeartScreen.SetActive(false);
        
        //Time.timeScale = 0f;
        _youWin.SetActive(true);
        //_panel.SetActive(true);
        //float scale = CameraControllerMain._orthoSize / 5f; // First camera size 5f.
        _instantiatedObj = Instantiate(_youWinParticles, new Vector2(0f, 4f), Quaternion.identity);
        //_instantiatedObj.transform.localScale = new Vector2(scale, scale);

    }
    public void Collect()
    {
        _isGamePaused = false;
        _audioSource.Play();
        isPaused = false; // for admob
        StartCoroutine(startGameAgain());        
        
        _youWin.SetActive(false);
        _gameOverPanel.SetActive(false);
        //_panel.SetActive(true);
        Destroy(_instantiatedObj);
        
    }
    IEnumerator startGameAgain()
    {
        _stopButton.interactable = false;
        yield return new WaitForSecondsRealtime(0.5f);        
        Time.timeScale = 1f;
        _gameManager._gameOver = false;
        _stopButton.interactable = true;
    }


    public void openExitLevelScreen()
    {
        Time.timeScale = 0f;
        _isGamePaused = true;
        _settingPanel.SetActive(true);
        _exitLevelScreen.SetActive(true);
        if (_settingScreen.activeSelf == true)
        {
            _settingScreen.SetActive(false);
        }
        
    }
    public void closeExitLevelScreen()
    {                
        _exitLevelScreen.SetActive(false);
    }
    public void yesButton()
    {
        _gameManager._heart--;
        PlayerPrefs.SetInt("Heart",_gameManager._heart);
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainScene");
    }
    public void noButton()
    {
        closeExitLevelScreen();
        //Time.timeScale = 0f;        
        _settingScreen.SetActive(true);
    }


    public void openExitLevelNormalScreen()
    {
        Time.timeScale = 0f;
        _isGamePaused = true;
        _gameOverPanel.SetActive(true);
        _exitLevelNormalScreen.SetActive(true);
        if (_buyHeartScreen.activeSelf == true)
        {
            _buyHeartScreen.SetActive(false);
        }
        if (_levelScreen.activeSelf == true)
        {
            _levelScreen.SetActive(false);
        }


    }
    public void closeExitLevelNormalScreen()
    {        
        _exitLevelNormalScreen.SetActive(false);
    }
    public void yesNormalButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainScene");
    }
    public void noNormalButton()
    {
        if (_gameManager._heart <= 0)
        {
            closeExitLevelNormalScreen();
            _buyHeartScreen.SetActive(true);
        }
        else
        {
            closeExitLevelNormalScreen();
            _levelScreen.SetActive(true);
        }
              
    }
    


}
