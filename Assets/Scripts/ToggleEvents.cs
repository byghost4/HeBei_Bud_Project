using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleEvents : MonoBehaviour
{

    [SerializeField]
    private Toggle 城市切换;
    private Vector3 orginPos;
    public Transform bud;
    private Vector3 orginBudPos;
    private Vector3 orginBudRot;
    public Transform targetPos;
    public List<Transform> greyTrans;
    [SerializeField]
    private Toggle 基站规划分布;
    [SerializeField]
    private Transform dataPara;
    private float transX;
    public Transform 基站;

    [SerializeField]
    private Toggle 用户数热力图;
    public Transform 热力图;

    [SerializeField]
    private Toggle 业务量热力图;
    public Transform 热力图1;

    [SerializeField]
    private Toggle 保障资源分布;

    [SerializeField]
    private Toggle 投诉分布;

    [SerializeField]
    private Toggle 告警分布;
    // Start is called before the first frame update
    void Start()
    {
        orginPos = Camera.main.transform.position;
        orginBudPos = bud.position;
        orginBudRot = bud.eulerAngles;
        //城市切换.onValueChanged.AddListener(ShowOne);
        基站规划分布.onValueChanged.AddListener(ShowTwo);
        用户数热力图.onValueChanged.AddListener(ShowThree);
        业务量热力图.onValueChanged.AddListener(ShowFour);
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 城市切换
    /// </summary>
    /// <param name="isOn"></param>
    public void ShowOne(bool isOn)
    {
        bud.GetComponent<Animator>().enabled = false;
        bud.DOMove(orginBudPos, 0.5f);
        bud.DORotate(orginBudRot, 0.5f);
        Camera.main.transform.DOMove(targetPos.position, 0.5f);
        Camera.main.transform.DORotate(targetPos.eulerAngles, 0.5f).OnComplete(()=> 
        {
            基站规划分布.isOn = true;
        });
        foreach (Transform item in greyTrans)
        {
            item.gameObject.SetActive(false);//.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", new Color32(0, 37, 63,255));
        }

    }

    /// <summary>
    /// 基站规划分布
    /// </summary>
    private void ShowTwo(bool isOn)
    {
        tween.Kill();
        StopCoroutine("ShowTwoSource");  
        transX = isOn ? 0 : -440;
        if (isOn)
        {
            foreach (Transform item in dataPara)
            {
                item.gameObject.SetActive(true);
            }
            foreach (Transform item in 基站)
            {
                item.DOScale(1f, 0.5f);
            }
        }
        else { 
            foreach (Transform item in 基站)
            {
                item.DOScale(0f, 0.5f);
            }
            //热力图.Play("热力图回");
        }
        StartCoroutine("ShowTwoSource");
    }

    private void ShowThree(bool isOn)
    {
        if (isOn)
        {
            foreach (Transform item in 热力图)
            {
                item.DOScale(0.003614091f, 0.5f);
            }
        }
            //热力图.Play("热力图");
        else
        {
            foreach (Transform item in 热力图)
            {
                item.DOScale(0f, 0.5f);
            }
            //热力图.Play("热力图回");
        }
    }

    private void ShowFour(bool isOn)
    {
        if (isOn)
        {
            foreach (Transform item in 热力图1)
            {
                item.DOScale(0.003614091f, 0.5f);
            }
        }
            //热力图1.Play("热力图1");
        else
        {
            foreach (Transform item in 热力图1)
            {
                item.DOScale(0, 0.5f);
            }
            //热力图1.Play("热力图回1");
        }
    }

    Tween tween;
    private IEnumerator ShowTwoSource()
    {
        foreach (Transform item in dataPara)
        {
            tween = item.DOLocalMoveX(transX, 0.5f).OnComplete(()=>
            {
                if(transX == -440)
                {
                    item.gameObject.SetActive(false);
                }
            });
            yield return new WaitForSeconds(0.1f);
        }
    }
}
