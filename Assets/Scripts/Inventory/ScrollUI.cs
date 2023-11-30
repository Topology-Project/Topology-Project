using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollUI : MonoBehaviour
{
    public Image scrollImg;
    public Text scrollName;
    public Text type;
    public Text info;
    
    public void SetUI(Scroll.Data data)
    {
        scrollImg.sprite = data.sprite;
        scrollName.text = data.name;
        type.text = data.ScrollType.ToString();
        info.text = data.info;
    }
}
