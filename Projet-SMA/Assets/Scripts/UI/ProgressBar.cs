using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Image _image;
    public Text _text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateBar(float fillAmount)
    {
        _image.fillAmount = fillAmount / 100;
        _text.text = fillAmount + "%";
    }
}
