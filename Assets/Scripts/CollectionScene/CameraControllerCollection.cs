using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerCollection : MonoBehaviour
{
    GameObject _eggCollection;
    [SerializeField] private GameObject _focusObject;
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
        cameraSet();
        Camera.main.transform.position = new Vector3(_focusObject.transform.position.x,_focusObject.transform.position.y,transform.position.z);
    }
    public void cameraSet()
    {
        float orthoSize =  1440f* (float)Screen.height / (float)Screen.width * 0.700f;
        Camera.main.orthographicSize = orthoSize;
    }
}
