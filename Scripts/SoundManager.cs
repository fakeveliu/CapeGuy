using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager S; // Singleton Definition

    private AudioSource audio;

    public AudioClip jumpSound;

    public AudioClip enemyDeathSound;
    public AudioClip playerDeathSound;
    public AudioClip coinSound;
    public AudioClip powerUpSound;
    public AudioClip winSound;
    public AudioClip loseSound;
    public AudioClip checkpointSound;
    public AudioClip laserSound;
    public AudioClip levelSound;
    public AudioSource ambientSound;

    private void Awake()
    {
        S = this; // singleton is assigned
    }

    // Start is called before the first frame update
    void Start()
    {
        // assign the audio source component
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MakeJumpSound()
    {
        audio.PlayOneShot(jumpSound, 0.8f);
    }

    public void MakePlayerDeathSound()
    {
        audio.PlayOneShot(playerDeathSound, 0.4f);
    }

    public void MakeEnemyDeathSound()
    {
        audio.PlayOneShot(enemyDeathSound, 0.8f);
    }

    public void MakeCoinSound()
    {
        audio.PlayOneShot(coinSound, 1.7f);
    }

    public void MakePowerUpSound()
    {
        audio.PlayOneShot(powerUpSound);
    }

    public void MakeLevelSound()
    {
        audio.PlayOneShot(levelSound, 0.8f);
    }

    public void MakeWinSound()
    {
        audio.PlayOneShot(winSound, 0.8f);
    }

    public void MakeLoseSound()
    {
        audio.PlayOneShot(loseSound, 0.8f);
    }

    public void MakeCheckpointSound()
    {
        audio.PlayOneShot(checkpointSound, 0.8f);
    }

    public void MakeLaserSound()
    {
        audio.PlayOneShot(laserSound, 0.8f);
    }

    public void StopAllSounds()
    {
        // stop the ambient noise
        ambientSound.Stop();

        // stop all child sound
        foreach (Transform child in this.transform)
        {
            Destroy(child.gameObject);
        }
    }
}