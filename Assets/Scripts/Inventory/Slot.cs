using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class Slot : MonoBehaviour, IPointerEnterHandler
{
    public Image image;
    private Scroll.Data data;

    ScrollUI scrollUI;

    public void SetData(Scroll.Data data)
    {
        Debug.Log(data.sprite);
        this.data = data;
        image.sprite = data.sprite;
        image.gameObject.SetActive(true);
        if(data == null) image.gameObject.SetActive(false);
    }

    public Scroll.Data GetData()
    {
        return data;
    }

    public void SetScrollUI(ScrollUI scrollUI) => this.scrollUI = scrollUI;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!scrollUI.IsUnityNull()) scrollUI.SetUI(data);
    }
}
