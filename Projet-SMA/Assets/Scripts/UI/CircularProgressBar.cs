using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularProgressBar : ProgressBar
{
    public Color _highColor = Color.green;
    public Color _middleColor = Color.yellow;
    public Color _lowColor = Color.red;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public new void UpdateBar(float fillAmount)
    {
        base.UpdateBar(fillAmount);

        // Color manager
        if (_image.fillAmount <= 0.6)
        {
            _image.color = Color.Lerp(_lowColor, _middleColor, _image.fillAmount);
        }
        else
        {
            _image.color = Color.Lerp(_middleColor, _highColor, _image.fillAmount);
        }
    }
}
