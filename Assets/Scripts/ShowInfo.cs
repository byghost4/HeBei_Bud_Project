using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShowInfo : MonoBehaviour
{
    public Transform m_UITrans;
    public Transform toggles;
    public UnityEvent unityEvent;
    public GameObject[] hideObj;
    private bool isShow = true;

    private void OnMouseOver()
    {
        if (!isShow)
            return;
        m_UITrans.gameObject.SetActive(true);
        m_UITrans.GetComponent<RectTransform>().anchoredPosition = Input.mousePosition;
        Utils.Highlighting(transform, Color.yellow);
    }

    private void OnMouseDown()
    {
        m_UITrans.gameObject.SetActive(false);
        foreach (var item in hideObj)
        {
            item.SetActive(false);
        }

        Utils.Highlighting(transform, Color.clear);
        toggles.DOLocalMoveX(703, 0.5f);
        unityEvent?.Invoke();
        isShow = false;
    }

    private void OnMouseExit()
    {
        if (!isShow)
            return;
        m_UITrans.gameObject.SetActive(false);
        Utils.Highlighting(transform, Color.clear);
    }
}
