using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class TimePanel : MonoBehaviour
{
    public Text _time;
    public GlobalCanvas _pausePanel;

    Stopwatch _stopWatch;

    // Start is called before the first frame update
    void Start()
    {
        _stopWatch = new Stopwatch();
        _stopWatch.Start();
    }

    // Update is called once per frame
    void Update()
    {
        _stopWatch.Stop();

        // Get the elapsed time as a TimeSpan value
        TimeSpan ts = _stopWatch.Elapsed;

        // Format and display the TimeSpan value
        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
        ts.Hours, ts.Minutes, ts.Seconds,
        ts.Milliseconds / 10);
        _time.text = elapsedTime;

        if (!_pausePanel.GetGameIsPaused())
        {
            _stopWatch.Start();
        }
    }

    public TimeSpan GetTime()
    {
        return _stopWatch.Elapsed;
    }
}
