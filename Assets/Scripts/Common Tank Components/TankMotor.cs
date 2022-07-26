using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMotor : MonoBehaviour
{
    //components
    private CharacterController characterController;
    public Transform tf;
    public Pawn pawn;
    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        //get components
        tf = GetComponent<Transform>();
        pawn = GetComponent<Pawn>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {

    }

    //function for tank movement
    public void Move(float forwardInput)
    {
        //moves the tank forward and backwards
        Vector3 desiredPosition = transform.position + (transform.forward * forwardInput * pawn.moveSpeed * Time.deltaTime);
        rb.MovePosition(desiredPosition);


    }

    public void Rotation(float rotationInput)
    {
        //Rotates the tank
        Quaternion desiredRotation = transform.rotation * Quaternion.Euler(Vector3.up * (pawn.rotateSpeed * rotationInput * Time.deltaTime));
        rb.MoveRotation(desiredRotation);
    }

    //RotateTowards (Target, Speed) - rotate towards the target (if possible).
    //If we rotate, then returns true. If we can't rotate returns false.
    public bool RotateTowards(Vector3 target, float speed)
    {
        //find the vector to our target by finding the difference between the target position and our position
        Vector3 vectorToTarget = target - tf.position;
        //find the quaternion that looks down that vector
        Quaternion targetRotation = Quaternion.LookRotation(vectorToTarget);
        if (targetRotation == tf.rotation)
        {
            return false;
        }
        else
        {
            tf.rotation = Quaternion.RotateTowards(tf.rotation, targetRotation, pawn.rotateSpeed * Time.deltaTime);
            return true;
        }
    }
    public void TurretRotateLeft()
    {
        pawn.turretTf.transform.Rotate(0, pawn.turretRotateSpeed * -1 * Time.deltaTime, 0);
    }

    public void TurretRotateRight()
    {
        pawn.turretTf.transform.Rotate(0, pawn.turretRotateSpeed * Time.deltaTime, 0);
    }
}


