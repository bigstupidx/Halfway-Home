﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public int MainLevel = 1;

    public GameObject ConfirmationPanel;

	// Use this for initialization
	void Start ()
    {

        ConfirmationPanel.SetActive(false);

	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void LoadLevel()
    {
        SceneManager.LoadScene(MainLevel);
    }

    public void ConfirmQuit()
    {
        ConfirmationPanel.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }

}