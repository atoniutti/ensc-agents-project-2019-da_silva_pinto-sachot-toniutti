using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanel : MonoBehaviour
{
    // Canvas
    public GlobalCanvas _globalCanvas;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameOver()
    {
        gameObject.SetActive(true);
        _globalCanvas.SetGameIsPaused(true);
    }
}
