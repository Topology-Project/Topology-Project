using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Image image;
    private Scroll.Data data;

    public void SetData(Scroll.Data data)
    {
        Debug.Log(data.sprite);
        this.data = data;
        image.sprite = data.sprite;
        image.gameObject.SetActive(true);
        if(data == null) image.gameObject.SetActive(false);
    }
}
