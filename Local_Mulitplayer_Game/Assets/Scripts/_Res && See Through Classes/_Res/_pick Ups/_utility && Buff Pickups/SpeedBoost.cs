using UnityEngine;

/// <summary>
/// grants temporary speed boost
/// </summary>
public class SpeedBoost : PickUpsBase
{
    #region Custom Variables

    [Header("Speed Boost Properties"), Space(10f)]

    [SerializeField] private float duration = 5f;
    [SerializeField] private float speedBoost = 10f;
    [SerializeField] private GameObject pickUpEffect;

    #endregion

    #region Overridden Methods

    protected override void ApplyEffect(GameObject player)
    {
        if(pickUpEffect)
        {
            Instantiate(pickUpEffect, transform.position, Quaternion.identity);
        }

        IPlayerEffect playerEffect = player.GetComponent<IPlayerEffect>();
        if (playerEffect!= null) playerEffect.ActivateSpeedBoost(duration, speedBoost);
        Destroy(gameObject, 0.01f);
    }

    #endregion
}
