using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BubbleShooter;
using UnityEditor;
using UnityEngine;

public static class HexMetrics
{
    public const float OuterRadius = 1.0f;
    public const float InnerRadius = OuterRadius * 0.866025404f;
}

public class BubbleGrid : MonoBehaviour
{
    private const int HEX_NEIGHBOR_COUNT = 6;

    public List<BubbleGridCell> CellGrid => grid;

    [SerializeField] private GameObject bubblePrefab = null;
    [SerializeField] private int width = 10;
    [SerializeField] private int height = 10;
    [SerializeField] private float gapSize = 0.35f;
    [SerializeField] private LevelGrid level = null;

    private List<BubbleGridCell> grid;

    private void Start()
    {
        grid = new List<BubbleGridCell>();

        SetupGrid();
        SetupLevel();
        UpdateConnection();
        UpdateNeighbors();
    }

    private void SetupGrid()
    {
        for (int y = 0, i = 0; y < height; y++)
        {
            if (y % 2 == 0)
            {
                width += 1;
            }
            for (int x = 0; x < width; x++)
            {
                CreateCell(x, y, i++);
            }
            if (y % 2 == 0)
            {
                width -= 1;
            }
        }
    }

    private void SetupLevel()
    {
        if (!level) return;

        int originalWidth = width;
        int offset = 0;

        for (int y = 0; y < height; y++)
        {
            if (y % 2 == 0)
            {
                width += 1;
            }
            else
            {
                offset++;
            }
            for (int x = 0; x < width; x++)
            {
                if (y >= level.Grid.Count) continue;
                if (level.Grid[y] == null) continue;

                BubbleColor bubbleColor = level.Grid[y].BubbleDataList[x];
                BubbleData bubbleData = level.GetBubbleData(bubbleColor);
                if (!bubbleData) continue;

                int idx = y * originalWidth + x + offset;

                grid[idx].GetComponent<Bubble>().SetBubble(bubbleData);

            }
            if (y % 2 == 0)
            {
                width -= 1;
            }
        }
    }

    private void SetFirstRowConnection()
    {
        for (int x = 0; x < width + 1; x++)
        {
            grid[x].IsConnected = true;
        }
    }

    private void CreateCell(int x, int y, int i)
    {
        // Create the position of the cell
        Vector3 position;
        position.x = ((x + y * 0.5f - y / 2) * (HexMetrics.InnerRadius * 2.0f) * gapSize);
        position.z = 0f;
        position.y = y * (HexMetrics.OuterRadius * 1.5f) * gapSize * -1;

        // Create the cell using prefab
        GameObject cell = Instantiate(bubblePrefab);
        // Make cell transparent
        cell.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.15f);
        // Disable 
        cell.GetComponent<CircleCollider2D>().enabled = false;

        // Set the cell's layer same as the component's gameobject
        cell.layer = gameObject.layer;

        // Add the cell to the list of grid
        grid.Add(cell.GetComponent<BubbleGridCell>());

        // Set this as the parent of the cell
        cell.transform.SetParent(transform, false);
        // Set the position of the cell
        cell.transform.localPosition = position;
        // Create the hex coordinates of the cell
        grid[i].Coordinates = HexCoordinates.FromOffsetCoordinates(x, y);

        // Rename the cell to match its hex coordinates
        cell.name = "Cell [" + grid[i].Coordinates.X + "] [" +
                                grid[i].Coordinates.Y + "] [" +
                                grid[i].Coordinates.Z + "]";
    }


    /// <summary>
    /// Gets the surrounding cells of one cell
    /// </summary>
    /// <param name="cell"></param>
    private void GetNeighbors(BubbleGridCell cell)
    {
        if (!cell.GetComponent<Bubble>().BubbleData) return;

        HexCoordinates cellCoordinates = cell.Coordinates;

        cell.Neighbors.Clear();

        cell.AddNeighbor(FindCell(cell, 1, 0, -1));
        cell.AddNeighbor(FindCell(cell, 1, 0, -1));
        cell.AddNeighbor(FindCell(cell, 0, 1, -1));
        cell.AddNeighbor(FindCell(cell, -1, 1, 0));
        cell.AddNeighbor(FindCell(cell, -1, 0, 1));
        cell.AddNeighbor(FindCell(cell, 0, -1, 1));
        cell.AddNeighbor(FindCell(cell, 1, -1, 0));

        cell.Neighbors.RemoveAll(x => x == null);
    }

    /// <summary>
    /// Finds the cell based on the starting cell and an offset
    /// </summary>
    /// <param name="start"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    private BubbleGridCell FindCell(BubbleGridCell start, int x, int y, int z)
    {
        Vector3 checkCoordinate = new Vector3(start.Coordinates.X + x, start.Coordinates.Y + y, start.Coordinates.Z + z);
        BubbleGridCell foundCell = grid.Find(c => c.Coordinates.VectorCoordinates == checkCoordinate);
        if (!foundCell) return null;
        return foundCell.GetComponent<Bubble>().BubbleData ? foundCell : null;
    }

    /// <summary>
    /// Finds a cell based on the world position given
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <returns></returns>
    public BubbleGridCell FindCell(Vector2 worldPosition)
    {
        BubbleGridCell nearest = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector2 currentPosition = worldPosition;
        foreach (BubbleGridCell cell in grid)
        {
            if (cell.GetComponent<Bubble>().IsInitialized) continue;

            Vector2 directionToTarget = new Vector2(cell.transform.position.x, cell.transform.position.y) - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;

            if (!(dSqrToTarget < closestDistanceSqr)) continue;

            closestDistanceSqr = dSqrToTarget;
            nearest = cell;
        }

        return nearest;
    }

    public void ResetChecked()
    {
        foreach (BubbleGridCell bubble in grid)
        {
            bubble.IsChecked = false;
        }
    }

    public void UpdateConnection()
    {
        foreach(BubbleGridCell cell in grid)
        {
            cell.IsConnected = false;
        }

        SetFirstRowConnection();

        int originalWidth = width;
        int offset = 0;

        for (int y = 1; y < height-1; y++)
        {
            if (y % 2 == 0)
            {
                width += 1;
            }
            else
            {
                offset++;
            }
            for (int x = 0; x < width; x++)
            {
                int idx = y * originalWidth + x + offset;

                if (idx >= grid.Count) return;

                Bubble bubble = grid[idx].GetComponent<Bubble>();
                if (!bubble) continue;

                if(grid[idx].Neighbors.Any(n => n.IsConnected))
                {
                    grid[idx].IsConnected = true;
                }
            }
            if (y % 2 == 0)
            {
                width -= 1;
            }
        }


        /*
        foreach (BubbleGridCell cell in grid)
        {
            Bubble bubble = cell.GetComponent<Bubble>();

            if (!bubble) continue;

            foreach (BubbleGridCell neighbor in cell.Neighbors)
            {
                Bubble neighborBubble = neighbor.GetComponent<Bubble>();

                if (!neighborBubble) continue;

                if (neighborBubble.IsConnected)
                {
                    bubble.IsConnected = true;
                    break;
                }
            }
        }
        */
    }

    public void UpdateNeighbors()
    {
        foreach (BubbleGridCell cell in grid)
        {
            cell.Neighbors.Clear();
            GetNeighbors(cell);
        }
    }

    public void ResetDisconnectedCells()
    {
        foreach(BubbleGridCell cell in grid)
        {
            Bubble cellBubble = cell.GetComponent<Bubble>();
            if (!cellBubble) continue;

            cellBubble.ResetBubble();
        }


        //int originalWidth = width;
        //int offset = 0;

        //for (int y = 0; y < height; y++)
        //{
        //    if (y % 2 == 0)
        //    {
        //        width += 1;
        //    }
        //    else
        //    {
        //        offset++;
        //    }
        //    for (int x = 0; x < width; x++)
        //    {
        //        if (y >= level.Grid.Count) continue;
        //        if (level.Grid[y] == null) continue;

        //        BubbleColor bubbleColor = level.Grid[y].BubbleDataList[x];
        //        BubbleData bubbleData = level.GetBubbleData(bubbleColor);
        //        if (!bubbleData) continue;

        //        int idx = y * originalWidth + x + offset;

        //        grid[idx].GetComponent<Bubble>().SetBubble(bubbleData);

        //    }
        //    if (y % 2 == 0)
        //    {
        //        width -= 1;
        //    }
        //}

    }
}
