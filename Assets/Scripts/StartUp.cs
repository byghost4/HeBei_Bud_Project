using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartUp : MonoBehaviour
{
    public Transform leftTop;
    public Transform rightDown;
    public Image image;
    public Transform topUI;
    public Transform bottomUI;
    public Transform toggles;
    public GameObject 底座UI;
    public Transform bud;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        leftTop.DOLocalMove(new Vector3(-672, 226, 0), 1f);
        rightDown.DOLocalMove(new Vector3(642, -343, 0), 1f);
        yield return new WaitForSeconds(0.5f);
        image.DOColor(Color.clear, 1f).OnComplete(()=>
        {
            topUI.DOMove(new Vector3(0.01f, 0.86f, -0.138f), 1f);
            bottomUI.DOMove(new Vector3(0.57f, -0.63f, -0.138f), 1f).OnComplete(()=>
            {
                底座UI.SetActive(true);
                bud.DOMove(new Vector3(0.05f, 0.16f,0.139f),1f);
                bud.DOScale(1,1f).OnComplete(()=>
                {
                    //toggles.DOLocalMoveX(703, 0.5f);
                });
            });
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
