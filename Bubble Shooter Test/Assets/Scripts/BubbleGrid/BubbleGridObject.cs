using System.Collections;
using System.Collections.Generic;
using BubbleShooter;
using UnityEngine;

[System.Serializable]
public struct HexCoordinates
{
    public int X => x;
    public int Y => y;
    public int Z => -X - Y;

    public Vector3 VectorCoordinates => new Vector3(X, Y, Z);

    private int x, y;

    public HexCoordinates(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static HexCoordinates FromOffsetCoordinates(int x, int y)
    {
        return new HexCoordinates(x - y / 2, y);
    }

    public static bool operator ==(HexCoordinates a, HexCoordinates b)
    {
        return a.X == b.X &&
               a.Y == b.Y &&
               a.Z == b.Z;
    }

    public static bool operator !=(HexCoordinates a, HexCoordinates b)
    {
        return !(a == b);
    }
}

public class BubbleGridObject : MonoBehaviour
{
    public HexCoordinates Coordinates
    {
        get => coordinates;
        set => coordinates = value;
    }

    [SerializeField] private HexCoordinates coordinates;
    [SerializeField] private List<BubbleGridObject> neighbors = null;

    public void AddNeighbor(BubbleGridObject neighbor)
    {
        if (neighbor == null) return;
        if (neighbors.Contains(neighbor)) return;

        neighbors.Add(neighbor);
    }


    #region old code

    //public List<Transform> Neighbors => neighbors;

    //[SerializeField] private List<Transform> neighbors = new List<Transform>();


    //public void AddNeighbor(Transform neighbor)
    //{
    //    if (neighbors.Contains(neighbor)) return;
    //    neighbors.Add(neighbor);
    //}

    #endregion
}
