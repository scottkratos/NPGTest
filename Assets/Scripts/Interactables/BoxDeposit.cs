using UnityEngine;

public class BoxDeposit : InteractiveBase
{
    public override void Use(PlayerController player)
    {
        base.Use(player);
        GameManager.instance.OpenBox();
    }
}
