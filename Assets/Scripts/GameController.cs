using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class GameController : MonoBehaviour {
    const int SPEED_INCREASE_INTERVAL_GROWTH = 50;

    DinoController dino;
    public float speedX = 10;
    public GameObject groundTilemap;
    public TMP_Text deathText;
    public TMP_Text scoreText;
    private float score = 0;
    private int nextSpeedIncreaseScore = SPEED_INCREASE_INTERVAL_GROWTH;
    private int speedIncreaseInterval = SPEED_INCREASE_INTERVAL_GROWTH;
    public float speedIncreaseFactor = 1.2f;
    void Start() {
        dino = GameObject.FindObjectOfType<DinoController>();
    }

    void Update() {
        ProcessInputs();
        MoveGround();
        UpdateScore();
        UpdateSpeed();
    }

    void ProcessInputs() {
        if (Input.GetButtonDown("Fire2")) {
            // pause/unpause
            Time.timeScale = 1 - Time.timeScale;
            deathText.enabled = !deathText.enabled;
            dino.GetComponent<SpriteRenderer>().color = Color.white;
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
        int displayScore = (int)Mathf.Floor(score);
        scoreText.text = string.Format("score: {0}\nspeed: {1:0.0}", displayScore.ToString(), speedX);
    }

    void UpdateSpeed() {
        if (score < nextSpeedIncreaseScore) {
            return;
        }
        speedX *= speedIncreaseFactor;
        foreach (GameObject target in GameObject.FindGameObjectsWithTag("Target")) {
            target.GetComponent<Rigidbody2D>().velocity = Vector2.left * speedX;
        }
        nextSpeedIncreaseScore += speedIncreaseInterval;
        speedIncreaseInterval += SPEED_INCREASE_INTERVAL_GROWTH;
    }
}
