using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using BubbleShooter;
using Unity.Collections;
using UnityEngine;

public class MatchChecker : MonoBehaviour
{
    [SerializeField] private BubbleGrid grid = null;
    [SerializeField] private List<BubbleGridCell> matches;

    private void Start()
    {
        EventManager.Instance.OnBubbleHit = new Action<GameObject>(CheckGrid);
    }

    private void CheckGrid(GameObject bubble)
    {
        StartCoroutine(MatchCheckCR(bubble));
    }

    private IEnumerator MatchCheckCR(GameObject bubble)
    {
        if (!bubble) yield break;

        BubbleGridCell bubbleCell = bubble.GetComponent<BubbleGridCell>();

        if (!bubbleCell) yield break;

        grid.UpdateNeighbors();

        matches.Clear();
        
        matches.Add(bubbleCell);

        while (true)
        {
            yield return new WaitForEndOfFrame();

            bool visitedAll = true;

            for (int i = matches.Count - 1; i >= 0; i--)
            {
                if (matches[i].IsChecked) continue;
                matches[i].IsChecked = true;
                GetInitialMatch(matches[i]);
                matches = matches.Distinct().ToList();
                visitedAll = false;
            }

            if (!visitedAll) continue;
            if (matches.Count <= 2) continue;
            if (visitedAll) break;
        }

        foreach (BubbleGridCell cell in matches)
        {
            cell.GetComponent<Bubble>().ResetBubble();
        }
        grid.ResetChecked();
        grid.UpdateNeighbors();
        grid.UpdateConnection();
        CheckForDisconnected();
    }


    public void GetInitialMatch(BubbleGridCell bubbleCell)
    {
        foreach (BubbleGridCell neighbor in bubbleCell.Neighbors)
        {
            Bubble neighborBubble = neighbor.GetComponent<Bubble>();
            Bubble cellBubble = bubbleCell.GetComponent<Bubble>();

            if (neighborBubble?.BubbleData?.ColorType == cellBubble?.BubbleData?.ColorType)
            {
                Debug.Log(neighbor, neighbor);
                matches.Add(neighbor);
            }
        }
    }

    private void CheckForDisconnected()
    {
        foreach(BubbleGridCell cell in grid.CellGrid)
        {
            Bubble cellBubble = cell.GetComponent<Bubble>();

            if (!cellBubble) continue;

            if (cellBubble.IsConnected) continue;

            cellBubble.ResetBubble();
        }
    }
}
