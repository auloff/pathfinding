using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject prefab;
    public Vector2 spawnZone;

    private Transform _currentObstacle;

    private void Start()
    {
        SpawnObstacle();
    }

    public void SpawnObstacle()
    {
        float randomX = Random.Range(transform.position.x - spawnZone.x / 2, transform.position.x + spawnZone.x / 2);
        float randomY = Random.Range(transform.position.y - spawnZone.y / 2, transform.position.y + spawnZone.y / 2);

        float randomA = Random.Range(0, 360);

        if (_currentObstacle == null)
        {
            _currentObstacle = Instantiate(prefab, new Vector3(randomX, transform.position.y, randomY), Quaternion.AngleAxis(randomA, transform.up), this.transform).transform;
        }
        else
        {
            _currentObstacle.localPosition = new Vector3(randomX, transform.position.y, randomY);
            _currentObstacle.localRotation = Quaternion.AngleAxis(randomA, transform.up);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(this.transform.position, new Vector3(spawnZone.x, 1, spawnZone.y));
    }
}
