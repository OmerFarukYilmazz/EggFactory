using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameManager _gameManager;
    [SerializeField] Transform _targetTransform;
    [SerializeField] public Camera _mainCamera;
    [SerializeField] public Camera _animationCamera;
    Vector3 orignalPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        //new Vector3(_gameManager._boardCenter.x, _gameManager._boardCenter.y, transform.position.z);

        // I canceled two camera and stoppin game for timeline
        /*
        if (_gameManager._isAnimationPlaying == false)
        {
            _mainCamera.transform.position = new Vector3(0, 0, transform.position.z);
        }
        else
        {
            _mainCamera.transform.position = new Vector3(0, 0, transform.position.z);
            /*
            _animationCamera.gameObject.SetActive(true);
            _mainCamera.gameObject.SetActive(false);
            _animationCamera.transform.position = new Vector3(_targetTransform.position.x, _targetTransform.position.y+6,transform.position.z);   
            
        }
        */

    }
    private void Awake()
    {
        _gameManager = Object.FindObjectOfType<GameManager>();
        _mainCamera.transform.position = new Vector3(0, 0, transform.position.z);
        orignalPosition = _mainCamera.transform.position;
        cameraSet();        
    }
    public void cameraSet()
    {
        float orthoSize = ((float)_gameManager._width * (float)Screen.height / (float)Screen.width * 0.7f);        
        _mainCamera.orthographicSize = orthoSize;
    }
    public IEnumerator Shake(float duration, float magnitude)
    {       
        
        float elapsed = 0f;        
        while (elapsed < duration)
        {            
            float x = _mainCamera.transform.position.x + Random.Range(-0.5f, 0.5f) * magnitude;
            float y = _mainCamera.transform.position.y + Random.Range(-0.5f, 0.5f) * magnitude;
            _mainCamera.transform.position = new Vector3(x, y, -10f);
            elapsed += Time.realtimeSinceStartup;
            yield return 0;
        }
        _mainCamera.transform.position = orignalPosition;
        //print("shake");
        
    }
    
    public IEnumerator setCamPos(float duration)
    {
        yield return new WaitForSeconds(duration);
        _mainCamera.transform.position = orignalPosition;
        //print("set");
    }
}

