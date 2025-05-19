using UnityEngine;

public class Coin : Pickable
{
    public Coin()
    {
        type = PickableType.Coin;
    }

    public override void Initialize()
    {
        
    }

    protected override void TriggerEffect()
    {
        SfxManager.Instance.PlayAudio("Coin");
    }
}
