using UnityEngine;
/// <summary>
/// Defines a contract between pickups and PlayerPickupManager classes
/// </summary>
public interface IPlayerEffect
{
    #region Interface
    void ActivateSpeedBoost(float duration, float speedMultiplier);
    #endregion
}
