﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapAccessTime : MonoBehaviour
{


    public List<AccessLocker> ClosedTimeContainer;
    public List<List<bool>> TimeClosed;
    

    public bool LimitedDailyAccess;

    public string AccessPoint;

    public int TimesCanVisit;

    Button self;

    // Use this for initialization
    void Start ()
    {

        TimeClosed = new List<List<bool>>();

        self = GetComponent<Button>();
        Space.Connect<DefaultEvent>(Events.ReturnToMap, CheckAccess);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}



    void CheckAccess(DefaultEvent Eventdata)
    {

        if(LimitedDailyAccess)
        {
            if(Game.current.Progress.GetIntValue(AccessPoint) < TimesCanVisit)
            {
                Game.current.Progress.SetValue<int>(AccessPoint, Game.current.Progress.GetIntValue(AccessPoint) + 1);
            }
            else
            {
                self.interactable = false;
                return;
            }
        }

        if(TimeClosed[Game.current.Day][Game.current.Hour])
        {
            self.interactable = false;
        }
        else
        {
            self.interactable = true;
        }

    }
}

[System.Serializable]
public class AccessLocker
{
    public int Day;
    public int starttime;
    public int endTime;
}