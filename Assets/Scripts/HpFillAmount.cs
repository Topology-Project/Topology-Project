using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpFillAmount : MonoBehaviour
{
    public Image HP;
    public Image HP_Back;
    public Image Pro;
    public Image Pro_Back;
    public float healthPoint { get; set; }
    public float maxHealthPoint { get; set; }
    public float protectionPoint { get; set; }
    public float maxProtectionPoint { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Bullet"))
        {
            GameObject parent = other.GetComponent<Bullet>().parent;
            if (!parent.tag.Equals(gameObject.tag))
            {
                Player player = GameManager.Instance.Player;
                Pro.fillAmount = (float)player.ProtectionPoint / (float)player.MaxProtectionPoint;
                Debug.Log("보호막 줄어듬");
                HP.fillAmount = (float)player.HealthPoint / (float)player.MaxHealthPoint;
                Debug.Log("나 체력 준다");
            }
        }
    }
}
