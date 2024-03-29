using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickups : MonoBehaviour
{
    #region Variables
    public PowerUps powerup; //variable that holds the powerup of this pickup
    public Transform tf; //the pickups position in the gameworld
    public AudioSource pickupSound; //sound that plays when a pickup is picked up
    #endregion
    #region Functions
    #region Custom Functions
    // Start is called before the first frame update
    void Awake()
    {
        tf = GetComponent<Transform>(); //assigns the transform of the pickup
    }

    public void OnTriggerEnter(Collider other)
    {
        pickupSound.Play();
        PowerUpManager puMan = other.GetComponent<PowerUpManager>(); //stores the objects power up manager

        if (puMan != null)
        {
            puMan.Add(powerup);//Adds the powerup to the manager

        }
        GameManager.instance.currentPickUps--; //decrease the amount of powerups in the game
        Destroy(gameObject); //destroys this object
    }
    #endregion
    #endregion
}
