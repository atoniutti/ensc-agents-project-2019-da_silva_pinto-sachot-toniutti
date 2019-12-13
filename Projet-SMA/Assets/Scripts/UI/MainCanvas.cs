using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvas : MonoBehaviour
{
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
        if (Input.GetKeyDown("space"))
        {
            ActivePausePanel();
        }
    }

    public void ActivePausePanel()
    {
        if (_pausePanel.isActiveAndEnabled)
        {
            _pausePanel.ClosePanel();
        }
        else
        {
            _pausePanel.OpenPanel();
        }
    }
}
