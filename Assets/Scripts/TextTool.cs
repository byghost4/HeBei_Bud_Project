using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextTool : MonoBehaviour
{
    private float targetValue;
    public float addValue;
    private float nowValue;
    private bool isAdd;
    private Text text;

    private void Start()
    {
        text = GetComponent<Text>();
    }

    private void Update()
    {
        if (isAdd)
        {
            nowValue += addValue;//Time.deltaTime;
            text.text = ((int)nowValue).ToString();
            if (nowValue >= targetValue)
            {
                isAdd = false;
                text.text = targetValue.ToString();
            }
        }
    }

    private void OnEnable()
    {
        targetValue = Random.Range(1000, 8000);
        isAdd = true;
    }

    private void OnDisable()
    {
        nowValue = 0;
        text.text = "0";
        isAdd = false;
    }
}
