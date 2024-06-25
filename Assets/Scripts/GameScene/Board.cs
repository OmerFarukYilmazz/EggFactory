using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public SpriteRenderer _spriteRenderer;
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
        _spriteRenderer = Object.FindObjectOfType<SpriteRenderer>();
    }
}
