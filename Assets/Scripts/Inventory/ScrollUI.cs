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
        Color color;
        switch(data.ScrollType)
        {
            case ScrollType.Normal:
            color = Color.green;
            break;
            case ScrollType.Rare:
            color = Color.blue;
            break;
            case ScrollType.Legendary:
            color = Color.yellow;
            break;
            case ScrollType.Cursed:
            color = Color.grey;
            break;
            default:
            color = Color.white;
            break;
        }
        scrollImg.sprite = data.sprite;
        scrollName.text = data.name;
        scrollName.color = color;
        type.text = data.ScrollType.ToString();
        type.color = color;
        info.text = data.info;
    }
}
