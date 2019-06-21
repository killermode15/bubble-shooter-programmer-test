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
     
    public override bool Equals(object obj)
    {
        if (!(obj is HexCoordinates))
        {
            return false;
        }

        var coordinates = (HexCoordinates)obj;
        return X == coordinates.X &&
               Y == coordinates.Y &&
               Z == coordinates.Z;
    }

    public override int GetHashCode()
    {
        var hashCode = -307843816;
        hashCode = hashCode * -1521134295 + X.GetHashCode();
        hashCode = hashCode * -1521134295 + Y.GetHashCode();
        hashCode = hashCode * -1521134295 + Z.GetHashCode();
        return hashCode;
    }
}

public class BubbleGridCell : MonoBehaviour
{
    public HexCoordinates Coordinates
    {
        get => coordinates;
        set => coordinates = value;
    }

    public bool IsChecked
    {
        get => isChecked;
        set => isChecked = value;
    }
    public List<BubbleGridCell> Neighbors => neighbors;

    [SerializeField] private HexCoordinates coordinates;
    [SerializeField] private List<BubbleGridCell> neighbors = null;
    [SerializeField] private bool isChecked = false;

    public void AddNeighbor(BubbleGridCell neighbor)
    {
        if (neighbor == null) return;
        if (neighbors.Contains(neighbor)) return;

        neighbors.Add(neighbor);
    }
}
