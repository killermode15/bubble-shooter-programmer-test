using System.Collections;
using System.Collections.Generic;
using BubbleShooter;
using UnityEngine;

[RequireComponent(typeof(RaycastPath))]
public class RaycastPathUI : MonoBehaviour
{
    private const string POOL_IDENTIFIER = "Dots";

    [SerializeField] private Pool pool;

    private RaycastPath path;

    // Start is called before the first frame update
    private void Start()
    {
        path = GetComponent<RaycastPath>();
    }

    // Update is called once per frame
    private void Update()
    {
        DisplayPath(path.DotPositions);
    }

    private void DisplayPath(List<Vector3> dots)
    {
        pool.ResetPool(POOL_IDENTIFIER);

        foreach (Vector3 dotPosition in dots)
        {
            pool.Instantiate(POOL_IDENTIFIER, dotPosition, Quaternion.identity);
        }
    }
}
