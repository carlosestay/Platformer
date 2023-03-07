using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    [SerializeField]
    Button exitButtont, restartButton, resumeButton;

    [SerializeField]
    GameObject panel, losePanel, winPanel;


    int menuState;



    void Start()
    {
        exitButtont.onClick.AddListener(Exit);
        restartButton.onClick.AddListener(Restart);
        resumeButton.onClick.AddListener(Resume);
        menuState = 0;
        panel.SetActive(false);
        losePanel.SetActive(false);
        winPanel.SetActive(false);
        EventManager.Instance.onGameEnd.AddListener(GameEnd);
        Time.timeScale = 1;
        EventManager.Instance.onLedActivator.Invoke(gameObject, new CustomEventArgs("P0\r"));
    }

    private void GameEnd(GameObject arg0, CustomEventArgs arg1)
    {
        if(!arg1.Successful)
        {
            panel.SetActive(true);
            losePanel.SetActive(true);
            Time.timeScale = 0;
            resumeButton.gameObject.SetActive(false);
        }
        else
        {
            panel.SetActive(true);
            winPanel.SetActive(true);
            Time.timeScale = 0;
            resumeButton.gameObject.SetActive(false);
        }
    }

    private void Resume()
    {
        panel.SetActive(false);
        menuState = 0;
        Time.timeScale = 1;
    }

    private void Restart()
    {
        SceneManager.LoadScene("Game");
    }

    private void Exit()
    {
        Application.Quit();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            menuState ^= 1;
            if(menuState == 1)
            {
                panel.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                panel.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }
}
