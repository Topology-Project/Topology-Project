using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    public Text weaponName;
    public Text baseDamage;
    public Text criticalX;
    public Text rateOfFire;
    public Text magazine;
    public Text inscription1;
    public Text inscription2;

    Weapon weapon;
    
    // Start is called before the first frame update
    void Start()
    {
        weapon = GameManager.Instance.Player.weapon;
        weaponName.text = "파운드리 ";
    }

    // Update is called once per frame
    void Update()
    {
        weapon = GameManager.Instance.Player.weapon;
        weaponName.text = "파운드리 +" + weapon.Upgrade;
        baseDamage.text = weapon.BaseDamage.ToString();
        criticalX.text = weapon.CriticalX.ToString();
        rateOfFire.text = weapon.RateOfFire.ToString();
        magazine.text = weapon.Magazine.ToString("N0");

        SetInscriprion(inscription1, weapon.Inscriptions[0]);
        SetInscriprion(inscription2, weapon.Inscriptions[1]);
    }

    void SetInscriprion(Text text, Inscription.Data data)
    {
        switch(data.inscriptionType)
        {
            case InscriptionType.Normal:
            text.color = Color.green;
            break;
            case InscriptionType.Rare:
            text.color = Color.blue;
            break;
            case InscriptionType.Exclusive:
            text.color = Color.yellow;
            break;
            case InscriptionType.Gemini:
            text.color = Color.grey;
            break;
        }
        text.text = data.info;
    }
}
