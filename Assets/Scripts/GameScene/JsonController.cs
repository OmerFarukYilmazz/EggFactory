using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class JsonController : MonoBehaviour
{
    //public EggsManager _eggManager = new EggsManager();
    [HideInInspector] public List<EggsManager> _eggtypeList = new List<EggsManager>();
    [HideInInspector] public List<LevelManager> _levelManager = new List<LevelManager>();
    [HideInInspector] public List<ItemManager> _itemmanager = new List<ItemManager>();
    //public LevelSystemManager _retryLevel = new LevelSystemManager();    
    //public string Jsonnn;

    //  C:\Users\omerf\AppData\LocalLow\DefaultCompany\EggFactory1 // old
    //  C:\Users\omerf\AppData\LocalLow\MorphyHeadGames\EggFactory1\Saves // new...
    //  C:\Users\omerf\AppData\LocalLow\MorphyHeadGames\EggStore\Saves // new...
    //  Debug.Log(Application.persistentDataPath); // for learning Application.persistentDataPath loc.
    public static string directory = "/Saves/";

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(Application.persistentDataPath);

    }
    
    public void JsonSave(List<EggsManager> eggtypeList)
    {        
        // 1. merthod -> to our computer
        /*string Jsonnn = JsonUtility.ToJson(_level);        
        File.WriteAllText(Application.dataPath + "Saves/UsersInfo.json", Jsonnn);*/

        // 2. merthod -> to our device
        string dir = Application.persistentDataPath + directory;
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        /*var JsonnRoad = JsonUtility.ToJson(eggtypeList);
        print(JsonnRoad);*/
        var JsonnRoad = JsonConvert.SerializeObject(eggtypeList);        
        File.WriteAllText(dir + "EggsCollection.json", JsonnRoad);

    }
    public void JsonLoad()
    {
        /*LevelSystemManager temp = new LevelSystemManager();
        temp = JsonUtility.FromJson<LevelSystemManager>(Jsonnn);
        for(int i = 0; i < arrayX.Length; i++)
        {
            Debug.Log(temp._arrayX[i]);
        }*/
        string JsonnRoad = (Application.persistentDataPath + directory + "EggsCollection.json");
        if (File.Exists(JsonnRoad))
        {
            /*string JsonnRead = File.ReadAllText(JsonnRoad);
            eggtypeList = JsonUtility.FromJson<List<EggsManager>>(JsonnRead);*/
            var JsonnRead = File.ReadAllText(JsonnRoad);
            _eggtypeList = JsonConvert.DeserializeObject<List<EggsManager>>(JsonnRead);
        }
        else
        {
            Debug.Log("error");
        }
    }

    public void JsonSaveOrginal(List<EggsManager> eggtypeList)
    {        
        string dir = Application.persistentDataPath + directory;
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }       
        var JsonnRoad = JsonConvert.SerializeObject(eggtypeList);
        File.WriteAllText(dir + "EggsCollectionOrginal.json", JsonnRoad);

    }
    public void JsonLoadOrginal()
    {       
        string JsonnRoad = (Application.persistentDataPath + directory + "EggsCollectionOrginal.json");
        if (File.Exists(JsonnRoad))
        {            
            var JsonnRead = File.ReadAllText(JsonnRoad);
            _eggtypeList = JsonConvert.DeserializeObject<List<EggsManager>>(JsonnRead);
        }
        else
        {
            Debug.Log("error");
        }
    }
    public void JsonSaveLevel(List<LevelManager> levelManager)
    {
        string dir = Application.persistentDataPath + directory;
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        /*var JsonnRoad = JsonUtility.ToJson(eggtypeList);
        print(JsonnRoad);*/
        var JsonnRoad = JsonConvert.SerializeObject(levelManager);
        File.WriteAllText(dir + "LevelManager.json", JsonnRoad);
    }
    public void JsonLoadLevel()
    {
        string JsonnRoad = (Application.persistentDataPath + directory + "LevelManager.json");
        if (File.Exists(JsonnRoad))
        {
            var JsonnRead = File.ReadAllText(JsonnRoad);
            _levelManager = JsonConvert.DeserializeObject<List<LevelManager>>(JsonnRead);
        }
        else
        {
            Debug.Log("error");
        }
    }
    public void JsonSaveItem(List<ItemManager> itemmanager)
    {
       
        string dir = Application.persistentDataPath + directory;
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }        
        /*var JsonnRoad = JsonUtility.ToJson(eggtypeList);
        print(JsonnRoad);*/
        var JsonnRoad = JsonConvert.SerializeObject(itemmanager);
        File.WriteAllText(dir + "ItemManager.json", JsonnRoad);
    }
    public void JsonLoadItem()
    {
        string JsonnRoad = (Application.persistentDataPath + directory + "ItemManager.json");        
        if (File.Exists(JsonnRoad))
        {
            var JsonnRead = File.ReadAllText(JsonnRoad);
            _itemmanager = JsonConvert.DeserializeObject<List<ItemManager>>(JsonnRead);
        }
        else
        {
            Debug.Log("error");
        }
    }

















    /*public void JsonSaveFirst(LevelSystemManager _level)
    {
        string dir = Application.persistentDataPath + directory;
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        string JsonnRoad = JsonUtility.ToJson(_level);        
        File.WriteAllText(dir + "UsersInfoFirst.json", JsonnRoad);
    }
    public void JsonLoadFirst()
    {       
        string JsonnRoad = (Application.persistentDataPath + directory +"UsersInfoFirst.json");
        if (File.Exists(JsonnRoad))
        {
            string JsonnRead = File.ReadAllText(JsonnRoad);
            _registeredLevel = JsonUtility.FromJson<LevelSystemManager>(JsonnRead);

        }
        else
        {
            Debug.Log("error");
        }
    }
    public void JsonSaveShape(LevelSystemManager _level)
    {
        string dir = Application.persistentDataPath + directory;
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        string JsonnRoad = JsonUtility.ToJson(_level);
        File.WriteAllText(dir + "UsersInfoShape.json", JsonnRoad);
    }
    public void JsonLoadShape()
    {
        string JsonnRoad = (Application.persistentDataPath + directory + "UsersInfoShape.json");
        if (File.Exists(JsonnRoad))
        {
            string JsonnRead = File.ReadAllText(JsonnRoad);
            _registeredLevel = JsonUtility.FromJson<LevelSystemManager>(JsonnRead);

        }
        else
        {
            Debug.Log("error");
        }
    }
    public void JsonSaveQuestion(LevelSystemManager _level)
    {
        string dir = Application.persistentDataPath + directory;
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        string JsonnRoad = JsonUtility.ToJson(_level);
        File.WriteAllText(dir + "UsersInfoQuestion.json", JsonnRoad);
    }
    public void JsonLoadQuestion()
    {
        string JsonnRoad = (Application.persistentDataPath + directory + "UsersInfoQuestion.json");
        if (File.Exists(JsonnRoad))
        {
            string JsonnRead = File.ReadAllText(JsonnRoad);
            _registeredLevel = JsonUtility.FromJson<LevelSystemManager>(JsonnRead);

        }
        else
        {
            Debug.Log("error");
        }
    }*/


}
