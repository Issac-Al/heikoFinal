using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Net.Mime.MediaTypeNames;

public class GameManager : MonoBehaviour
{
    public GameState currentState;
    public GameEvent OnStartEvent, continueEvent, pauseEvent;
    public GameObject playerStats, pauseMenu;
    private bool paused = false;
    void Start()
    {
        currentState = GameState.ON_START;
        EvaluateState();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                continueEvent.Raise();
                ContinueGame();
                paused = false;
            }
            else
            {
                pauseEvent.Raise();
                PauseGame();
                paused = true;
            }
        }
    }

    public void EvaluateState()
    {
        switch (currentState)
        {
            case GameState.ON_START:
                OnStartEvent.Raise();
                break;
        }
    }

    public void PauseGame()
    {
        currentState = GameState.PAUSE;
        Time.timeScale = 0;
        playerStats.SetActive(false);
        pauseMenu.SetActive(true);
        EvaluateState();
    }
    public void ContinueGame()
    {
        currentState = GameState.PLAYING;
        Time.timeScale = 1;
        playerStats.SetActive(true);
        pauseMenu.SetActive(false);
        EvaluateState();
    }
    public void ExitGame()
    {
        currentState = GameState.GAME_OVER;
        EvaluateState();
        UnityEngine.Application.Quit();
    }
}

public enum GameState
{
    LOADING,
    ON_START,
    PLAYING,
    GAME_OVER,
    PAUSE
}