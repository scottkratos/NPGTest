using UnityEngine;

public class SaveStation : InteractiveBase
{
    public override void Use(PlayerController player)
    {
        base.Use(player);
        SaveManager.instance.SaveCurrentProgress();
    }
}
