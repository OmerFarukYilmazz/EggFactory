using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelScreenManager : MonoBehaviour
{
    [SerializeField] TMP_Text _scoreText;
    [SerializeField] TMP_Text _coinText;
    [SerializeField] TMP_Text _statusText;
    [SerializeField] TMP_Text _levelText;
    //[SerializeField] TMP_Text _eggText; // for old menu
    [SerializeField] GameObject _star1empty;
    [SerializeField] GameObject _star2empty;
    [SerializeField] GameObject _star3empty;

    [SerializeField] GameObject _star1;
    [SerializeField] GameObject _star2;
    [SerializeField] GameObject _star3;
    //[SerializeField] Sprite _star; //  for old menu

    GameManager _gameManager;
    JsonController _jsonController;
    public List<LevelManager> _levelManager = new List<LevelManager>();
    bool _star1Bool, _star2Bool,_star3Bool;


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
        _jsonController = Object.FindObjectOfType<JsonController>();
    }

    public void updateLevelScreen()
    {
        _scoreText.text = _gameManager._scoreText.text;
        _coinText.text = _gameManager._coinText.text;
        _levelText.text = "LEVEL" + " " +_gameManager._level.ToString();
        //_eggText.text = _gameManager._numberofEggs.ToString(); // for old menu
        updateStar();
        if (_gameManager._level > 1)
        {
            loadLevel();
        }        
        saveLevel();
        updateLevel();
    }
    public void updateLevel()
    {
        if (PlayerPrefs.GetInt("CurrentLevel") > 0)
        {
            _gameManager._level++;
            PlayerPrefs.SetInt("CurrentLevel", _gameManager._level);
        }
        else
        {
            _gameManager._level++;
            PlayerPrefs.SetInt("Level", _gameManager._level);
        }
        
    }
    public void updateStar()
    {        
        float producedOfEgg = PlayerPrefs.GetFloat("Time") / _gameManager._respawnTime;
        switch (_gameManager._numberofBasketObject)
        {
            case int i when (i >= producedOfEgg * 0.75):
                /*_star1.gameObject.GetComponent<Image>().sprite = _star;
                _star2.gameObject.GetComponent<Image>().sprite = _star;
                _star3.gameObject.GetComponent<Image>().sprite = _star; */
                _star1.SetActive(true);
                _star2.SetActive(true);
                _star3.SetActive(true);
                _statusText.text = "PERFECT!";

                _star1Bool = true;
                _star2Bool = true;
                _star3Bool = true;
                break;
            case int i when (i > producedOfEgg * 0.50 && i < producedOfEgg * 0.75):
                /*_star1.gameObject.GetComponent<Image>().sprite = _star;
                _star2.gameObject.GetComponent<Image>().sprite = _star;*/
                _star1.SetActive(true);
                _star2.SetActive(true);
                _statusText.text = "WELL DONE!";

                _star1Bool = true;
                _star2Bool = true;
                break;
            case int i when (i <= producedOfEgg * 0.50):
                //_star1.gameObject.GetComponent<Image>().sprite = _star;
                _star1.SetActive(true);
                _statusText.text = "GOOD!";

                _star1Bool = true;                
                break;
        }
    }

    public void loadLevel()
    {
        _jsonController.JsonLoadLevel();
        _levelManager = _jsonController._levelManager;
        
    }


    public void saveLevel()
    {
        if (PlayerPrefs.GetInt("CurrentLevel") > 0)
        {
            _levelManager[PlayerPrefs.GetInt("CurrentLevel")-1].Star1 = _star1Bool;
            _levelManager[PlayerPrefs.GetInt("CurrentLevel")-1].Star2 = _star2Bool;
            _levelManager[PlayerPrefs.GetInt("CurrentLevel")-1].Star3 = _star3Bool;         
        }
        else
        {
            _levelManager.Add(new LevelManager(_gameManager._level, true, _star1Bool, _star2Bool, _star3Bool));
            
        }
        _jsonController.JsonSaveLevel(_levelManager);
    }
}
