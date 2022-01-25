using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;

    public int level;

    private float xVelocity = 0.0f;
    private float yVelocity = 0.0f;

    public float x_offset, y_offset, x_dtime, y_dtime;
    public bool x_fix, cam_fix;

    void Start()
    {
        x_fix = false;
        SetCamera(LevelManager.S.dx, LevelManager.S.dy, LevelManager.S.dtx, LevelManager.S.dty);
    }

    void FixedUpdate()
    {
        if (player == null || LevelManager.S.finalScene || cam_fix)
            return;

        MoveCamera();
    }

    public void SetCamera(float dx, float dy, float dtx, float dty)
    {
        x_offset = dx;
        y_offset = dy;
        x_dtime = dtx;
        y_dtime = dty;
    }

    private void MoveCamera()
    {
        Vector3 playerposition = player.transform.position;
        Vector3 cameraposition = transform.position;

        if (x_fix)
        {
            cameraposition.x = Mathf.SmoothDamp(cameraposition.x, playerposition.x + x_offset, ref xVelocity, x_dtime);
        } else {
            if (playerposition.x + x_offset > cameraposition.x)
                cameraposition.x = Mathf.SmoothDamp(cameraposition.x, playerposition.x + x_offset, ref xVelocity, x_dtime);
        }
        cameraposition.y = Mathf.SmoothDamp(cameraposition.y, playerposition.y + y_offset, ref yVelocity, y_dtime);

        transform.position = cameraposition;
    }
}
