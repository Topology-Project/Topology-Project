using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayUI : MonoBehaviour
{
    public Text BulletText;

    Player player;
    Weapon weapon;
    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.Instance.Player;
        weapon = player.weapon;
        int a = player.GetAmmo(weapon.AmmunitionType);
        int b = weapon.ResidualAmmunition;
        BulletText.text = b + " / " + a;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        weapon = player.weapon;
        int a = player.GetAmmo(weapon.AmmunitionType);
        int b = weapon.ResidualAmmunition;
        BulletText.text = b + " / " + a;
    }
}
