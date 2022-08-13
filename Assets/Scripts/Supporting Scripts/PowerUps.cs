using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    #region Variables
    //variables for modifiers
    public float speedModifier = 1.0f;
    public float healthModifier;
    public float maxHealthModifier = 1.0f;
    public float fireRateModifier;
    public float damageModifier;
    public bool isPermanent;

    //variables for duration
    public float duration; //holds the duration of the powerup
    public float durationReset; //holds what the duration needs to reset to
    #endregion

    #region Functions
    #region Custom Functions
    public void OnActivate(Pawn target) //modifies all stats by defined modifiers on active
    {
        target.moveSpeed *= speedModifier; //increases the movement speed stat
        target.shootCoolDownTime -= fireRateModifier; //decreases the time between shots
        target.tankDamage *= damageModifier; //increases the damage of each tank shot
        target.health.currentHealth *= healthModifier; //multiplies current health by modifier
        target.health.MaxHealth *= maxHealthModifier; //multiplies current health by modifier
    }

    public void OnDeactivate(Pawn target) //reverses the OnActive function
    {
        target.moveSpeed /= speedModifier; //resets the movement speed stat
        target.shootCoolDownTime += fireRateModifier; //resets the time between shots
        target.tankDamage /= damageModifier; //resets the damage of each tank shot
        target.health.currentHealth /= healthModifier; //resets current health by modifier
        target.health.MaxHealth /= maxHealthModifier; //resets current health by modifier
    }
    #endregion
    #endregion
}
