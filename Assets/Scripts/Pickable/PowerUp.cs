using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : Pickable
{
    private Player player;

    public PowerUp()
    {
        type = PickableType.PowerUp;
    }

    public override void Initialize()
    {
        
    }

    public void Initialize(Player player)
    {
        this.player = player;
    }

    protected override void TriggerEffect()
    {
        player?.PickPowerUp();
    }
}
