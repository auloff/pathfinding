using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Range(0.01f,5f)]
    public float speed;
    public bool IsMoving { get; private set; } = false;
    public event Action StopMoving;

    private List<Vector3> _path;
    private int _currentState = 0;

    public void StartMove(List<Vector3> path)
    {
        if (IsMoving) return;
        _path = path;
        IsMoving = true;
    }

    private void FixedUpdate()
    {
        if (!IsMoving) return;

        Vector3 newPos = new Vector3(_path[_currentState].x, 0.5f, _path[_currentState].z);
        this.transform.position = Vector3.MoveTowards(this.transform.position, _path[_currentState], speed);

        if (this.transform.position.x == _path[_currentState].x && this.transform.position.y == _path[_currentState].y)
            _currentState++;
        if (_currentState >= _path.Count)
        {
            _currentState = 0;
            IsMoving = false;
            StopMoving?.Invoke();
        }
    }
}