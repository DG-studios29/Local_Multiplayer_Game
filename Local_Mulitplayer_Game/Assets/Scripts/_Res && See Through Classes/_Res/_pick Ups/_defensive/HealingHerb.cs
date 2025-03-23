using UnityEngine;
/// <summary>
/// heals the player
/// </summary>
public class HealingHerb : PickUpsBase
{
    #region Custom Variables

    [SerializeField] private float healthAmount;

    #endregion

    #region Overridden Methods

    protected override void ApplyEffect(GameObject player)
    {
        IPlayerEffect playerEffect = player.GetComponent<IPlayerEffect>();

        if (playerEffect != null) playerEffect.GiveHealth(healthAmount);
    }

    #endregion
}
