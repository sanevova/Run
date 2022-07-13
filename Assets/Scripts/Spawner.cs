using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    GameController game;
    public GameObject tree;
    public GameObject fireball;
    public float spawnTreeProbability;

    const float SPAWN_INTERVAL_MIN = 0.8F;
    const float SPAWN_INTERVAL_MAX = 1.6F;

    private float lastSpawnTime = 0;
    private float nextSpawnTime = 0;

    void Start() {
        game = GameObject.FindObjectOfType<GameController>();
        tree.SetActive(false);
        fireball.SetActive(false);
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
        if (Random.Range(0f, 1.0f) < spawnTreeProbability) {
            SpawnTree();
        } else {
            SpawnFireball();
        }
    }

    void SpawnFireball() {
        SpawnEnemyFromPrototype(fireball);
    }

    void SpawnTree() {
        SpawnEnemyFromPrototype(tree);
    }

    void SpawnEnemyFromPrototype(GameObject prototype) {
        GameObject enemy = Instantiate(prototype, prototype.transform.position, tree.transform.rotation);
        enemy.SetActive(true);
        enemy.GetComponent<Rigidbody2D>().velocity = Vector2.left * game.speedX;
    }
}
