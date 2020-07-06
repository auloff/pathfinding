using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance = null;
    public static GameManager Instance { get => _instance; }

    private PlayerController _playerController;
    private ObstacleSpawner _obstacleSpawner;
    private InputSystem _inputSystem;
    private Pathfinder _pathfinder;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance == this)
            Destroy(gameObject);

        _obstacleSpawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<ObstacleSpawner>();
        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _inputSystem = GetComponent<InputSystem>();
        _pathfinder = GetComponent<Pathfinder>();
    }

    private void Start()
    {
        _playerController.StopMoving += () =>
        {
            _obstacleSpawner.SpawnObstacle();
        };

        _inputSystem.userMouseClick += (point) =>
        {
            if (_playerController.IsMoving) return;

            List<Vector3> playerPath = _pathfinder.FindPath(_playerController.gameObject.transform.position, point);
            _playerController.StartMove(playerPath);

        };
    }
}