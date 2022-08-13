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
            
        }
    }
    #endregion
    #endregion Functions
}

