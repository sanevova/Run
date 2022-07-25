using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class GameController : MonoBehaviour {
    const int SPEED_INCREASE_INTERVAL_GROWTH = 100;

    DinoController dino;
    public float speedX = 10;
    public GameObject groundTilemap;
    public TMP_Text deathText;
    public TMP_Text scoreText;
    public TMP_Text highScoreText;
    public float speedIncreaseFactor = 1.1f;
    private float score = 0;
    private float highScore = 0;
    private int nextSpeedIncreaseScore = SPEED_INCREASE_INTERVAL_GROWTH;
    private int speedIncreaseInterval = SPEED_INCREASE_INTERVAL_GROWTH;
    private float startSpeedX;

    void Start() {
        dino = GameObject.FindObjectOfType<DinoController>();
        startSpeedX = speedX;
        ShowHighscore();

    }

    void Update() {
        ProcessInputs();
        MoveGround();
        UpdateScore();
        UpdateSpeed();
    }

    void ProcessInputs() {
        if (dino.isDead &&  Input.anyKey) {
            // pause/unpause
            Time.timeScale = 1 - Time.timeScale;
            if (dino.isDead) {
                RestartGame();
            }
            deathText.enabled = dino.isDead;
        }
    }

    void MoveGround() {
        Tilemap tilemap = groundTilemap.GetComponent<Tilemap>();
        tilemap.tileAnchor += Vector3.left * speedX * Time.deltaTime;
        float x = tilemap.tileAnchor.x;
        if (x < 0) {
            tilemap.tileAnchor += Vector3.right * Mathf.Floor(Mathf.Abs(x) + 1);
        }
    }

    void UpdateScore() {
        score += Time.deltaTime * speedX;
        scoreText.text = string.Format("speed: {0:0.0}\nscore: {1,5:0}", speedX, score);
    }

    public void OnHit() {
        dino.isDead = true;
        deathText.enabled = true;
    }

    void RestartGame() {
        Debug.Log("restart");
        if (score > highScore) {
            highScore = score;
            ShowHighscore();
        }
        foreach (Target target in GameObject.FindObjectsOfType<Target>()) {
            Destroy(target.gameObject);
        }
        dino.GetComponent<SpriteRenderer>().color = Color.white;
        dino.isDead = false;
        speedX = startSpeedX;
        score = 0;
    }

    void ShowHighscore() {
        highScoreText.text = string.Format("highscore: {0,5:0}", highScore);
    }

    void UpdateSpeed() {
        if (score < nextSpeedIncreaseScore) {
            return;
        }
        speedX *= speedIncreaseFactor;
        foreach (Target target in GameObject.FindObjectsOfType<Target>()) {
            target.DidUpdateGameSpeed(speedX);
        }
        nextSpeedIncreaseScore += speedIncreaseInterval;
        speedIncreaseInterval += SPEED_INCREASE_INTERVAL_GROWTH;
    }
}
