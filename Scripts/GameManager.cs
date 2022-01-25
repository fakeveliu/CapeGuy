using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum GameState { getReady, playing, oops, gameOver, roundWin };

public class GameManager : MonoBehaviour
{
    // Singleton Declaration
    public static GameManager S;

    // Game State
    public GameState gameState;

    // UI variables
    public TextMeshProUGUI messageOverlay;
    public TextMeshProUGUI messageLives;
    public TextMeshProUGUI messageScore;
    public GameObject btn_returnToMenu;

    // Enemy variables
    private GameObject currentEnemies;

    // Player variables
    public GameObject playerPrefab;
    private GameObject currentPlayer;
    private Vector3 spawnPoint = new Vector3();

    // Camera variables
    private GameObject camera;
    private Vector3 camInitPos = new Vector3();
    private Vector3 camSpawnPos = new Vector3();
    // public GameObject camCheckPrefab;
    private GameObject currentCamChecks;
    private float x_offset, y_offset, x_dtime, y_dtime;

    // Game variables
    private int livesStart = 3, livesLeft = 3;
    private int scoreStart = 0 , currentScore = 0;

    void Awake ()
    {
        // Singleton Definition
        if (GameManager.S)
        {
            // singleton exists, delete this object
            Destroy(this.gameObject);
        }
        else
        {
            S = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        btn_returnToMenu.SetActive(false);
        messageOverlay.enabled = false;
        messageLives.enabled = true;
        messageScore.enabled = true;
        DontDestroyOnLoad(this);

        StartANewGame();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void StartANewGame()
    {
        livesLeft = livesStart;
        currentScore = scoreStart;
    }

    public void UpdateCamSpawn(float dx, float dy, float dtx, float dty)
    {
        x_offset = dx;
        y_offset = dy;
        x_dtime = dtx;
        y_dtime = dty;
    }

    void ResetCamera()
    {
        camera.transform.position = camSpawnPos;
        camera.GetComponent<CameraFollow>().player = currentPlayer;
        camera.GetComponent<CameraFollow>().SetCamera(x_offset, y_offset, x_dtime, y_dtime);
    }

    public void ResetRound()
    {
        btn_returnToMenu = LevelManager.S.levelReturn;
        btn_returnToMenu.SetActive(false);

        if (!SoundManager.S.ambientSound.isPlaying)
        {
            SoundManager.S.ambientSound.Play();
        }

        UpdateScore();
        UpdateLives();
        UpdateCamSpawn(LevelManager.S.dx, LevelManager.S.dy, LevelManager.S.dtx, LevelManager.S.dty);

        camInitPos = LevelManager.S.levelCamInit.transform.position;
        spawnPoint = LevelManager.S.levelSpawn.transform.position;
        camera = LevelManager.S.levelCamera;
        currentEnemies = LevelManager.S.levelEnemies;
        currentPlayer = Instantiate(playerPrefab);
        currentCamChecks = LevelManager.S.levelCamChecks;

        if (LevelManager.S.finalScene && !currentEnemies.GetComponent<Boss>().player)
            currentEnemies.GetComponent<Boss>().player = currentPlayer;

        camSpawnPos = camInitPos;
        currentPlayer.transform.position = spawnPoint;
        ResetCamera();

        gameState = GameState.getReady;

        StartCoroutine(GetReadyState());
    }

    public void Respawn()
    {
        currentPlayer = Instantiate(playerPrefab);
        if (!LevelManager.S.finalScene)
        {
            if (currentCamChecks) { Destroy(currentCamChecks); }
            currentCamChecks = Instantiate(LevelManager.S.levelCamChecksPrefab);
            foreach (Transform camCheck in currentCamChecks.transform)
            {
                camCheck.gameObject.GetComponent<Checkpoint1>().camera = camera;
            }
        }

        if (!SoundManager.S.ambientSound.isPlaying)
        {
            SoundManager.S.ambientSound.Play();
        }

        if (LevelManager.S.finalScene && !currentEnemies.GetComponent<Boss>().player)
        {
            currentEnemies.GetComponent<Boss>().ResetHealth();
            currentEnemies.GetComponent<Boss>().player = currentPlayer;
        }

        currentPlayer.transform.position = spawnPoint;
        ResetCamera();

        gameState = GameState.getReady;

        StartCoroutine(GetReadyState());
    }

    public IEnumerator GetReadyState()
    {
        messageOverlay.enabled = true;
        messageOverlay.text = "GET READY!";

        yield return new WaitForSeconds(2.0f);

        messageOverlay.enabled = false;

        StartRound();
    }

    private void StartRound()
    {
        gameState = GameState.playing;
        if (LevelManager.S.finalScene)
        {
            StartCoroutine(currentEnemies.GetComponent<Boss>().Attack());
        } else {
            foreach (Transform enemy in currentEnemies.transform)
            {
                StartCoroutine(enemy.GetComponent<Enemy>().Shoot());
            }
        }
    }

    public void Checkpoint(Vector3 newPlayerSpawn, Vector3 newCameraSpawn, int number)
    {
        spawnPoint = newPlayerSpawn;
        camSpawnPos = newCameraSpawn;

        StartCoroutine(InGameMessage("CHECKPOINT" + number));
    }

    public void CoinCollected()
    {
        currentScore += 50;
        UpdateScore();
    }

    public void PoweredUp()
    {
        livesLeft += 1;
        UpdateLives();

        StartCoroutine(InGameMessage("LIVES +1"));
    }

    public IEnumerator InGameMessage(string message)
    {
        messageOverlay.enabled = true;
        messageOverlay.text = message;

        yield return new WaitForSeconds(1.0f);

        messageOverlay.enabled = false;
    }

    public void EnemyDestroyed()
    {
        if (LevelManager.S.finalScene)
            currentScore += 500;
        else 
            currentScore += 100;
        UpdateScore();
    }

    public void PlayerDestroyed()
    {
        SoundManager.S.ambientSound.Stop();
        SoundManager.S.MakePlayerDeathSound();

        StartCoroutine(OopsState());
    }

    private void UpdateScore()
    {
        messageScore.text = "SCORE: " + currentScore;
    }

    private void UpdateLives()
    {
        messageLives.text = "LIVES: " + livesLeft;
    }

    public IEnumerator OopsState()
    {
        gameState = GameState.oops;

        livesLeft--;
        UpdateLives();

        if (livesLeft <= 0)
        {
            GameOverLose();
        }
        else
        {
            // turn on the message
            messageOverlay.enabled = true;
            messageOverlay.text = "OOPS...";

            // leave the message for 2 seconds
            yield return new WaitForSeconds(2.0f);

            messageOverlay.enabled = false;

            // currentPlayer = Instantiate(playerPrefab);
            Respawn();
        }
    }

    private void GameOverLose()
    {
        // enter the game over state
        gameState = GameState.gameOver;

        SoundManager.S.MakeLoseSound();
        SoundManager.S.ambientSound.Stop();

        // set the message
        messageOverlay.enabled = true;
        messageOverlay.text = "You Lose.\n";

        btn_returnToMenu.SetActive(true);
    }

    public void GameOverWin()
    {
        // enter the game over state
        gameState = GameState.gameOver;

        SoundManager.S.MakeWinSound();
        SoundManager.S.ambientSound.Stop();

        // set the message
        messageOverlay.enabled = true;
        messageOverlay.text = "You Win!!!\n";

        btn_returnToMenu.SetActive(true);
    }

    public void EndOfLevel()
    {
        StartCoroutine(LevelComplete());
    }

    public IEnumerator LevelComplete()
    {
        gameState = GameState.roundWin;

        // set the message
        messageOverlay.enabled = true;
        messageOverlay.text = "LEVEL COMPLETE!";

        yield return new WaitForSeconds(2.0f);

        LevelManager.S.RoundWin();
    }

}
