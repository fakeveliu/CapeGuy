using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager S;

    [Header("Level Info")]
    public string levelName; // string to display at the level start
    public GameObject levelEnemies;
    public GameObject levelPlayer;
    public GameObject levelCamera;
    public GameObject levelSpawn;
    public GameObject levelCamInit;
    public GameObject levelCamChecks;
    public GameObject levelCamChecksPrefab;
    public GameObject levelReturn;

    public float dx, dy, dtx, dty;

    [Header("Scene Info")]
    public string nextScene; // string that is the level name
    public bool finalScene; // indicates that this is the end of the game

    private void Awake()
    {
        S = this; // singleton assignment
    }

    private void Start()
    {
        if (GameManager.S)
        {
            GameManager.S.ResetRound();
        }
    }

    public void RoundWin()
    {
        SceneManager.LoadScene(nextScene);
    }

    public void RestartLevel()
    {
        // reload this scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMainMenu()
    {
        Destroy(GameManager.S.gameObject);
        SceneManager.LoadScene("TitleMenu");

    }


}
