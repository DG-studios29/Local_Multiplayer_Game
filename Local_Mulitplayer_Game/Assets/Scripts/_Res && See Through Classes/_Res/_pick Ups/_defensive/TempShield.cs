using UnityEngine;
/// <summary>
/// grants the player a temporary shield.
/// </summary>
public class TempShield : PickUpsBase
{
    #region Custom Variables

    [SerializeField] private float shieldDuration;

    #endregion

    #region Overridden Methods

    protected override void ApplyEffect(GameObject player)
    {
        IPlayerEffect playerEffect = player.GetComponent<IPlayerEffect>();

        if (playerEffect != null) playerEffect.ActivateShield(shieldDuration);
    }

    #endregion
}
