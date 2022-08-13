using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    #region Variables
    private Pawn pawn; //stores the pawn of whoever collects the powerup
    public List<PowerUps> powerups;
    #endregion
    // Start is called before the first frame update

    #region Functions
    #region Builtin Functions
    void Start()
    {
        pawn = GetComponent<Pawn>(); //sets the pawn at start
        powerups = new List<PowerUps>(); //sets a new list of PowerUps at start
    }

    // Update is called once per frame
    void Update()
    {
        List<PowerUps> expiredPowerUps = new List<PowerUps>(); //this list holds all of the expired powerups

        foreach (PowerUps power in powerups) 
        {
            power.duration -= Time.deltaTime; //decrements duration
            if (power.duration <= 0) 
            {
                expiredPowerUps.Add(power); //add to expired list
                power.duration = power.durationReset; //resets the duration for the next powerup
            }
        }
        foreach (PowerUps power in expiredPowerUps) 
        {
            power.OnDeactivate(pawn); //deactivates powerup
            powerups.Remove(power);
        }
    }
    #endregion
    #region Custom Functions
    public void Add(PowerUps powerup)
    {
        powerup.OnActivate(pawn); //run OnActivate of powerup

        if (!powerup.isPermanent) //adds to list if the powerup isnt permanent
        {
            powerups.Add(powerup);
        }
    }
    #endregion
    #endregion
}

