using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint1 : MonoBehaviour
{
    public GameObject camera;
    private CameraFollow camFollow;
    public bool isXfix, isCamFix;
    public int checkpointNum;
    public float dx, dy, dtx, dty;

    void Start()
    {
        camFollow = camera.GetComponent<CameraFollow>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GetComponent<EdgeCollider2D>().enabled = false;
            camFollow.x_fix = isXfix;
            camFollow.cam_fix = isCamFix;
            camFollow.SetCamera(dx, dy, dtx, dty);
            if (checkpointNum != 0)
            {
                SoundManager.S.MakeCheckpointSound();
                GameManager.S.Checkpoint(transform.position, camera.transform.position, checkpointNum);
                GameManager.S.UpdateCamSpawn(dx, dy, dtx, dty);
            }
        }
    }
}
