using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class LeaderBoard : MonoBehaviour
{
    public AudioClip audioClick;

    AudioSource audioButton;
    string LEADER_BOARD_ID = "Cxxxxxxxxxxxxxxxxxxx";
    // Start is called before the first frame update
    void Start()
    {
        audioButton = GetComponent<AudioSource>();
        audioButton.clip = audioClick;

        EventTrigger eventTrigger = gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry_TouchUp = new EventTrigger.Entry();
        entry_TouchUp.eventID = EventTriggerType.PointerUp;
        entry_TouchUp.callback.AddListener((data) => { TouchUp(); });
        eventTrigger.triggers.Add(entry_TouchUp);

        // authenticate user:
        PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptOnce, (result) =>
        {
            // handle results
            Debug.Log("handle results : " + result);
        });

        /*
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
        // enables saving game progress.
        .EnableSavedGames()
        // requests the email address of the player be available.
        // Will bring up a prompt for consent.
        .RequestEmail()
        // requests a server auth code be generated so it can be passed to an
        //  associated back end server application and exchanged for an OAuth token.
        .RequestServerAuthCode(false)
        // requests an ID token be generated.  This OAuth token can be used to
        //  identify the player to other services such as Firebase.
        .RequestIdToken()
        .Build();
        PlayGamesPlatform.InitializeInstance(config);
        // recommended for debugging:
        PlayGamesPlatform.DebugLogEnabled = true;
        // Activate the Google Play Games platform
        */

        PlayGamesPlatform.Activate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void TouchUp()
    {
        RankButtonClick();
        if (SoundControl.bSoundOn) audioButton.Play();
        
    }

    public void RankButtonClick()
    {
        Social.localUser.Authenticate(AuthenticateHandler);
    }

    void AuthenticateHandler(bool isSuccess)
    {
        if (isSuccess)
        {
            float highScore = PlayerPrefs.GetFloat("BestTime", 0);
            Social.ReportScore((long) highScore, LEADER_BOARD_ID, (bool success) =>
            {
                if (success)
                {
                    PlayGamesPlatform.Instance.ShowLeaderboardUI(LEADER_BOARD_ID);
                    Debug.Log("Show Leader Board UI : " + success);
                }
                else
                {
                    Debug.Log("Show Leader Board UI : " + success);
                }

            });
        }
        else
        {
            // login failed
            Debug.Log("Login failed to Google Play Games : " + isSuccess);
        }
    }
}
