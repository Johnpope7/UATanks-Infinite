using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTank : Pawn
{
    #region Variables
   
    #endregion

    #region Custom Methods
    public override void Shoot(float _shotforce) //firing method
    {
        base.Shoot(shotForce);
    }
    #endregion

    #region BuiltIn Methods
    // Start is called before the first frame update
    void Start()
    {
        motor = GetComponent<TankMotor>();
    }

    // Update is called once per frame
    void Update()
    {
        //subtract the time from the shot cool down clamped between 0 and its cool down time
        shootCoolDown = Mathf.Clamp(shootCoolDown - Time.deltaTime, 0, shootCoolDownTime);
    }

    //function for getting the cooldown because the method I used in my declarations wasn't working for cool down
    public float GetCoolDown() 
    {
        return shootCoolDown;
    }
    #endregion
}
