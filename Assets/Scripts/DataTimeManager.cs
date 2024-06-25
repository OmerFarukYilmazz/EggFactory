using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DataTimeManager : MonoBehaviour
{
    DateTime currentDate;
    DateTime oldDate;
    TimeSpan difference;

    EntranceSceneManager _entranceSceneManager;


    private void Awake()
    {
        _entranceSceneManager = UnityEngine.Object.FindObjectOfType<EntranceSceneManager>();
    }

    void Start()
    {
        //Store the current time when it starts
        currentDate = System.DateTime.Now;

        //Grab the old time from the player prefs as a long
        long temp = Convert.ToInt64(PlayerPrefs.GetString("sysString"));

        //Convert the old time from binary to a DataTime variable
        oldDate = DateTime.FromBinary(temp);
        print("oldDate: " + oldDate);

        //Use the Subtract method and store the result as a timespan variable
        difference = currentDate.Subtract(oldDate);
        print("Difference: " + difference);

        int minutes = (int)difference.TotalMinutes;
        //print(minutes);

        _entranceSceneManager.checkDataTimeforGivingHeart(minutes);

    }
    // Update is called once per frame
    void Update()
    {

    }

    /*
    void OnApplicationPause()
    {
        //Savee the current system time as a string in the player prefs class
        PlayerPrefs.SetString("sysString", System.DateTime.Now.ToBinary().ToString());

        PlayerPrefs.SetString("ShowTimelineMain", "true");
        PlayerPrefs.SetString("ShowTimelineCollection", "true");
        PlayerPrefs.SetString("ShowTimelineEgg", "true");
        print("Saving this date to prefs: " + System.DateTime.Now);
    }
    */

    private void OnDisable()
    {

        //Savee the current system time as a string in the player prefs class
        PlayerPrefs.SetString("sysString", System.DateTime.Now.ToBinary().ToString());
        
        print("Saving this date to prefs: " + System.DateTime.Now);
    }





}
