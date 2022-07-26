using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class AIController : Controller
{
    #region Variables
    [Header("AI Controller Components")]
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
    Vector3 movementVec; //stores the vector3 of the movement
    Vector3 randomVec; //stores the random vector3 generated for wandering
    Quaternion rotationQuat; //stores the quaternion of the rotation
    Quaternion randomQuat; //stores the randomly generated quaternion
    private float movement; //stores the movement the enemy will use
    private float rotation; //stores the rotation the enemy will use
    public int avoidanceStage = 0;
    public float avoidanceTime = 2.0f;
    private float exitTime = 0.5f; //variable that stores Move exit times
    public float stateEnterTime; //variable to store the time a state was entered
    public float stateExitTime = 30.0f; //float that determines state exiting time
    public float restingHealthRate; //sets the heal rate in hp per second
    public float fleeDistance = 1.0f;
    public int currentWaypoint = 0; //stores ai's current value in waypoint index
    public float withinWaypointRange = 1.0f; //value for if our ai is close enough to its waypoint
    public float stopDistance; //to stop AI before they enter the players space
    private float fireRadius = 4f; //gives the radius at which the enemy can turn and fire
    private float currentAngle; // stores the current angle of the tank cannon
    private float fireAngle; //stores the firing angle of the cannon
    private float distanceToTarget; //stores the distance to the target
    private float angleModifier = 1; //changes the range of the Wander radius
    private bool isWandering = false; //stores if the AI is wandering or not
    private float wanderingSpeed = 10; //speed of wander
    Vector3? direction = null; //random direction for wandering
    #endregion

    #region Builtin functions
    void Awake()
    {

    }

    void Start()
    {

    }


    void Update()
    {

        foreach (GameObject enemy in GameManager.instance.enemyList)
        {
            tf = enemy.GetComponent<Transform>(); //sets the transform of the AI pawn
            ePawn = enemy.GetComponent<EnemyTank>(); //gets the pawn of the AI pawn
            motor = enemy.GetComponent<TankMotor>(); //gets the TankMotor and sets it to motor
            eHealth = enemy.GetComponent<Health>(); //gets the health script of the enemy

            if (enemyType == EnemyType.Test)
            {
                switch (ePawn.aiState) //Chase, Attack, Flee, Rest, Patrol, Idle
                {
                    case Pawn.AIState.Attack:
                        //get our rotation instructions from the targets position - the enemy position
                        Quaternion desiredRotation = Quaternion.LookRotation(targetTf.position - ePawn.turretTf.transform.position, Vector3.up);
                        //rotate towards our target starting from our current rotation to the desired rotation 
                        ePawn.turretTf.transform.rotation = Quaternion.RotateTowards(ePawn.turretTf.transform.rotation, desiredRotation, ePawn.rotateSpeed * Time.fixedDeltaTime);
                        //get our distance to target
                        distanceToTarget = GetDistanceToTarget();
                        //get current angle to target
                        currentAngle = GetAngleToTarget();

                        //if the distance to our target is less than or equal to our fire radius
                        if (distanceToTarget <= fireRadius)
                        {
                            // and if our current angle is less than or equal to our weapon's fire angle
                            if (currentAngle <= fireAngle)
                            {
                                //Attack the target
                                Attack();
                            }
                        }
                        else if (distanceToTarget > fireRadius) 
                        {
                            ChangeState(Pawn.AIState.Chase, ePawn); //if the distance to the target is greater than fire radius than switch to chase
                        }
                        break;
                    case Pawn.AIState.Chase:

                        distanceToTarget = GetDistanceToTarget(); //sets the distanceToTarget

                        if (!See(target))
                        {
                            ChangeState(Pawn.AIState.Patrol, ePawn); //changes to the patrol state of the target cant be seen
                            return;
                        }
                        else if (avoidanceStage != 0)
                        {
                            Avoidance(); //if the avoidance stage is not zero, start avoiding
                        }
                        else if (distanceToTarget <= fireRadius * 0.5) 
                        {
                            ChangeState(Pawn.AIState.Attack, ePawn); //change state to attack if the distance to the target is less than or half the fireRadius
                        }
                        else
                        {
                            Chase(targetTf); //makes the enemy chase
                        }
                        break;
                    case Pawn.AIState.Flee:

                        distanceToTarget = GetDistanceToTarget(); //sets the distanceToTarget

                        if (eHealth.GetPercent() >= 0.25)
                        {
                            Flee(); // at 25 percent health enemies flee
                        }
                        else if (distanceToTarget > fireRadius * 2) 
                        {
                            ChangeState(Pawn.AIState.Rest, ePawn); //if the distance to the target is greater than 2 times the fire radius, rest
                        }
                        break;
                    case Pawn.AIState.Patrol:
                        if (avoidanceStage != 0) //if the avoidance stage isnt zero.
                        {
                            //avoid obstacles
                            Avoidance();
                        }
                        else if (See(target))
                        {
                            ChangeState(Pawn.AIState.LookAt, ePawn); //change state to look at
                        }
                        else
                        {
                            //otherwise patrol
                            Patrol();
                        }
                        break;
                    case Pawn.AIState.LookAt:
                        if (See(target))
                        {
                            SetTarget(target, target.transform); //sets the new target
                            ChangeState(Pawn.AIState.Chase, ePawn); //changes state to chase
                        }
                        else 
                        {
                            //input a timer for going into Patrol state
                            ChangeState(Pawn.AIState.Patrol, ePawn); //go back into patrol if they dont see you
                        }
                        break;
                    case Pawn.AIState.Rest:
                        if (eHealth.GetPercent() >= 0.25) //if health is below 25 percent, rest
                        {
                            Rest();
                        }
                        else 
                        {
                            ChangeState(Pawn.AIState.Patrol, ePawn); //return top patrolling
                        }
                        break;
                    case Pawn.AIState.Idle:
                        Idle();
                        break;
                }
            }
        }
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

    private void Attack()
    {
       //fire your weapon
       base.ePawn.Shoot(base.ePawn.p_shotForce);
    }

    private void Chase(Transform targetTf)
    {
        GetMoveRotateFloats();
        motor.Rotation(rotation);
        motor.Move(movement);
        
    }

    private void Flee()
    {
        GetMoveRotateFloats();
        motor.Rotation(-rotation);
        motor.Move(-movement);
    }
    private void Patrol()
    {
        //set target equal to current waypoint
        newTarget = GameManager.instance.waypoints[currentWaypoint].transform.gameObject;
        //set target transform equal to current waypoint transform
        newTargetTf = GameManager.instance.waypoints[currentWaypoint].transform;
        SetTarget(newTarget, newTargetTf);

        if (motor.RotateTowards(GameManager.instance.waypoints[currentWaypoint].position, base.ePawn.rotateSpeed))
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
        //if close to waypoint
        Vector3 delta = GameManager.instance.waypoints[currentWaypoint].position - tf.position;
        delta.y = 0;
        if (delta.sqrMagnitude < withinWaypointRange)
        {
            //and if the waypoint index hasn't been completed
            if (currentWaypoint < GameManager.instance.waypoints.Count - 1)
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
    private void Rest()
    {
        Health health = GetComponent<Health>(); 
        health.Heal(base.ePawn.healRate * Time.deltaTime); //heals the healRate every second
    }

    private void Idle()
    {
        Debug.Log("Idling");
    }
    #endregion

    private float GetDistanceToTarget()
    {
        float distanceToTarget = Vector3.Distance(targetTf.position, base.ePawn.transform.position); //finds the distance to the target
        return distanceToTarget; //returns it
    }

    private float GetAngleToTarget()
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

    private void RotateAgent()
    {
        float rotateOrientation = Random.Range(-30f, 30f) * angleModifier; //random number * -30 degrees and 30 degrees times the angleModifier
        randomQuat = Quaternion.AngleAxis(rotateOrientation, Vector3.up); //determines the new rotation the enemy is facing
        rotation = randomQuat.y;
    }

    private void MoveAgent()
    {
        //TODO; Make a random Vector3 Generator that is focused around the enemy

        float moveRange = Random.Range(5f, 20f); //random number for moving forwards
        randomVec = new Vector3 (moveRange, ePawn.transform.position.z); //determines a vector for the new movement range
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

    #endregion
}
/*
 Wander Logic
                        if (isWandering) 
                        {
                            if (direction.HasValue) 
                            {
                                GetMoveRotateFloats();
                                motor.Rotation(rotation);
                                motor.Move(movement);
                            }
                        }
                        isWandering = true;
                        StartCoroutine(Wander());

Old Patrol Stuff

       //set target equal to current waypoint
        newTarget = GameManager.instance.waypoints[currentWaypoint].transform.gameObject;
        //set target transform equal to current waypoint transform
        newTargetTf = GameManager.instance.waypoints[currentWaypoint].transform;
        SetTarget(newTarget, newTargetTf);

        if (motor.RotateTowards(GameManager.instance.waypoints[currentWaypoint].position, base.ePawn.rotateSpeed))
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
        //if close to waypoint
        Vector3 delta = GameManager.instance.waypoints[currentWaypoint].position - tf.position;
        delta.y = 0;
        if (delta.sqrMagnitude < withinWaypointRange)
        {
            //and if the waypoint index hasn't been completed
            if (currentWaypoint < GameManager.instance.waypoints.Count - 1)
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
 */

