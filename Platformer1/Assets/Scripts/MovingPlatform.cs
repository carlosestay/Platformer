using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{


    [SerializeField]
    float maxUp, maxDown, maxLeft, maxRight, speed;
    Vector3 direction;
    [SerializeField]
    bool horizontal, vertical;


    public enum Direction
    {
        Idle,
        Left,
        Right,
        Up,
        Down,
    }
    Direction myDirection;
    void Start()
    {
        if (horizontal)
        {
            myDirection = Direction.Right;
        }
        else if (vertical)
        {
            myDirection = Direction.Up;
        }
        else
        {
            myDirection = Direction.Idle;
        }
    }


    void Update()
    {
        switch (myDirection)
        {
            case Direction.Left:
                transform.Translate(speed * Time.deltaTime * Vector3.left);
                if (transform.position.x < maxLeft)
                {
                    myDirection = Direction.Right;
                }
                break;
            case Direction.Right:
                transform.Translate(speed * Time.deltaTime * Vector3.right);
                if (transform.position.x > maxRight)
                {
                    myDirection = Direction.Left;
                }
                break;
            case Direction.Up:
                transform.Translate(speed * Time.deltaTime * Vector3.up);
                if (transform.position.y > maxUp)
                {
                    myDirection = Direction.Down;
                }
                break;
            case Direction.Down:
                transform.Translate(speed * Time.deltaTime * Vector3.down);
                if (transform.position.y < maxDown)
                {
                    myDirection = Direction.Up;
                }
                break;
            default:
                break;
        }
        // print(myDirection);
    }
}
