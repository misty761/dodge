using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using GoogleMobileAds.Api;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class GameManager : MonoBehaviour
{
    public GameObject gameoverText;
    public Text timeText;
    public Text recordText;
    public static float surviveTime;
    public static bool isGameover;

    private InterstitialAd interstitial;

    float timeAfterGameOver;

    private void Awake()
    {
        PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder().Build());
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }

    // Start is called before the first frame update
    void Start()
    {
        surviveTime = 0;
        isGameover = false;
        timeAfterGameOver = 0f;
        
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => { });
        RequestInterstitial();

        LogInPlayGames();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameover)
        {
            surviveTime += Time.deltaTime;
            timeText.text = "Time : " + (int) surviveTime;
            GetAchievement();
        }
        else
        {
            timeAfterGameOver += Time.deltaTime;
            //Debug.Log("timeAfterGameOver" + timeAfterGameOver);

            if (timeAfterGameOver > 2.5f)
            {
                // AdMob 광고 보기
                if (this.interstitial.IsLoaded())
                {
                    this.interstitial.Show();
                }

                // 최고 기록 불러오기
                float bestTime = PlayerPrefs.GetFloat("BestTime");

                // 최고 기록 갱신시
                if (surviveTime > bestTime)
                {
                    // 최고 기록 저장
                    bestTime = surviveTime;
                    PlayerPrefs.SetFloat("BestTime", bestTime);
                }

                // 최고 기록 표시
                recordText.text = "Best time : " + (int)bestTime;

                // 게임 오버 표시
                gameoverText.SetActive(true);

                // 키보드 R 눌르면 새 게임 시작
                if (Input.GetKeyDown(KeyCode.R))
                {
                    SceneManager.LoadScene("SampleScene");
                }

            }

            
        }  
    }

    public void LogInPlayGames()
    {
        //이미 인증된 사용자는 바로 로그인 성공됩니다.
        if (Social.localUser.authenticated)
        {
            Debug.Log(Social.localUser.userName);
            //txtLog.text = "name : " + Social.localUser.userName + "\n";
        }
        else
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    Debug.Log(Social.localUser.userName);
                    //txtLog.text = "name : " + Social.localUser.userName + "\n";
                }
                else
                {
                    Debug.Log("Login Fail");
                    //txtLog.text = "Login Fail\n";
                }
            });
    }

    public void GetAchievement()
    {
        if (surviveTime >= 10)
        {
            Social.ReportProgress(GPGSIds.achievement_beginner, 100.0f, (bool success) =>
            {
                if (success)
                {
                    //Debug.Log("Get Achievement");
                }
                else
                {
                    //Debug.Log("Fail to get Achievement");
                }
            });
        }

        if (surviveTime >= 30)
        {
            Social.ReportProgress(GPGSIds.achievement_gooddriver, 100.0f, (bool success) =>
            {
                if (success)
                {
                    //Debug.Log("Get Achievement");
                }
                else
                {
                    //Debug.Log("Fail to get Achievement");
                }
            });
        }

        if (surviveTime >= 60)
        {
            Social.ReportProgress(GPGSIds.achievement_bestdriver, 100.0f, (bool success) =>
            {
                if (success)
                {
                    //Debug.Log("Get Achievement");
                }
                else
                {
                    //Debug.Log("Fail to get Achievement");
                }
            });
        }

        if (surviveTime >= 120)
        {
            Social.ReportProgress(GPGSIds.achievement_crazydriver, 100.0f, (bool success) =>
            {
                if (success)
                {
                    //Debug.Log("Get Achievement");
                }
                else
                {
                    //Debug.Log("Fail to get Achievement");
                }
            });
        }

        if (surviveTime >= 180)
        {
            Social.ReportProgress(GPGSIds.achievement_drivingmachine, 100.0f, (bool success) =>
            {
                if (success)
                {
                    //Debug.Log("Get Achievement");
                }
                else
                {
                    //Debug.Log("Fail to get Achievement");
                }
            });
        }
    }

    public void EndGame()
    {
        isGameover = true;

        
    }

    private void RequestInterstitial()
    {
        #if UNITY_ANDROID
            string adUnitId = "ca-app-pub-3940256099942544/1033173712";
        #elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/4411468910";
        #else
            string adUnitId = "unexpected_platform";
        #endif

        // Initialize an InterstitialAd.
        this.interstitial = new InterstitialAd(adUnitId);

        // Called when an ad request has successfully loaded.
        this.interstitial.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is shown.
        this.interstitial.OnAdOpening += HandleOnAdOpened;
        // Called when the ad is closed.
        this.interstitial.OnAdClosed += HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        this.interstitial.OnAdLeavingApplication += HandleOnAdLeavingApplication;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitial.LoadAd(request);
    }

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: " + args.Message);
        interstitial.Destroy();
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
        interstitial.Destroy();
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
        interstitial.Destroy();
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeavingApplication event received");
        interstitial.Destroy();
    }
}
