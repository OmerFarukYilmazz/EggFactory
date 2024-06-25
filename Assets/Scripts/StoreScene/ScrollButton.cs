using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScrollButton : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    public bool _isDown = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPointerDown(PointerEventData eventData)
    {        
        _isDown = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {        
        _isDown = false;
    }


}
