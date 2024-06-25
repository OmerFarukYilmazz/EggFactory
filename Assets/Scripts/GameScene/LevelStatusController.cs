using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class LevelStatusController : MonoBehaviour
{
    JsonController _jsonController;
    public List<EggsManager> _eggCollection = new List<EggsManager>();
    [SerializeField] GameObject collectionBox;
    bool _active = false;
    // Start is called before the first frame update
    void Start()
    {
        try1();

    }
    private void Awake()
    {
        _jsonController = UnityEngine.Object.FindObjectOfType<JsonController>();

    }
    // Update is called once per frame
    void Update()
    {
        
    }
    void try1() 
    {
        collectionBox = GameObject.Find("sa");
        collectionBox.transform.position = new Vector2(10, 10);        
    }
    void checkLevelStatus()
    {
        foreach(var egg in _eggCollection)
        {            
            if (egg.HasEgg ==true)
            {
                string collectionBoxNum = egg.EggType;
                //int levelEggstatus = int.Parse(egg.EggType);
                collectionBox = GameObject.Find(collectionBoxNum);
                if(collectionBox.active == false)
                {
                    collectionBox.SetActive(true);
                    //collectionBox.GetComponent<Image>().sprite = Resources.Load<Sprite>("Eggs/" + type + "/" + type + " " + randomEggNum);
                }
            }
        }

    }

    public void loadEggCollection()
    {
        _jsonController.JsonLoad();
        _eggCollection = _jsonController._eggtypeList;
    }


}
