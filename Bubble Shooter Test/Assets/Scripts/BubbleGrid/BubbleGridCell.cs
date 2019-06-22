using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public List<BubbleGridCell> SameNeighbors => sameNeighbors.Distinct().ToList();
    public bool IsConnected
    {
        get => isConnected;
        set => isConnected = value;
    }

    [SerializeField] private bool isChecked = false;
    [SerializeField] private bool isConnected;
    [SerializeField] private HexCoordinates coordinates;
    [SerializeField] private List<BubbleGridCell> neighbors = new List<BubbleGridCell>();
    [SerializeField] private List<BubbleGridCell> sameNeighbors = new List<BubbleGridCell>();

    public void AddNeighbor(BubbleGridCell neighbor)
    {
        if (neighbor == null) return;
        if (neighbors.Contains(neighbor)) return;

        Bubble bubble = GetComponent<Bubble>();
        Bubble neighborBubble = neighbor.GetComponent<Bubble>();

        if (bubble?.BubbleData?.ColorType == neighborBubble?.BubbleData?.ColorType)
        {
            sameNeighbors.Add(neighbor);
        }


        neighbors.Add(neighbor);
    }

    public List<BubbleGridCell> GetSameNeighbors(List<BubbleGridCell> exceptionList = null)
    {
        if (exceptionList == null) return sameNeighbors;

        return sameNeighbors.Except(exceptionList).ToList();
    }

    public void ResetCell()
    {
        Bubble cellBubble = GetComponent<Bubble>();

        if (!cellBubble) return;

        cellBubble.ResetBubble();

        for(int i = 0; i< sameNeighbors.Count; i++)
        {
            Bubble neighborBubble = sameNeighbors[i].GetComponent<Bubble>();
            if (!neighborBubble) continue;

            if (neighborBubble.IsInitialized)
            {
                sameNeighbors[i].ResetCell();
            }
        }

        sameNeighbors.Clear();
    }
}
