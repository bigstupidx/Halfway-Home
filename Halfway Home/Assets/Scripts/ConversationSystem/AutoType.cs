﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class AutoType : MonoBehaviour
{
    // 1/(pause speed + player pref) = # of characters added to the screen per second.
    public float DefaultPauseSpeed = 0.2f; 
    public AudioClip sound;

    public List<AutoDelays> ExtraDelays;

    float letterPause;

    float PauseSpeedMultiplier = 1;

    string message;

    AudioSource audios;

    private TextMeshProUGUI Text;

    Dictionary<int, float> UpdateSpeed;

    Coroutine typing;

    float DefaultVolume;

    [HideInInspector]
    public bool Skipping;

    bool Paused;

    // Use this for initialization
    void Start()
    {
        Text = gameObject.GetComponent<TextMeshProUGUI>();
        audios = GetComponent<AudioSource>();
        Text.useMaxVisibleDescender = true;
        EventSystem.ConnectEvent<AutoTypeEvent>(gameObject, Events.AutoType, TypingText);
        EventSystem.ConnectEvent<DefaultEvent>(gameObject, Events.SkipTyping, SkipTyping);
        DefaultVolume = audios.volume;

        Space.Connect<DefaultEvent>(Events.Pause, OnPause);
        Space.Connect<DefaultEvent>(Events.UnPause, OnUnPause);

        Space.Connect<DefaultEvent>(Events.GetPlayerInfo, OnPause);
        Space.Connect<DefaultEvent>(Events.GetPlayerInfoFinished, OnUnPause);

    }


    public void Clear()
    {
        if(typing != null)
            StopCoroutine(typing);

        Text.text = "";
    }
    
    void OnPause(DefaultEvent eventdata)
    {
        Paused = true;
        if (typing != null)
            StopCoroutine(typing);
    }

    void OnUnPause(DefaultEvent eventdata)
    {
        Paused = false;
        if(Text.IsActive())
            typing = StartCoroutine(TypeText(Text.maxVisibleCharacters));
    }

    public void TypingText(AutoTypeEvent eventdata)
    {
    
        message = eventdata.text;
        Text.text = "";
        PauseSpeedMultiplier = Game.current.Progress.GetFloatValue("TextSpeed");
        letterPause = 1 / (DefaultPauseSpeed * PauseSpeedMultiplier);
        UpdateSpeed = TextParser.ExtractTextSpeed(ref message);
        if (!Paused)
            typing = StartCoroutine(TypeText(0));
        else
            Text.maxVisibleCharacters = 0;
    }

    public void SkipTyping(DefaultEvent eventdata)
    {
        StopCoroutine(typing);
        Text.text = message;
        Text.maxVisibleCharacters = message.Length;
        Space.DispatchEvent(Events.FinishedAutoType);
    }

    IEnumerator TypeText(int visible)
    {
        Text.maxVisibleCharacters = visible;
        Text.text = message;
        int Skip = 0;
        int length = message.Length;
        if(!Skipping)
        {
            while (Text.maxVisibleCharacters < message.Length)
            {

                if (Text.text[Text.maxVisibleCharacters] == '<')
                {
                    int i = Text.maxVisibleCharacters;
                    while (Text.text[i] != '>')
                    {
                        i += 1;
                    }
                    i -= Text.maxVisibleCharacters;
                    length -= i;
                    Skip = i;

                }


                //skip rich text stuff
                if (Text.text[Text.maxVisibleCharacters] == '<')
                {
                    //AudioOff = true;
                }
                if (Text.text[Text.maxVisibleCharacters] == '>')
                {
                    //AudioOff = false;
                }


                if (!audios.isPlaying)
                {
                    if (Text.maxVisibleCharacters < length && Skip == 0)
                    {
                        audios.volume = DefaultVolume * Game.current.Progress.GetFloatValue("SFXVolume")
                            * Game.current.Progress.GetFloatValue("MasterVolume");
                        audios.PlayOneShot(sound);
                    }
                    else
                    {
                        Skip -= 1;
                    }

                }

                if (UpdateSpeed.ContainsKey(Text.maxVisibleCharacters))
                {
                    letterPause = 1 / (UpdateSpeed[Text.maxVisibleCharacters] * PauseSpeedMultiplier);

                    //print("on " + letterPause + "with Speed: " + UpdateSpeed[Text.maxVisibleCharacters]);
                }

                var charaPause = DelayContains(message[Text.maxVisibleCharacters], letterPause);



                Text.maxVisibleCharacters += 1;

                yield return new WaitForSeconds(charaPause);
            }
        }
        else
        {
            Text.maxVisibleCharacters = message.Length;
            yield return new WaitForSeconds(letterPause);
        }

        

        Text.maxVisibleCharacters = message.Length;

        audios.Stop();
        Space.DispatchEvent(Events.FinishedAutoType);
        
    }


    float DelayContains(char character, float value)
    {
        foreach(var del in ExtraDelays)
        {
            if(del.chracter == character)
            {
                return value * del.DelayMultiplier;
            }

        }

        return value;
    }


}


/**
* CLASS NAME  : AutoTypeEvent
* DESCRIPTION : event data for the autotype function.
**/
public class AutoTypeEvent : EventData
{
    public string text;

    public AutoTypeEvent(string textToType)
    {
        text = textToType;
    }

}

[System.Serializable]
public struct AutoDelays
{
    public char chracter;
    public float DelayMultiplier;
}