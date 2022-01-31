using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using GooglePlayGames;

public class LeaderBoard : MonoBehaviour
{
    public AudioClip audioClick;

    AudioSource audioButton;
    string LEADER_BOARD_ID = "CgkIxeXi2bwYEAIQAQ";
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
        PlayGamesPlatform.Activate();
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
