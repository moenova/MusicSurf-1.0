﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Text.RegularExpressions;

public class GameController : MonoBehaviour
{
    public GameObject completeLevelUI;
    public Text upper_text;
    public Text lower_text;
    private Character char_scpt;
    public GameObject snow_balls_ui;
    public GameObject snow_balls_whole_ui;
    private void Start()
    {
        GameObject obj = GameObject.Find("Character");
        char_scpt = obj.GetComponent<Character>();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("MusicSurf");
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            if (Input.GetKeyDown(KeyCode.JoystickButton18))
            {
                PlayGame();
            }
            if (Input.GetKeyDown(KeyCode.JoystickButton19))
            {
                Quit();
            }
            if (Input.GetKeyDown(KeyCode.JoystickButton16))
            {
                SwitchToTutorial();
            }
        }

        if (SceneManager.GetActiveScene().name == "MusicSurf")
        {
            if (Input.GetKeyDown(KeyCode.JoystickButton18))
            {
                Restart();
            }
            if (Input.GetKeyDown(KeyCode.JoystickButton19))
            {
                SwitchToMenu();
            }
        }

        if (SceneManager.GetActiveScene().name == "Tutorial") 
        {
            if (Input.GetKeyDown(KeyCode.JoystickButton18))
            {
                SwitchToMenu();
            }
            if (Input.GetKeyDown(KeyCode.JoystickButton16))
            {
                SwitchToTutorial2();
            }
        }

        if (SceneManager.GetActiveScene().name == "Tutorial2") 
        {
            if (Input.GetKeyDown(KeyCode.JoystickButton18))
            {
                SwitchToMenu();
            }
            if (Input.GetKeyDown(KeyCode.JoystickButton16))
            {
                SwitchToTutorial3();
            }
            if (Input.GetKeyDown(KeyCode.JoystickButton17))
            {
                SwitchToTutorial();
            }
        }
        if (SceneManager.GetActiveScene().name == "Tutorial3") 
        {
            if (Input.GetKeyDown(KeyCode.JoystickButton18))
            {
                SwitchToMenu();
            }
            if (Input.GetKeyDown(KeyCode.JoystickButton17))
            {
                SwitchToTutorial2();
            }
        }

    }

    public void Quit()
    {
        Application.Quit();
    }

    public void SwitchToMenu()
    {
        Character.track = 0;
        Character.leveltime = 0;
        SceneManager.LoadScene("MainMenu");
    }

    public void Restart ()
	{
        completeLevelUI.SetActive(false);
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //Character.Restart();
    }

    // public void OnTriggerEnter()
    // {
    //     EndGame();
    // }
    public void EndGame (bool win)
	{
		if (win)
		{
            upper_text = completeLevelUI.transform.Find("Level").GetComponent<Text>();
            upper_text.text = "Home";
            lower_text = completeLevelUI.transform.Find("Complete").GetComponent<Text>();
            lower_text.text = "Arrived";
        } else {
            upper_text = completeLevelUI.transform.Find("Level").GetComponent<Text>();
            upper_text.text = "Lost";
            lower_text = completeLevelUI.transform.Find("Complete").GetComponent<Text>();
            lower_text.text = "Try Again";
            Character.start_time = Time.time;

            if (char_scpt.idx_hidx[0] > 148) {
                Character.track = 0;
                //level time is the for track music
                Character.leveltime = 0;

            } else if (char_scpt.idx_hidx[0] > 106){
                Character.track = 106;
                Character.leveltime = 46+59;
            } else if (char_scpt.idx_hidx[0] > 45){
                Character.track = 45;
                Character.leveltime = 46;
            } else {
                Character.track = 0;
                Character.leveltime = 0;
                //MusicController mc = GameObject.Find("MusicObject").GetComponent<MusicController>();
                //mc.asrc.time = 0;
            }
            MusicController mc = GameObject.Find("MusicObject").GetComponent<MusicController>();
            mc.asrc.Stop();
        }
        completeLevelUI.SetActive(true);
        snow_balls_ui.SetActive(false);
        snow_balls_whole_ui.SetActive(false);
        return;
	}

    public void SwitchToTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void SwitchToTutorial2()
    {
        SceneManager.LoadScene("Tutorial2");
    }
    public void SwitchToTutorial3()
    {
        
        SceneManager.LoadScene("Tutorial3");
    }
}
