using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface CharacterInterface
{
    // 상태 수정기(StateModifier)를 반환하는 메서드
    public StateModifier GetModifier();

    // 지정된 탄약 종류에 대한 탄약 수를 반환하는 메서드
    public int GetAmmo(AmmunitionType ammunitionType);

    // 지정된 탄약 종류에 대한 탄약 수를 설정하는 메서드
    public void SetAmmo(AmmunitionType ammunitionType, int ammo);
}
