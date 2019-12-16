using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvas : MonoBehaviour
{
    public static bool _gameIsPaused = false;

    // Panels
    public PausePanel _pausePanel;

    // Start is called before the first frame update
    void Start()
    {
        _pausePanel.ClosePanel();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ActivePausePanel();
        }
    }

    public void ActivePausePanel()
    {
        if (_gameIsPaused)
        {
            _pausePanel.OpenPanel();
            _gameIsPaused = false;
        }
        else
        {
            _pausePanel.ClosePanel();
            _gameIsPaused = true;
        }
    }
}
