using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface CharacterInterface
{
    public StateModifier GetModifier();
    public int GetAmmo(AmmunitionType ammunitionType);
    public void SetAmmo(AmmunitionType ammunitionType, int ammo);
}
