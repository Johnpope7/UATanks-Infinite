using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    #region Variables
    //motor for all pawns
    public Health health; //holds the health class for the pawn
    public float healRate = 2; //how fast the pawns heal
    public TankMotor motor; //holds the movement handler, tank motor, for the pawn
    public int points; //decides how many points a pawn is worth
    protected bool isDead; //a boolean for death logic later
                           // Start is called before the first frame update
    [Header("Movement Properties")]
    public float moveSpeed; //movement speed of the tank
    public float rotateSpeed; //turn speed of the tank
    public float turretRotateSpeed; //determines the rotation speed of the turret
    public Transform turretTf; //holds the Turrets transform

    [Header("Tank Stats")]
    protected float shotForce = 20000f; //speed of the bullet shot
    public float p_shotForce  //gets the speed of the bullet
    {
        get { return shotForce; }
    }
    [SerializeField]
    protected float tankDamage = 25f; //damage the player tank does
    [SerializeField]
    protected float shootCoolDown; //the cooldown time between shoots, dont change this in the inspector
    [SerializeField]
    protected float shootCoolDownTime = 2f; //the time our cool down takes to refresh, adjust this to shoot faster or slower
    protected float noise = 0; //float for how much noise our pawn is making
    public float noiseRange = 10; //float for how far the noise our pawn is making travels
    public bool isMakingNoise = false; //bool for if pawn is making noise or not

    [Header("Tank Bullet Properties")]
    public GameObject bulletPrefab; //game object for the bullet instantiation 
    protected Rigidbody brb; //stores the bullet rigid body
    [SerializeField]
    protected Transform firingZone; //the spot from which the bullet comes from
    public float bulletLifeSpan; //decides how long the bullet has till its destroyed

    [Header("AI Settings")]
    public float withinWaypointRange; //close enough distance to waypoint
    public float viewRadius = 10; //for radius of player detection
    public float fieldOfView = 180f; //for Ai field of view
    public float hearingDistance = 10; //distance ai can hear
    public enum AIState { Chase, Attack, Flee, Rest, Patrol, LookAt, Idle } // an enumeration of AI States
    public AIState aiState = AIState.Patrol; //states for the AI, initial state is chase
    #endregion

    #region BuiltIn Methods
    void Start()
    {
        
    }
    #endregion
    // Update is called once per frame
    void Update()
    {
        Death();
    }
    #region Custom Methods
    public void Death() //the function that handles the death of all pawns
    {
        float ch = health.GetHealth();
        if (ch <= 0) 
        {
            GameManager.instance.UpdateScore(points); //wrties the new score to the game manager
            Destroy(gameObject); //destroys pawn
            isDead = true; //sets isDead variable which will be used later
        }
    
    }
    public virtual void Shoot(float _shotforce) //firing method
    {
        if (shootCoolDown <= 0)
        {
            //create the vector 3 variable that is equal to our firing zones forward vector multiplied by shot force
            Vector3 shotDir = firingZone.forward * shotForce;
            Debug.Log("shotDir is," + shotDir);
            //spawn the bullet
            GameObject bulletInstance = Instantiate(bulletPrefab, firingZone.position, firingZone.rotation);
            Debug.Log("bullet Instantiated");
            //get the instigator
            bulletInstance.GetComponent<Bullet>().instigator = this.gameObject;
            Debug.Log("gameObject assigned");
            //get the bulletDamage variable
            bulletInstance.GetComponent<Bullet>().SetBulletDamage(tankDamage);
            Debug.Log("tankDamage set," + tankDamage);
            //get the shell rigid body to apply force
            brb = bulletInstance.GetComponent<Rigidbody>();
            Debug.Log("Rigidbody set");
            //apply the shotforce variable to the rigid body to make the bullet move
            brb.AddForce(shotDir);
            Debug.Log("Added force to the power of " + shotDir);
            //destroy the bullet after a desired time
            Destroy(bulletInstance, bulletLifeSpan);
            Debug.Log("Bullet destroyed");
            shootCoolDown = shootCoolDownTime;
        }
    }
#endregion

}
