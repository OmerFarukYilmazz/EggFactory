using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Basket : MonoBehaviour , IPointerDownHandler , IPointerUpHandler //, IDragHandler , IEndDragHandler 
{
    GameManager _gameManager;
    StopMenuManager _stopMenuManager;
    Vector2 _startPos = new Vector2();
    [HideInInspector] public Animator _animator;
    bool _dragging ;
    // Start is called before the first frame update
    void Start()
    {
        //this.gameObject.GetComponent<SpriteRenderer>().sprite
        _startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_dragging || _stopMenuManager._isGamePaused)
        {
            return;
        }
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        checkDistane2();        
    }
    private void Awake()
    {
        _gameManager = Object.FindObjectOfType<GameManager>();
        _stopMenuManager = Object.FindObjectOfType<StopMenuManager>();
        _animator = GetComponent<Animator>();
        
    }

    


    public void OnPointerDown(PointerEventData eventData)
    {        
        _dragging = true;

    }
    public void OnPointerUp(PointerEventData eventData)
    {
        _dragging = false;
    }


    void checkDistane()
    {
        Vector2 temp = Input.mousePosition;        
        temp.x = Mathf.Clamp(temp.x, 0 , 720); // deger sinirlama fonk.
        Vector2 temp2 = Camera.main.WorldToScreenPoint(transform.position);
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(new Vector2 (temp.x, temp2.y));        
    }
    void checkDistane2()
    {
        Vector2 temp = transform.position;
        temp.x = Mathf.Clamp(temp.x,-2.8f,2.8f); // deger sinirlama fonk.
        temp.y = Mathf.Clamp(temp.y, _startPos.y, _startPos.y);
        transform.position = temp;
    }

    /*private void OnMouseDown()
    {
        print(transform.position);
        _dragging = true;
    }

    private void OnMouseUp()
    {
        _dragging = false;
    }*/
}
