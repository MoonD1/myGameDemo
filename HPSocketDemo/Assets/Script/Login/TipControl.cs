using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TipControl : MonoBehaviour
{
    public static TipControl Instance;
    public TMP_Text contentField;
    void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    public void ButtonClick()
    {
        gameObject.SetActive(false);
    }

    public void Show(string str)
    {
        contentField.text = str;
        gameObject.SetActive(true);
    }
    void Update()
    {
        
    }
}
