using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponTemp : Weapon
{
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
        projectileSpeed.ResetState(10);
    }

    public override void Fire1(Transform transform)
    {
        // Debug.Log("Enemy Fire");
        // bullet인스턴스 생성 및 초기화 (임시)
        GameObject b = Instantiate(bullet, transform.position + Vector3.up, transform.rotation);
        b.GetComponent<Bullet>().Set(parent,
        character.GetModifier().GetState(StateType.ProjectileSpeed),
        character.GetModifier().GetState(StateType.Range));
    }
}
