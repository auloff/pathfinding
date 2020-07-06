using System;
using UnityEngine;

public class InputSystem : MonoBehaviour
{
    public event Action<Vector3> userMouseClick;

    public bool IsMouseClick { get => Input.GetMouseButtonDown(0); }

    void Update()
    {
        if (IsMouseClick)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                userMouseClick?.Invoke(hit.point);
            }
        }
    }
}