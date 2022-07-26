using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : Controller
{
    public enum InputScheme { WASD, arrowKeys };
    public InputScheme input = InputScheme.WASD;
    protected float movement;
    protected float turn;
    /*        
         var forwardInput = Input.GetAxis("Vertical");
        var rotationInput = Input.GetAxis("Horizontal");*/
    void Start()
    {
        ePawn = GetComponent<Pawn>();
        motor = GetComponent<TankMotor>();
    }

    // Update is called once per frame
    void Update()
    {
        var forwardInput = Input.GetAxis("Vertical");
        var rotationInput = Input.GetAxis("Horizontal");
        //switch statement for control schemes
        switch (input)
        {
            case InputScheme.WASD:
                if (Input.GetKey(KeyCode.W))
                {
                    motor.Move(forwardInput);
                }
                if (Input.GetKey(KeyCode.S))
                {
                    motor.Move(forwardInput);
                }
                if (Input.GetKey(KeyCode.A))
                {
                    motor.Rotation(rotationInput);
                }
                if (Input.GetKey(KeyCode.D))
                {
                    motor.Rotation(rotationInput);
                }
                if (Input.GetKey(KeyCode.E))
                {
                    motor.TurretRotateRight();
                }
                if (Input.GetKey(KeyCode.Q))
                {
                    motor.TurretRotateLeft();
                }
                if (Input.GetKey(KeyCode.Space))
                {
                    ePawn.Shoot(ePawn.p_shotForce);
                }

                break;

            case InputScheme.arrowKeys:

                if (Input.GetKey(KeyCode.UpArrow))
                {
                    motor.Move(forwardInput);
                }
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    motor.Move(forwardInput);
                }
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    motor.Rotation(rotationInput);
                }
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    motor.Rotation(rotationInput);
                }
                if (Input.GetKey(KeyCode.Keypad1))
                {
                    motor.TurretRotateRight();
                }
                if (Input.GetKey(KeyCode.RightShift))
                {
                    motor.TurretRotateLeft();
                }
                if (Input.GetKey(KeyCode.RightAlt))
                {
                    ePawn.Shoot(ePawn.p_shotForce);
                }
                break;
        }
    }
}

