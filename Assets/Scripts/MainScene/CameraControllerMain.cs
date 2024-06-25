using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerMain : MonoBehaviour
{
    public static float _orthoSize=0;
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
        Screen.orientation = ScreenOrientation.Portrait;
        cameraSet();
    }
    public void cameraSet()
    {
        float orthoSize = ((float)720f * (float)Screen.height / (float)Screen.width * 0.5f);   
        if(_orthoSize == 0)
        {            
            Camera.main.orthographicSize = orthoSize;
            _orthoSize = orthoSize;
        }
        else
        {
            Camera.main.orthographicSize = _orthoSize;
        }
        
    }
}
