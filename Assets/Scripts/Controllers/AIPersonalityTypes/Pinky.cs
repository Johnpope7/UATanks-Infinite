using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinky : AIController
{
    void Update()
    {

        foreach (GameObject enemy in GameManager.instance.enemyList)
        {
            tf = enemy.GetComponent<Transform>(); //sets the transform of the AI pawn
            ePawn = enemy.GetComponent<EnemyTank>(); //gets the pawn of the AI pawn
            motor = enemy.GetComponent<TankMotor>(); //gets the TankMotor and sets it to motor
            eHealth = enemy.GetComponent<Health>(); //gets the health script of the enemy
        }
        switch (ePawn.aiState) //Chase, Attack, Flee, Rest, Patrol, Idle
        {
            case Pawn.AIState.Attack:
                Debug.Log("Attack: Entered");
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
                    Debug.Log("attack: distanceToTarget <= fireRadius");
                    // and if our current angle is less than or equal to our weapon's fire angle
                    if (currentAngle <= fireAngle)
                    {
                        Debug.Log("attack: currentAngle <= fireAngle");
                        //Attack the target
                        Attack();
                    }
                }
                else if (distanceToTarget > fireRadius)
                {
                    Debug.Log("Change state to Chase");
                    ChangeState(Pawn.AIState.Chase, ePawn); //if the distance to the target is greater than fire radius than switch to chase
                }
                break;
            case Pawn.AIState.Chase:

                distanceToTarget = GetDistanceToTarget(); //sets the distanceToTarget

                if (!See(target))
                {
                    Debug.Log("Chase: target not seen, change to patrol");
                    ChangeState(Pawn.AIState.Patrol, ePawn); //changes to the patrol state of the target cant be seen
                    return;
                }
                else if (avoidanceStage != 0)
                {
                    Debug.Log("Chase: Avoidance");
                    Avoidance(); //if the avoidance stage is not zero, start avoiding
                }
                else if (distanceToTarget <= fireRadius * 0.5)
                {
                    Debug.Log("Chase: Change state to attack");
                    ChangeState(Pawn.AIState.Attack, ePawn); //change state to attack if the distance to the target is less than or half the fireRadius
                }
                else
                {
                    Debug.Log("Chase: Target");
                    Chase(targetTf); //makes the enemy chase
                }
                break;
            case Pawn.AIState.Flee:

                distanceToTarget = GetDistanceToTarget(); //sets the distanceToTarget

                if (eHealth.percent >= 0.25)
                {
                    Debug.Log("Flee: Low Health");
                    Flee(); // at 25 percent health enemies flee
                }
                else if (distanceToTarget > fireRadius * 2)
                {
                    Debug.Log("Flee: Change state Rest");
                    ChangeState(Pawn.AIState.Rest, ePawn); //if the distance to the target is greater than 2 times the fire radius, rest
                }
                break;
            case Pawn.AIState.Patrol:
                if (avoidanceStage != 0) //if the avoidance stage isnt zero.
                {
                    Debug.Log("Patrol: Avoidance");
                    //avoid obstacles
                    Avoidance();
                }
                else if (See(target))
                {
                    Debug.Log("Patrol: Change state LookAt");
                    ChangeState(Pawn.AIState.LookAt, ePawn); //change state to look at
                }
                else
                {
                    Debug.Log("Patrol");
                    //otherwise patrol
                    Patrol();
                }
                break;
            case Pawn.AIState.LookAt:
                if (See(target))
                {
                    Debug.Log("LookAt: Set target");
                    SetTarget(target, target.transform); //sets the new target
                    Debug.Log("LookAt: Change state chase");
                    ChangeState(Pawn.AIState.Chase, ePawn); //changes state to chase
                }
                else
                {
                    Debug.Log("LookAt: change state Patrol");
                    //input a timer for going into Patrol state
                    ChangeState(Pawn.AIState.Patrol, ePawn); //go back into patrol if they dont see you
                }
                break;
            case Pawn.AIState.Rest:
                if (eHealth.percent >= 0.25) //if health is below 25 percent, rest
                {
                    Debug.Log("Rest: Low health");
                    Rest();
                }
                else
                {
                    Debug.Log("Rest: Change state patrol");
                    ChangeState(Pawn.AIState.Patrol, ePawn); //return top patrolling
                }
                break;
            case Pawn.AIState.Idle:
                Debug.Log("Idle");
                Idle();
                break;

        }
    }
}