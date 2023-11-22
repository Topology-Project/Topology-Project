using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpFillAmount : MonoBehaviour
{
    public Image HP;
    public Image Pro;
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
                GameManager.Instance.TriggerManager.OnTrigger(PlayTriggerType.EnemyHit);
                Pro.fillAmount = (float)protectionPoint / (float)maxProtectionPoint;
                Debug.Log("보호막 줄어듬");
                if (protectionPoint <= 0)
                {
                    HP.fillAmount = (float)healthPoint / (float)maxHealthPoint;
                    Debug.Log("나 체력 준다");
                }
                if (healthPoint <= 0)
                {
                    GameManager.Instance.TriggerManager.OnTrigger(PlayTriggerType.EnemyDie);
                    Destroy(gameObject);
                }
            }
        }
    }
}
