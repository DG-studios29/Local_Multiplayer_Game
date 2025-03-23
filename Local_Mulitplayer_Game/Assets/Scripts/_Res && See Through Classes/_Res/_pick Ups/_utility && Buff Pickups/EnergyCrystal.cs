using UnityEngine;

/// <summary>
/// grants instant energy restoration for abilities
/// </summary>
public class EnergyCrystal : PickUpsBase
{
    #region Custom Variables

    [SerializeField] private float energyFillAmount;

    #endregion

    #region Overridden Methods

    protected override void ApplyEffect(GameObject player)
    {
        IPlayerEffect playerEffect = player.GetComponent<IPlayerEffect>();

        if (playerEffect != null) playerEffect.RefillAbilityBar(energyFillAmount);
    }

    #endregion
}
