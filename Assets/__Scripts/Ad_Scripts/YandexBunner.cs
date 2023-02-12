using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YandexMobileAds;
using YandexMobileAds.Base;

public class YandexBunner : MonoBehaviour
{
    public Banner banner;

    private float startAd;

    void Start ()
        {
        RequestBanner();
        }

    private void Update ()
        {
        if (6f <= Time.time - startAd)
            {
            RequestBanner();
            }
        }
    private void RequestBanner ()
        {
        string adUnitId = "R-M-2135489-1";

        banner = new Banner(adUnitId, AdSize.BANNER_320x50, AdPosition.TopCenter);

        AdRequest request = new AdRequest.Builder().Build();

        banner.LoadAd(request);
        startAd = Time.time;
        }
    }
