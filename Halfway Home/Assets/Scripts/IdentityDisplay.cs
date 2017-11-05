﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IdentityDisplay : MonoBehaviour
{
    public GameObject ChoiceBox;
    public GameObject ConfirmBox;
    public TMP_InputField Name;
    public TextMeshProUGUI Pronouns;

    string genderPicked = "N";
    string namePicked = "Sam";

	// Use this for initialization
	void Start ()
    {

        //ChoiceBox.SetActive(false);
        ConfirmBox.SetActive(false);
        
        Space.Connect<DefaultEvent>(Events.GetPlayerInfo, OnGetPlayerInfo);
        gameObject.SetActive(false);
  }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void AssignGender(string gender)
    {
        genderPicked = gender;

        if (genderPicked == "N")
            Pronouns.text = "They/Them";
        if (genderPicked == "M")
            Pronouns.text = "He/Him";
        if (genderPicked == "F")
            Pronouns.text = "She/Her";
    }

    public void AssignName()
    {
        if (Name.text == "")
            namePicked = "Sam";
        else
            namePicked = Name.text;
    }

    public void ConfirmIdentity()
    {
        ConfirmBox.SetActive(true);
    }

    public void SetIdentity()
    {
        print(namePicked);
        Game.current.PlayerName = namePicked;
        Game.current.Progress.SetValue<string>("PlayerName", Game.current.PlayerName);
        Game.current.Progress.SetValue<string>("PlayerGender", genderPicked);

        ChoiceBox.SetActive(false);
        ConfirmBox.SetActive(false);

        print("my name is " + Game.current.Progress.GetStringValue("PlayerName"));
        Space.DispatchEvent(Events.GetPlayerInfoFinished);
        gameObject.SetActive(false);
    }

    public void OnGetPlayerInfo(DefaultEvent e)
    {
        print("Ooooon");
        gameObject.SetActive(true);
        ConfirmBox.SetActive(false);
    }
}
