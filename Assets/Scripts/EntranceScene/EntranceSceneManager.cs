using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EntranceSceneManager : MonoBehaviour
{
    public string _playButton;
    public GameObject _gameHeader;
    public AudioSource _audioSource;   

    JsonController _jsonController;
    public List<ItemManager> _itemCollection = new List<ItemManager>();

    
    // Start is called before the first frame update
    void Start()
    {
        //printTime();
        //System.DateTime myTime = System.DateTime.Now;
        //Debug.Log(myTime.TimeOfDay);
        Screen.orientation = ScreenOrientation.Portrait;
        StartCoroutine(playAnimation());
    }
    void Awake()
    {        
        //GameObject.FindGameObjectWithTag("Music").GetComponent<MusicClass>().PlayMusic();        
        _jsonController = Object.FindObjectOfType<JsonController>();        

        checkSetttings();        

        if (PlayerPrefs.GetString("SaveCollectionFirstTime") == "")
        {
            saveItemCollection();
            //print("saveCollection");
            PlayerPrefs.SetString("SaveCollectionFirstTime", "true");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void checkDataTimeforGivingHeart(int min)
    {
        float minutes = min;
        int heart;
        if (minutes > 4320f)
        {
            PlayerPrefs.SetString("ShowTimelineMain", "true");
            PlayerPrefs.SetString("ShowTimelineCollection", "true");
            PlayerPrefs.SetString("ShowTimelineGame", "true");
        }

        if (PlayerPrefs.GetInt("Heart") >= 10)
        {
            return;
        }
        if (minutes > 100f)
        {
            heart = 10;
        }
        else
        {
            heart = Mathf.RoundToInt(minutes / 10); // give 1 hearth every 10 minutes.
        }

        heart = PlayerPrefs.GetInt("Heart") + heart;
        
        if (heart > 10)
        {
            heart = 10;
        }
        
        PlayerPrefs.SetInt("Heart", heart);
    }

    void printTime()
    {
        System.DateTime currentTime = System.DateTime.Now;
        
        int year = currentTime.Year;
        int month = currentTime.Month;
        int day = currentTime.Day;
        int hour = currentTime.Hour;
        int minute = currentTime.Minute;
        int second = currentTime.Second;

        print(minute);
    }

    public void checkSetttings()
    {
        if (PlayerPrefs.GetString("GameOpenFirstTime") == "")
        {
            PlayerPrefs.SetInt("VolumeOn", 1);
            PlayerPrefs.SetInt("SoundOn", 1);
        }

        bool soundBool = (PlayerPrefs.GetInt("SoundOn") == 1) ? true : false;
        if (soundBool)
        {            
            _audioSource.mute = false;
            GameObject.FindGameObjectWithTag("Music").GetComponent<MusicClass>().PlayMusic();
        }
        else
        {            
            _audioSource.mute = true;
            GameObject.FindGameObjectWithTag("Music").GetComponent<MusicClass>().StopMusic();
        }

        switch (PlayerPrefs.GetInt("VolumeOn"))
        {
            case 1:
                {                    
                    AudioListener.volume = 1;
                    break;
                }
            case 0:
                {                    
                    AudioListener.volume = 0;
                    break;
                }
        }
    }
    IEnumerator playAnimation()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        _gameHeader.LeanScale(Vector2.one, 0.8f).setIgnoreTimeScale(true);
    }

    public void saveItemCollection()
    {
        bool hasItem = false;
        string[] item = (System.Enum.GetNames(typeof(Item)));
        int totalItem = 0;
        bool isSingular = false;
        //int[] numbers = ((int[])System.Enum.GetValues(typeof(ItemNo)));
        for (int i = 0; i < item.Length; i++)
        {
            _itemCollection.Add(new ItemManager(item[i], hasItem, totalItem, isSingular, 0));

        }
        _jsonController.JsonSaveItem(_itemCollection);

    }


    public void playGame()
    {       
        if (PlayerPrefs.GetString("GameOpenFirstTime") == "")
        {
            PlayerPrefs.SetInt("Heart", 10);
            PlayerPrefs.SetInt("Level", 1);
            PlayerPrefs.SetInt("TotalScore", 0);
            PlayerPrefs.SetInt("TotalCoin", 0);
            //PlayerPrefs.SetInt("TotalEgg", 0);
            PlayerPrefs.SetFloat("Time", 60);
            PlayerPrefs.SetInt("BasketCounter", 0);
            PlayerPrefs.SetInt("HeartBarCounter", 0);
            PlayerPrefs.SetString("ShowTimelineMain", "true");
            PlayerPrefs.SetString("ShowTimelineCollection", "true");
            PlayerPrefs.SetString("ShowTimelineGame", "true");
            PlayerPrefs.SetInt("BossOrder", 0);

            PlayerPrefs.SetString("GameOpenFirstTime", "true");            
        }
             
        /*
        PlayerPrefs.SetString("_resumeBool", "false");        
        PlayerPrefs.SetString("_nextLevel", "false");   
        */
        SceneManager.LoadScene(_playButton);

        //PlayerPrefs.SetString("_retryBool", "false");
    }
    public void quitGame()
    {        
        Application.Quit();
    }    
    public void Resume()
    {
        if (PlayerPrefs.GetString("GameOpenFirstTime") == "")
        {
            PlayerPrefs.SetInt("Heart", 10);
            PlayerPrefs.SetInt("Level", 1);
            PlayerPrefs.SetInt("TotalScore", 0);
            PlayerPrefs.SetInt("TotalCoin", 0);
            //PlayerPrefs.SetInt("TotalEgg", 0);
            PlayerPrefs.SetFloat("Time", 60);
            PlayerPrefs.SetInt("BasketCounter", 0);
            PlayerPrefs.SetInt("HeartBarCounter", 0);
            PlayerPrefs.SetString("ShowTimelineMain", "true");
            PlayerPrefs.SetString("ShowTimelineCollection", "true");
            PlayerPrefs.SetString("ShowTimelineGame", "true");
            PlayerPrefs.SetInt("BossOrder", 0);

            PlayerPrefs.SetString("GameOpenFirstTime", "true");
        }
        
        /*
        PlayerPrefs.SetString("_nextLevel", "false");     
        PlayerPrefs.SetString("_resumeBool", "true");  
        */
        SceneManager.LoadScene(_playButton);

        //PlayerPrefs.SetString("_retryBool", "false");

        /*float resumeTime = PlayerPrefs.GetFloat("ResumeTime");
        PlayerPrefs.SetFloat("Time", resumeTime);*/
    }
    

}
