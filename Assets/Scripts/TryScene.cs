using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TryScene : MonoBehaviour
{
    bool isPaused;    
    public GameObject _obj;    
    // Start is called before the first frame update
    void Start()
    {
        AdMobManager.BannerGoster();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPaused)
        {
            if (bool.Parse(PlayerPrefs.GetString("isRewardedClose")))
            {
                PlayerPrefs.SetString("isRewardedClose", "False");
                _obj.SetActive(false);
            }
        }
    }

    public void buyHeartUseAdMob()
    {
        isPaused = true;       
        bool isAdReady = AdMobManager.RewardedReklamHazirMi();
        if (isAdReady)
        {
            AdMobManager.RewardedReklamGoster(RewardedAdHeart);
        }
        else
        {
            AdMobManager.RewardedReklamAl();
        }

    }
    public void RewardedAdHeart(GoogleMobileAds.Api.Reward odul)
    {
        print("sadsa");       

    }
}
