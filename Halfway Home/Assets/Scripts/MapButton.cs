﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapButton : MonoBehaviour
{

    public Room Location;
    
    public ColorBlock SelectedColors;

    ColorBlock DefaultColors;

    Button Body;

    [Range(1, 8)]
    public int TimeSpending = 1;

    public bool DrainFatigue = true;

	// Use this for initialization
	void Start ()
    {

        Body = GetComponent<Button>();
        DefaultColors = Body.colors;

        Space.Connect<MapEvent>(Events.MapChoiceMade, OnSelectedRoom);

        Space.Connect<DefaultEvent>(Events.LeaveMap, Reset);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void SelectRoom()
    {
        Space.DispatchEvent(Events.MapChoiceMade, new MapEvent(Location, TimeSpending, DrainFatigue));
    }

    void OnSelectedRoom(MapEvent eventdata)
    {
        var bloc = Body.colors;
        if(eventdata.Destination == Location && bloc != SelectedColors)
        {

            bloc = SelectedColors;

        }
        else
        {
            bloc = DefaultColors;
        }

        Body.colors = bloc;
    }

    void Reset(DefaultEvent eventdata)
    {
        Body.colors = DefaultColors;
    }

}
