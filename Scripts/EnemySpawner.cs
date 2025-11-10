using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 2f;
    public Transform[] spawnPoints;
    public float minSpawnInterval = 0.5f;  // Límite inferior
    public float decreaseRate = 0.05f;     // Cuánto disminuye por ciclo

    private float timer;
    private int lastSpawnIndex = -1; // -1 indica que no hay spawn previo

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
            // Disminuir progresivamente el intervalo de aparición
            spawnInterval = Mathf.Max(spawnInterval - decreaseRate, minSpawnInterval);
        }
    }

    void SpawnEnemy()
    {
        int index;

        // Asegurarse de que el nuevo índice sea diferente al anterior
        do
        {
            index = Random.Range(0, spawnPoints.Length);
        } while (index == lastSpawnIndex && spawnPoints.Length > 1);

        lastSpawnIndex = index;
        Transform spawnPoint = spawnPoints[index];

        Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
    }
}
