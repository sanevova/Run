using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    private GameController game;
    public Target tree;
    public Target fireball;
    public Target slime;
    public float spawnTreeProbability;
    public float spawnSlimesProbability;
    public float slimeOffsetFromX;
    public float slimeOffsetToX;

    const float SPAWN_INTERVAL_MIN = 0.8F;
    const float SPAWN_INTERVAL_MAX = 1.6F;

    private float lastSpawnTime = 0;
    private float nextSpawnTime = 0;

    void Start() {
        game = GameObject.FindObjectOfType<GameController>();
        tree.gameObject.SetActive(false);
        fireball.gameObject.SetActive(false);
        slime.gameObject.SetActive(false);
    }

    void Update() {
        SpawnEnemies();
    }

    void SpawnEnemies() {
        if (nextSpawnTime > Time.time) {
            return;
        }
        SpawnEnemy();
        lastSpawnTime = Time.time;
        float interval = Random.Range(SPAWN_INTERVAL_MIN, SPAWN_INTERVAL_MAX);
        nextSpawnTime = lastSpawnTime + interval;
    }
    
    void SpawnEnemy() {
        if (Roll() < spawnTreeProbability) {
            SpawnTree();
        } else {
            SpawnFireball();
        }
        if (Roll() < spawnSlimesProbability) {
            SpawnSlime();
        }
    }

    float Roll() {
        return Random.Range(0f, 1.0f);
    }

    void SpawnFireball() {
        SpawnEnemyFromPrototype(fireball, 0);
    }

    void SpawnTree() {
        SpawnEnemyFromPrototype(tree, 0);
    }

    void SpawnSlime() {
        float slimeOffsetX = Random.Range(slimeOffsetFromX, slimeOffsetToX);
        SpawnEnemyFromPrototype(slime, slimeOffsetX);
    }

    void SpawnEnemyFromPrototype(Target prototype, float offsetX) {
        Vector3 position = prototype.transform.position + Vector3.right * offsetX;
        Target enemy = Instantiate(prototype, position, tree.transform.rotation);
        enemy.gameObject.SetActive(true);
        enemy.GetComponent<Rigidbody2D>().velocity = Vector2.left * game.speedX;
        enemy.DidSpawn();
    }
}
