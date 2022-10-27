using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    [SerializeField]
    GameObject player, centerPoint;
    [SerializeField]
    float xDistance, yDistance, maxDistanceX, maxDistanceY, speed;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        centerPoint = transform.parent.gameObject;
        yDistance = 0;
    }


    void Update()
    {
        xDistance = player.transform.position.x - centerPoint.transform.position.x;
        yDistance = player.transform.position.y - centerPoint.transform.position.y;
        //yDistance = Mathf.Clamp(yDistance, 0, yDistance);
        if (Mathf.Abs(xDistance) > maxDistanceX || Mathf.Abs(yDistance) > maxDistanceY)
        {
            centerPoint.transform.Translate(new Vector3(xDistance, yDistance, 0) * speed * Time.deltaTime);
        }
    }
}
