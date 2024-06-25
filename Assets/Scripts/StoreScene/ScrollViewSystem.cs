using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScrollViewSystem : MonoBehaviour
{
    public ScrollRect _scrollRect;
    [SerializeField] private ScrollButton _leftButton;
    [SerializeField] private ScrollButton _rightButton;
    [SerializeField] private ScrollButton _upButton;
    [SerializeField] private ScrollButton _downButton;
    [SerializeField] private float _scrollSpeed = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        //_scrollRect = GetComponent<ScrollRect>();
    }
    // Update is called once per frame
    void Update()
    {
        if (_leftButton != null)
        {
            if (_leftButton._isDown)
            {
                scrollLeft();              

            }
                

        }
        if (_rightButton != null)
        {
            if (_rightButton._isDown)
            {
                if (_scrollRect.horizontalNormalizedPosition <= 0f)
                {
                    _scrollRect.horizontalNormalizedPosition = 0.01f;
                }
                scrollRight();
            }
        }
        

    }

    private void scrollLeft()
    {
        if (_scrollRect != null)
        {
            if (_scrollRect.horizontalNormalizedPosition >= 0f)
            {
                _scrollRect.horizontalNormalizedPosition -= _scrollSpeed;
            }
        }
    }

    private void scrollRight()
    {
        if (_scrollRect != null)
        {
            if (_scrollRect.horizontalNormalizedPosition >= 0f && _scrollRect.horizontalNormalizedPosition <= 1f)
            {
                _scrollRect.horizontalNormalizedPosition += _scrollSpeed;
            }
        }
    }

    public void scrollUp()
    {
        print(_scrollRect.verticalNormalizedPosition);
        if (_scrollRect != null)
        {
            if (_scrollRect.verticalNormalizedPosition >= 0f)
            {
                _scrollRect.verticalNormalizedPosition -= 100f;
            } 
        }
    }

    public void scrollDown()
    {
        //print(_scrollRect.verticalNormalizedPosition);
        if (_scrollRect != null)
        {
            if (_scrollRect.verticalNormalizedPosition >= 0f && _scrollRect.verticalNormalizedPosition <= 1f)
            {
                
                _scrollRect.verticalNormalizedPosition += 100f;
            }
        }
    }
}
