using Pathfinding.Helpers;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class ColorChanger : MonoBehaviour
{
    public HexHelper hexHelper;

    private MeshRenderer _mesh;

    private void Awake()
    {
        _mesh = this.GetComponent<MeshRenderer>();
    }
    private void Start()
    {
        hexHelper.hexReady += (color) =>
        {
            _mesh.material.color = color;
        };
    }
}