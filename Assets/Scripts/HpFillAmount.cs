using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpFillAmount : MonoBehaviour
{
    public Image P_HP;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Bullet"))
        {
            GameObject parent = other.GetComponent<Bullet>().parent;
            if (!parent.tag.Equals(gameObject.tag))
            {
                GameManager.Instance.TriggerManager.OnTrigger(PlayTriggerType.EnemyHit);
                //P_HP.fillAmount = (float)healthPoint / (float)maxHealthPoint;
                Debug.Log("나 체력 준다");
                // if (healthPoint <= 0)
                {
                    GameManager.Instance.TriggerManager.OnTrigger(PlayTriggerType.EnemyDie);
                    Destroy(gameObject);
                }
            }
        }
    }
}
