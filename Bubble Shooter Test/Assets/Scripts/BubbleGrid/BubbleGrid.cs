using System.Collections;
using System.Collections.Generic;
using System.Data;
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

    [SerializeField] private GameObject bubblePrefab = null;
    [SerializeField] private int width = 10;
    [SerializeField] private int height = 10;
    [SerializeField] private float gapSize = 0.35f;
    [SerializeField] private LevelGrid level = null;

    private List<BubbleGridObject> grid;

    private void Start()
    {
        grid = new List<BubbleGridObject>();

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

        foreach (BubbleGridObject cell in grid)
        {
            GetNeighbors(cell);
        }

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


        #region test
        /*
        for (int y = 0, i = 0; y < height; y++)
        {
            if (y % 2 == 0)
            {
                width ++;
                i++;
            }

            if (y % 3 == 0)
                i++;
            for (int x = 0; x < width; x++)
            {
                if (y >= level.Grid.Count) continue;
                if (level.Grid[y] == null) continue;
                BubbleColor bubbleColor = level.Grid[y].BubbleDataList[x];
                BubbleData bubbleData = level.GetBubbleData(bubbleColor);
                if (!bubbleData) continue;

                int xIdx = (y % 2 == 0) ? x : x + i;
                
                grid[(y * originalWidth) + xIdx].GetComponent<Bubble>().SetBubble(bubbleData);
            }
            if (y % 2 == 0)
            {
                width --;
            }
        }
        */
        #endregion
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
        grid.Add(cell.GetComponent<BubbleGridObject>());

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
    private void GetNeighbors(BubbleGridObject cell)
    {
        HexCoordinates cellCoordinates = cell.Coordinates;
        cell.AddNeighbor(FindCell(cell, 1, 0, -1));
        cell.AddNeighbor(FindCell(cell, 0, 1, -1));
        cell.AddNeighbor(FindCell(cell, -1, 1, 0));
        cell.AddNeighbor(FindCell(cell, -1, 0, 1));
        cell.AddNeighbor(FindCell(cell, 0, -1, 1));
        cell.AddNeighbor(FindCell(cell, 1, -1, 0));
    }

    /// <summary>
    /// Finds the cell based on the starting cell and an offset
    /// </summary>
    /// <param name="start"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    private BubbleGridObject FindCell(BubbleGridObject start, int x, int y, int z)
    {
        Vector3 checkCoordinate = new Vector3(start.Coordinates.X + x, start.Coordinates.Y + y, start.Coordinates.Z + z);
        BubbleGridObject foundCell = grid.Find(c => c.Coordinates.VectorCoordinates == checkCoordinate);
        return foundCell;
    }

    /// <summary>
    /// Finds a cell based on the world position given
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <returns></returns>
    public BubbleGridObject FindCell(Vector2 worldPosition)
    {
        BubbleGridObject nearest = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector2 currentPosition = worldPosition;
        foreach (BubbleGridObject cell in grid)
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

    #region old code

    //private const float HEX_ANGLE = 26.18f;

    //[SerializeField] private GameObject bubblePrefab = null;
    //[SerializeField] private float row = 10;
    //[SerializeField] private float column = 10;
    //[SerializeField] private float gapSize = 0.35f;

    //private List<List<Transform>> grid;

    //private void Start()
    //{


    //    grid = new List<List<Transform>>();

    //    SetupGrid();
    //}

    //private void Update()
    //{


    //    FindNeighbors();
    //}

    //private void SetupGrid()
    //{
    //    for (int y = 0; y < column + 1; y++)
    //    {
    //        grid.Add(new List<Transform>());
    //        for (int x = 0; x < row; x++)
    //        {
    //            if (grid[x] == null)
    //                grid[x] = new List<Transform>();

    //            GameObject gridHolder = Instantiate(bubblePrefab, transform);
    //            gridHolder.name = "Point [" + x + "] [" + y + "]";
    //            gridHolder.GetComponent<CircleCollider2D>().enabled = true;

    //            float newX = (y % 2 == 0) ? (x - (gapSize / 2)) * gapSize : x * gapSize;

    //            gridHolder.transform.position = new Vector3(newX, y * gapSize, 0);

    //            grid[x].Add(gridHolder.transform);
    //        }
    //    }
    //}

    //private void FindNeighbors()
    //{
    //    for (int y = 0; y < column + 1; y++)
    //    {
    //        for (int x = 0; x < row; x++)
    //        {
    //            Transform gameObject = grid[x][y];
    //            if (!gameObject) return;
    //            BubbleGridObject gridObject = gameObject.GetComponent<BubbleGridObject>();
    //            if (!gridObject) return;

    //            for (int i = 0; i < 6; i++)
    //            {
    //                float radius = gapSize;

    //                float newX = radius * Mathf.Cos(HEX_ANGLE * i);
    //                float newY = radius * Mathf.Sin(HEX_ANGLE * i);

    //                Vector2 newPosition = new Vector3(newX, newY) + gameObject.position;
    //                Vector2 direction = newPosition - new Vector2(gameObject.position.x, gameObject.position.y);

    //                Debug.DrawRay(gameObject.position, direction);

    //                RaycastHit2D hit = Physics2D.Raycast(newPosition, direction);

    //                if (!hit.collider) continue;

    //                BubbleGridObject detectedBubble = hit.collider.GetComponent<BubbleGridObject>();
    //                if (!detectedBubble) continue;

    //                gridObject.AddNeighbor(detectedBubble.transform);
    //            }


    //        }
    //    }
    //}

    #endregion
}
