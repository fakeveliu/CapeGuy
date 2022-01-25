using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L2CameraFollow : MonoBehaviour
{
    public GameObject player;

    // public float MIN_Y_OFFSET;
    public float Y_INIT;

    public GameObject CHECKPOINT1;
    public GameObject CHECKPOINT2;
    public GameObject CHECKPOINT3;
    public GameObject CHECKPOINT4;

    private float xVelocity = 0.0f;
    private float yVelocity = 0.0f;

    public float offset1, offset2, offset3;


    void Start()
    {

    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (player == null) { return; }

        Vector3 playerposition = player.transform.position;
        Vector3 cameraposition = transform.position;

        if (playerposition.x > cameraposition.x && playerposition.x < CHECKPOINT4.transform.position.x)
        {
            cameraposition.x = Mathf.SmoothDamp(cameraposition.x, playerposition.x, ref xVelocity, 0.5f);
        }

        if (playerposition.x > CHECKPOINT3.transform.position.x)
        {
            cameraposition.y = Mathf.SmoothDamp(cameraposition.y, playerposition.y + offset3, ref yVelocity, 1.0f);
        }
        else if (playerposition.x > CHECKPOINT2.transform.position.x)
        {
            cameraposition.y = Mathf.SmoothDamp(cameraposition.y, playerposition.y + offset2, ref yVelocity, 1.0f);
        }
        else if (playerposition.x > CHECKPOINT1.transform.position.x)
        {
            cameraposition.y = Mathf.SmoothDamp(cameraposition.y, playerposition.y + offset1, ref yVelocity, 0.8f);
        }

        transform.position = cameraposition;
    }
}
