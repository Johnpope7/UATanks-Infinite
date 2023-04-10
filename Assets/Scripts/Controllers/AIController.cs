using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class AIController : Controller
{
    #region Variables
    [Header("AI Controller Components")]
    [SerializeField]
    public GameObject target; //stores the AI's target
    public Transform targetTf; //stores the transform of the AI's target
    public Health eHealth; //stores the Health script of the enemy
    protected Transform tf; //stores the transform of the AI pawn
    public LayerMask playerLayer; //allows me to access the player layer;
    GameObject newTarget; //stores the object of the new target
    Transform newTargetTf; //stores the transform of the new target

    [Header("Enemy Type"), SerializeField]
    protected EnemyType enemyType; //the type of enemy the AI controller is interfacing with (Melee or Ranged)
    protected enum EnemyType { Inky, Pinky, Blinky, Clyde, Test } //an enumeration of enemy types
    [SerializeField, Range(0, 10)]
    protected float timer = 3f; //timer for the melee state change coroutine

    [Header("Navigation Variables")]
    [SerializeField]
    protected WapointManager wm;
    Vector3 movementVec; //stores the vector3 of the movement
    Vector3 randomVec; //stores the random vector3 generated for wandering
    Quaternion rotationQuat; //stores the quaternion of the rotation
    Quaternion randomQuat; //stores the randomly generated quaternion
    protected float movement; //stores the movement the enemy will use
    protected float rotation; //stores the rotation the enemy will use
    public int avoidanceStage = 0;
    public float avoidanceTime = 2.0f;
    protected float exitTime = 0.5f; //variable that stores Move exit times
    public float stateEnterTime; //variable to store the time a state was entered
    public float stateExitTime = 30.0f; //float that determines state exiting time
    public float restingHealthRate; //sets the heal rate in hp per second
    public float fleeDistance = 1.0f;
    public int currentWaypoint = 0; //stores ai's current value in waypoint index
    public float withinWaypointRange = 1.0f; //value for if our ai is close enough to its waypoint
    public float stopDistance; //to stop AI before they enter the players space
    protected float fireRadius = 4f; //gives the radius at which the enemy can turn and fire
    protected float currentAngle; // stores the current angle of the tank cannon
    protected float fireAngle; //stores the firing angle of the cannon
    protected float distanceToTarget; //stores the distance to the target
    protected float angleModifier = 1; //changes the range of the Wander radius
    protected bool isWandering = false; //stores if the AI is wandering or not
    protected float wanderingSpeed = 10; //speed of wander
    Vector3? direction = null; //random direction for wandering
    protected WapointManager _wm;
    #endregion

    #region Builtin functions
    void Start()
    {

    }


    void Update()
    {

    }
    #endregion

    #region Custom Functions

    #region Senses
    public bool CanMove(float speed) //decides if the tank can move or not
    {

        if (Physics.Raycast(tf.position, tf.forward, out RaycastHit hit, speed)) //raycasts off the transform forward and checks to see if we hit the player
        {

            if (!hit.collider.CompareTag("Player")) //if our raycast doesnt hit a player
            {
                return false; //move returns false
            }
        }
        return true; //if it is a player than it returns true
    }

    public bool See(GameObject player) //allows the AI to "see" the player
    {

        Vector3 agentToPlayerVector = player.transform.position - ePawn.transform.position; //stores the distance between the enemy and the target player

        float angleToPlayer = Vector3.Angle(agentToPlayerVector, ePawn.transform.right);//finds the angle from our enemies view by using the distance between the player and the enemies transform 


        if (angleToPlayer < ePawn.fieldOfView) //does this if our angle to our player is lest that our field of view
        {
            if (Vector3.Distance(ePawn.transform.position, player.transform.position) < ePawn.viewRadius / 2)
            {
                //checks to see if we hit thje player target
                if (Physics.Raycast(ePawn.transform.position, agentToPlayerVector, out RaycastHit hit, ePawn.viewRadius, playerLayer))
                {
                    // If our raycast hits our player target
                    if (hit.collider.gameObject == player)
                    {
                        newTarget = GameManager.instance.playertarget;
                        newTargetTf = GameManager.instance.playertarget.transform;
                        SetTarget(newTarget, newTargetTf);
                        // return true 
                        return true;
                    }
                    else
                    {
                        return false; //if its not our target then return false
                    }
                }
            }
        }
        return false;
    }
    #endregion

    #region AI States

    public void Avoidance()
    {
        //need to find out a good way for avoidance
    }

    protected void Attack()
    {
        //fire your weapon
        base.ePawn.Shoot(base.ePawn.p_shotForce);
    }

    protected void Chase(Transform targetTf)
    {
        GetMoveRotateFloats();
        motor.Rotation(rotation);
        motor.Move(movement);

    }

    protected void Flee()
    {
        GetMoveRotateFloats();
        motor.Rotation(-rotation);
        motor.Move(-movement);
    }
    protected void Patrol()
    {
        //set target equal to current waypoint
        newTarget = wm.waypoints[currentWaypoint].transform.gameObject;
        //set target transform equal to current waypoint transform
        newTargetTf = wm.waypoints[currentWaypoint].transform;
        SetTarget(newTarget, newTargetTf);
        //if close to waypoint
        Vector3 delta = wm.waypoints[currentWaypoint].position - tf.position;
        delta.y = 0;

        if (motor.RotateTowards(wm.waypoints[currentWaypoint].position, base.ePawn.rotateSpeed))
        {
            //Does nothing
            Debug.Log("Target is ", target);
        }
        else
        {
            //Move forward
            GetMoveRotateFloats();
            motor.Rotation(rotation);
            motor.Move(movement);
        }

        if (delta.sqrMagnitude < withinWaypointRange)
        {
            //and if the waypoint index hasn't been completed
            if (currentWaypoint < wm.waypoints.Count - 1)
            {
                //move to the next patrol waypoint
                currentWaypoint++;
            }

            else
            {
                //if it has reset index
                currentWaypoint = 0;
            }
        }
    }
    protected void Rest()
    {
        Health health = GetComponent<Health>();
        health.Heal(base.ePawn.healRate * Time.deltaTime); //heals the healRate every second
    }

    protected void Idle()
    {
        Debug.Log("Idling");
    }
    #endregion

    protected float GetDistanceToTarget()
    {
        float distanceToTarget = Vector3.Distance(targetTf.position, base.ePawn.transform.position); //finds the distance to the target
        return distanceToTarget; //returns it
    }

    protected float GetAngleToTarget()
    {
        //get our target direction by subtracting our position from our target's position
        Vector3 targetDir = targetTf.position - base.ePawn.transform.position;
        //get the angle in degrees between our target direction and our forward transform
        float angleToTarget = Vector3.Angle(targetDir, base.ePawn.transform.forward);
        //return angle
        return angleToTarget;
    }

    public void ChangeState(Pawn.AIState newState, Pawn pawn)
    {
        pawn.aiState = newState; //changes the state
    }

    public void SetTarget(GameObject newTarget, Transform newTargetTf)
    {
        target = newTarget; //sets the target equal to the new target
        targetTf = newTargetTf; //sets the target transform equal to the new target transform
    }

    public void GetMoveRotateFloats()
    {
        movementVec = (targetTf.position - ePawn.transform.position) * ePawn.moveSpeed;
        movement = movementVec.z;
        rotationQuat = Quaternion.LookRotation(targetTf.position - ePawn.transform.position, Vector3.up);
        rotation = rotationQuat.y;
    }


    IEnumerator Wander()
    {
        RotateAgent();
        yield return new WaitForSeconds(2);
        StartCoroutine(LookAround());
    }

    protected void RotateAgent()
    {
        float rotateOrientation = Random.Range(-30f, 30f) * angleModifier; //random number * -30 degrees and 30 degrees times the angleModifier
        randomQuat = Quaternion.AngleAxis(rotateOrientation, Vector3.up); //determines the new rotation the enemy is facing
        rotation = randomQuat.y;
    }

    protected void MoveAgent()
    {
        //TODO; Make a random Vector3 Generator that is focused around the enemy

        float moveRange = Random.Range(5f, 20f); //random number for moving forwards
        randomVec = new Vector3(moveRange, ePawn.transform.position.z); //determines a vector for the new movement range
        movement = randomVec.z;
    }

    IEnumerator LookAround()
    {
        direction = null;
        motor.Move(0f);
        RotateAgent();
        yield return new WaitForSeconds(3);
        isWandering = false;
    }
    public WapointManager GetWaypointManager()
    {
        return _wm;
    }
    public void SetWaypointManager(WapointManager wm)
    {
        _wm = wm;
    }
    #endregion
}


