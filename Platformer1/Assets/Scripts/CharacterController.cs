using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField]
    float speed, movement, mmovement2, deltaMove, force, rotationSpeed;

    [SerializeField]
    bool remoteControl;

    //Remote Ccontrols;
    enum ButtonState
    {
        Idle,
        Pressed,
        Held,
        Released,
    };
    ButtonState upButton, downButton, leftButton, midButton, rightButton;


    

    [SerializeField]
    bool grounded;

    public enum CharacterState
    {
        Idle,
        RunningForward,
        RunningBack,
        Jumping,
        Test,
    };
    Animator animatorController;
    Rigidbody rBody;

    [SerializeField]
    public CharacterState mainState = CharacterState.Idle, prevState = CharacterState.Idle;


    Collision savedCollider;


    void Start()
    {
        animatorController = GetComponentInChildren<Animator>();
        rBody = GetComponentInChildren<Rigidbody>();
        EventManager.Instance.onBoardInteraction.AddListener(CommandReceived);
        remoteControl = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < -5)
        {
            EventManager.Instance.onGameEnd.Invoke(gameObject, new CustomEventArgs(false));
        }


        movement = Input.GetAxis("Horizontal") + Mathf.SmoothStep (0,  mmovement2, 700 *Time.deltaTime);
        switch (mainState)
        {
            case CharacterState.Idle:
                transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                if (Mathf.Abs(movement) > deltaMove && grounded)
                {
                    Run();
                }
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (grounded && mainState != CharacterState.Jumping)
                    {
                        Jump();
                        rBody.AddForce(force * Vector3.up);
                    }
                }
                break;

            case CharacterState.RunningForward:
                transform.localRotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 90, 0), rotationSpeed * Time.deltaTime);
                transform.Translate(Vector3.forward * speed * movement * Time.deltaTime);
                transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                if (Mathf.Abs(movement) <= deltaMove && grounded)
                {
                    Stop();
                }
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (grounded && mainState != CharacterState.Jumping)
                    {
                        //grounded = false;
                        Jump();
                        rBody.AddForce(force * Vector3.up);
                    }
                }
                break;

            case CharacterState.RunningBack:
                transform.localRotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, -90, 0), rotationSpeed * Time.deltaTime);
                transform.Translate(-Vector3.forward * speed * movement * Time.deltaTime);
                transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                if (Mathf.Abs(movement) <= deltaMove && grounded)
                {
                    Stop();
                }
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (grounded && mainState != CharacterState.Jumping)
                    {
                        if (grounded && mainState != CharacterState.Jumping)
                        {
                            //grounded = false;
                            Jump();
                            rBody.AddForce(force * Vector3.up);
                        }
                    }
                }
                break;

            case CharacterState.Jumping:
                if (prevState == CharacterState.RunningForward && !grounded)
                {
                    transform.localRotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 90, 0), rotationSpeed * Time.deltaTime);
                    movement = Mathf.Clamp(movement, -0.1f, movement);
                    transform.Translate(Vector3.forward * speed * movement * Time.deltaTime);
                }
                else if (prevState == CharacterState.RunningBack && !grounded)
                {
                    transform.localRotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, -90, 0), rotationSpeed * Time.deltaTime);
                    movement = Mathf.Clamp(movement, movement, 0.1f);
                    transform.Translate(-Vector3.forward * speed * movement * Time.deltaTime);
                }
                else if (Mathf.Abs(movement) <= deltaMove && grounded)
                {
                    StartCoroutine("SmoothStop");
                    //Stop();
                }
                else
                {
                    StartCoroutine("SmoothRun");
                }
                transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                break;

            default:
                break;
        }
    }


    private void OnCollisionStay(Collision collision)
    {
        if(!grounded && collision.transform.tag.Equals("Ground"))
        {
            grounded = true;
            transform.SetParent(collision.transform.root);
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        if (grounded)
        {
            grounded = false;
            transform.SetParent(null);
            if (mainState != CharacterState.Jumping)
            {
                Stop();
            }
        }
    }



    //State transitions 
    IEnumerator SmoothRun()
    {
        yield return new WaitForSeconds(0.05f);
        if (grounded)
        {
            print("Smooth Run is called");
            Run();
        }

    }

    IEnumerator SmoothStop()
    {
        yield return new WaitForSeconds(0.05f);
        if (grounded)
        {
            print("Smooth Top is called");
            //mainState = CharacterState.Idle;
            
            switch (prevState)
            {
                case CharacterState.Idle:
                    animatorController.SetBool("Run", false);
                    animatorController.SetBool("Stop", true);
                    animatorController.SetBool("Jump", false);
                    mainState = prevState;
                    break;
                case CharacterState.RunningForward:
                case CharacterState.RunningBack:
                    if (Mathf.Abs(movement) > deltaMove)
                    {
                        Run();
                    }
                    else
                    {
                        Stop();
                    }                   
                    break;
                case CharacterState.Jumping:
                    break;
                case CharacterState.Test:
                    break;
                default:
                    break;
            }
            
        }

    }

    private void Stop()
    {
        print("Stop is called");
        if (grounded)
        {
            mainState = CharacterState.Idle;
            animatorController.SetBool("Run", false);
            animatorController.SetBool("Stop", true);
            animatorController.SetBool("Jump", false);
        }
    }

    void Run()
    {
        if(movement > 0)
        {
            if (mainState != CharacterState.RunningForward && grounded)
            {
                mainState = CharacterState.RunningForward;
                animatorController.SetBool("Run", true);
                animatorController.SetBool("Stop", false);
                animatorController.SetBool("Jump", false);
            }
        }
        else if ((movement < 0))
        {
            if (mainState != CharacterState.RunningBack && grounded)
            {
                mainState = CharacterState.RunningBack;
                animatorController.SetBool("Run", true);
                animatorController.SetBool("Stop", false);
                animatorController.SetBool("Jump", false);
            }
        }
    }

    void Jump()
    {
        if (mainState != CharacterState.Jumping)
        {
            grounded = false;
            prevState = mainState;
            mainState = CharacterState.Jumping;
            animatorController.SetBool("Run", false);
            animatorController.SetBool("Stop", false);
            animatorController.SetBool("Jump", true);
        }

    }

    //Method that handles the rpesses from the board, via Events
    private void CommandReceived(GameObject sender, CustomEventArgs args)
    {      
        //UP Button
        if (args.BoardCommand.Equals("@SU1"))
        {
            print("UP pressed");
            upButton = ButtonState.Pressed;
        }
        if (args.BoardCommand.Equals("@SU0"))
        {
            print("UP released");
            upButton = ButtonState.Released;
        }

        //DOWN Button
        if (args.BoardCommand.Equals("@SD1"))
        {
            print("DOWN pressed");
            downButton = ButtonState.Pressed;
        }
        if (args.BoardCommand.Equals("@SD0"))
        {
            print("DOWN released");
            downButton = ButtonState.Released;
        }

        //LEFT Button
        if (args.BoardCommand.Equals("@SL1"))
        {
            print("LEFT pressed");
            leftButton = ButtonState.Pressed;
            mmovement2 = -1;
        }
        if (args.BoardCommand.Equals("@SL0"))
        {
            print("LEFT released");
            leftButton = ButtonState.Released;
            mmovement2 = 0;
        }

        //MID Button
        if (args.BoardCommand.Equals("@SM1"))
        {
            print("MID pressed");
            midButton = ButtonState.Pressed;
            if(grounded)
            {
                rBody.AddForce(force * Vector3.up);
                Jump();
            }
        }
        if (args.BoardCommand.Equals("@SM0"))
        {
            print("MID released");
            midButton = ButtonState.Released;
        }

        //RIGHT Button
        if (args.BoardCommand.Equals("@SR1"))
        {
            print("RIGHT pressed");
            rightButton = ButtonState.Pressed;
            mmovement2 = 1;          
        }
        if (args.BoardCommand.Equals("@SR0"))
        {
            print("RIGHT released");
            rightButton = ButtonState.Released;
            mmovement2 = 0;
        }
    }
}
