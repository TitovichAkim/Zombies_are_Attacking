using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YandexMobileAds;
using YandexMobileAds.Base;

public class YandexRequestRewardedAd : MonoBehaviour
{
    public GameObject       playerGo;
    public Canvas           deathCanvas;



    private RewardedAd rewardedAd;

    private void Start ()
        {
        // Загружать рекламу сразу при старте сцены
        RequestRewardedAdForHealth();
        }

    private void RequestRewardedAdForHealth ()
        {
        string adUnitId = "R-M-2135489-2";

        rewardedAd = new RewardedAd(adUnitId);

        AdRequest request = new AdRequest.Builder().Build();

        rewardedAd.LoadAd(request);

        #region события

        rewardedAd.OnRewardedAdFailedToLoad += HandleRewardedAdFailedToLoad;        // Если не удалось загрузить рекламу
        rewardedAd.OnRewarded += HandleRewarded;                                    // Если вознагражден за просмотр рекламы
        
        #endregion события

        }

    public void ShowRewardedAdForHealth ()
        {
        if(this.rewardedAd.IsLoaded())
            {
            rewardedAd.Show();
            } else
            {
            Debug.Log("Rewarded Ad is not ready yet");
            }
        }

    public void HandleRewardedAdFailedToLoad (object sender, AdFailureEventArgs args)
        {
        AdRequest request = new AdRequest.Builder().Build();
        rewardedAd.LoadAd(request);
        }

    public void HandleRewarded (object sender, Reward args)
        {
        Time.timeScale = 1;
        playerGo.GetComponent<Player_Scr>().hp = 100;
        playerGo.GetComponent<Player_Scr>().adBool = true;
        deathCanvas.gameObject.SetActive(false);
        }
    }
