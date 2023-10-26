using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammunition
{
    private AmmunitionType ammunitionType;
    public AmmunitionType AmmunitionType { get; set; }
    private int ammo;
    public int Ammo 
    {
        get
        {
            return ammo;
        }
        set
        {
            if(ammunitionType == AmmunitionType.Infinite) ammo = 99999;
            else ammo = value;
        }
    }

    public Ammunition(AmmunitionType ammunition, int ammo)
    {
        this.ammunitionType = ammunition;
        this.ammo = ammo;
    }
}
