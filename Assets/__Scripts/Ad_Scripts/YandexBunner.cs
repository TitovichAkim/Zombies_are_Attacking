using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YandexMobileAds;
using YandexMobileAds.Base;

public class YandexBunner:MonoBehaviour
{
    public Banner banner;


    void Start ()
    {
        RequestBanner();
    }
    private void RequestBanner ()
    {
        string adUnitId = "R-M-2135489-1";

        banner = new Banner(adUnitId, AdSize.BANNER_320x50, AdPosition.TopCenter);

        AdRequest request = new AdRequest.Builder().Build();

        banner.LoadAd(request);

        Invoke("RequestBanner", 6f);
    }
}
