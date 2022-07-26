using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    #region Variables
    public GameObject instigator; //stores the object that fires this bullet
    private Rigidbody brb; //stores the bullets rigidbody
    private float bulletDamage; //the damage value of the bullet



    #endregion

    private void Awake()
    {
        brb = GetComponent<Rigidbody>();
    }

    #region BuiltIn Method
    private void OnTriggerEnter(Collider _other)
    {
        GameObject enemyObject = _other.gameObject;
        Health enemyHealth = enemyObject.GetComponent<Health>();
        if (enemyHealth != null) //if the enemy has health, make it take damage
        {
            enemyHealth.Damage(bulletDamage); //take damage
        }
        brb.velocity = new Vector3(0f, 0f, 0f); //stops the projectile on impact
        Destroy(this.gameObject); //destroys the object after impact.
    }

    #endregion

    #region CustomMethods

    #region GettersAndSetters
    public float GetBulletDamage //the getter for the damage
    {
        get {return bulletDamage; }
    }
    public float SetBulletDamage(float dmg) //the setter of the damage
    {
        bulletDamage = dmg;
        return bulletDamage;
    }

    #endregion

    #endregion
}