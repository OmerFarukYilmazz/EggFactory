using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ContentController : MonoBehaviour
{
    public RectTransform rectTransform;
    public ScrollRect _scrollRect;
    public GameObject _rabbitPanel;
    public Vector2 _CurrentlevelRabbitPos;
    public bool _israbbitPanelactive = true;
    public bool _contentPosUp; // scroll content up or down  - position of rabbit.
    

    [HideInInspector] public float _loopOrder; // which map are we on now and which loop. there is 5 map 
    [HideInInspector] public float _scrollSpeed= 0.01f;
    public int _itemNumber; // for map object
    public int _levelNum; // for level object
    public int _totalLevelMapLength = 30000; // tolal we have 5 map one map length 6000f.
    public float _levelOrderMapPos; // which map is the level on and which loop.
    public int _currentotalMapOrder; // total 5 map we have when we start again we finished one loop the first loop is name is 0.
    //GameManager _gameManager;
    MainSceneManager _mainSceneManager;
    InfiniteScroll _infinitiveScroll; 

    //public float _num;
    private int mapLength = 6000;

    // Start is called before the first frame update
    void Start()
    {
        //print(Mathf.Round(_levelOrderMapPos) + " " + Mathf.Round(_loopOrder));
    }
    // Update is called once per frame
    void Update()
    {
        _loopOrder =-this.transform.localPosition.y / mapLength;
        //print(this.transform.localPosition);
        
        if (this.transform.localPosition.y>0f)
        {            
            var currentPos = this.transform.localPosition;
            this.transform.localPosition = new Vector3(currentPos.x, 0.01f, currentPos.z);
            _scrollRect.StopMovement();    
        }

        if (_CurrentlevelRabbitPos.y < this.transform.localPosition.y - 1000f || _CurrentlevelRabbitPos.y > this.transform.localPosition.y + 1000f)
        {
            //print(_CurrentlevelRabbitPos.y + " " + (this.transform.localPosition.y - 1000f) + " " + (this.transform.localPosition.y + 1000f));
            if (_israbbitPanelactive == false)
            {
                _israbbitPanelactive = true;
                _rabbitPanel.SetActive(true);
                if (_CurrentlevelRabbitPos.y < this.transform.localPosition.y - 1000f)
                {
                    _contentPosUp = false;
                }
                else
                {
                    _contentPosUp = true;
                }
            }
            
        }
        else
        {
            if (_israbbitPanelactive == true)
            {
                _israbbitPanelactive = false;
                _rabbitPanel.SetActive(false);
            }
        }




        /*
        if(Mathf.Round(_levelOrderMapPos) == Mathf.Round(_loopOrder))
        {            
            if (_israbbitPanelactive == true)
            {
                _israbbitPanelactive = false;
                _rabbitPanel.SetActive(false);
            }
        }
        else
        {
            print(Mathf.Round(_levelOrderMapPos) + " " + Mathf.Round(_loopOrder));
            if (_israbbitPanelactive == false)
            {
                _israbbitPanelactive = true;
                _rabbitPanel.SetActive(true);
                if(Mathf.Round(_levelOrderMapPos) < Mathf.Round(_loopOrder))
                {
                    _contentPosUp = true;
                }
                else
                {
                    _contentPosUp = false;
                }
            }
        }
        */


    }
    private void Awake()
    {
        _mainSceneManager = Object.FindObjectOfType<MainSceneManager>();
        _infinitiveScroll = Object.FindObjectOfType<InfiniteScroll>();
        
    }
    public void setContentPos()
    {        
        _itemNumber = _mainSceneManager.checkItemNumber(PlayerPrefs.GetInt("Level")-1);
        _levelNum = (PlayerPrefs.GetInt("Level") - 1) % 200;
        _levelNum += 1;        
        Vector2 _temp = GameObject.Find("Canvas 1/VerticalScroll/Viewport/ScrollContent/Item " + _itemNumber + "/Level" + " " + "(" + _levelNum + ")").transform.position; //_mainSceneManager._rabbit.transform.position;
        _currentotalMapOrder = (PlayerPrefs.GetInt("Level") - 1) / 200;        
        _CurrentlevelRabbitPos = new Vector2(_temp.x , _currentotalMapOrder * (-_totalLevelMapLength) - _temp.y);
        this.transform.localPosition = new Vector2(transform.position.x, (_CurrentlevelRabbitPos.y));

        //print(((float)PlayerPrefs.GetInt("Level") / 40f));
        _levelOrderMapPos = Mathf.Round(((float)PlayerPrefs.GetInt("Level") / 40f));
        //print(_levelOrderMapPos);
        for (int i = 4; i < _levelOrderMapPos; i++) 
        {            
            _infinitiveScroll.HandleVerticalScroll();
        }
 
        /*while (this.transform.localPosition.y <= _temp.y)
        {            
        }*/

    }

    public void scrolltoRabbit()
    {        
        
        if(_contentPosUp == true)
        {
            _infinitiveScroll.positiveDrag = true;
            //_infinitiveScroll.HandleVerticalScroll();
            StartCoroutine(waitUpScroll());
            /*if (this.transform.localPosition.y < _CurrentlevelRabbitPos.y)
            {
                //_y = this.transform.localPosition.y + sc
                print(this.transform.localPosition.y);
                print(_CurrentlevelRabbitPos.y);               
                
                this.transform.localPosition = new Vector2(Quaternion.identity.x, this.transform.localPosition.y + 100f);
            }*/
        }
        else
        {
            _infinitiveScroll.positiveDrag = false;
            //_infinitiveScroll.HandleVerticalScroll();
            StartCoroutine(waitDownScroll());
        }
        
    }

    IEnumerator waitUpScroll()
    {
        while (this.transform.localPosition.y < _CurrentlevelRabbitPos.y)
        {
            if (this.transform.localPosition.y > 0f) { break; }    // for rotect from going below level 0.              
            if (_infinitiveScroll.positiveDrag == false) { _infinitiveScroll.positiveDrag = true; } // for protect scroll
            yield return new WaitForSeconds(0.01f);
            this.transform.localPosition = new Vector2(Quaternion.identity.x, this.transform.localPosition.y + 50f);
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {                
                break;
            }
            
        }

    }
    IEnumerator waitDownScroll()
    {
        while (this.transform.localPosition.y > _CurrentlevelRabbitPos.y)
        {
            
            if (_infinitiveScroll.positiveDrag == true) { _infinitiveScroll.positiveDrag = false; } // for protect scroll.
            yield return new WaitForSeconds(0.01f);
            this.transform.localPosition = new Vector2(Quaternion.identity.x, this.transform.localPosition.y - 50f);
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {                
                break;
            }
        }

    }
    

}
