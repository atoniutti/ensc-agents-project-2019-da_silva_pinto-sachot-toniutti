using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalCanvas : MonoBehaviour
{
    private static bool _gameIsPaused = false;
    private static bool _gameIsOver = false;

    public string _menuSceneName = "";

    // Panels
    public PausePanel _pausePanel;
    public GameOverPanel _gameOverPanel;

    // Cylinder
    public CylinderLevel _energyLevel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Pause Panel
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ActivePausePanel();
        }

        // Game Over
        if (_energyLevel._currentPercent == 0)
        {
            _gameOverPanel.GameOver();
            _gameIsOver = true;

            if (_gameIsOver && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
            {
                QuitGame();
            }
        }
    }

    public void ActivePausePanel()
    {
        if (_gameIsPaused)
        {
            _pausePanel.ClosePanel();
        }
        else
        {
            _pausePanel.OpenPanel();
        }
    }

    public bool GetGameIsPaused()
    {
        return _gameIsPaused;
    }

    public void SetGameIsPaused(bool isPaused)
    {
        _gameIsPaused = isPaused;
    }

    public void QuitGame()
    {
        if (_menuSceneName != "")
        {
            SceneManager.LoadScene(_menuSceneName, LoadSceneMode.Single);
        }
    }
}
