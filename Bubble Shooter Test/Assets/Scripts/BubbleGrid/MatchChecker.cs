using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using BubbleShooter;
using UnityEngine;

public class MatchChecker : MonoBehaviour
{
    [SerializeField] private BubbleGrid grid = null;
    [SerializeField] private List<BubbleGridObject> bubbles;

    private void Start()
    {
        EventManager.Instance.OnBubbleHit = new Action<GameObject>(CheckGrid);
    }

    private void CheckGrid(GameObject bubble)
    {
        // TODO: Recursive check for matching neighbors
        //bubbles = CheckForMatch(bubble.GetComponent<BubbleGridObject>(), new List<BubbleGridObject>());

        //foreach (BubbleGridObject matchedBubbles in bubbles)
        //{
        //    if (!matchedBubbles) continue;
        //    Bubble bubbleScript = matchedBubbles.GetComponent<Bubble>();
        //    bubbleScript.ResetBubble();
        //}
    }

    public List<BubbleGridObject> CheckForMatch(BubbleGridObject bubbleCell, List<BubbleGridObject> foundNeighbors = null)
    {
        if (foundNeighbors == null)
            foundNeighbors = new List<BubbleGridObject>();

        List<BubbleGridObject> neighbors = bubbleCell.Neighbors;

        foreach (BubbleGridObject neighbor in neighbors)
        {
            if (neighbor.IsChecked) continue;

            // If the neighbor has no neighbors, it's detached and should be resetted
            if (neighbor.Neighbors.Count == 0)
            {
                neighbor.GetComponent<Bubble>().ResetBubble();
                continue;
            }
            

            Bubble neighborBubble = neighbor.GetComponent<Bubble>();
            Bubble bubble = bubbleCell.GetComponent<Bubble>();

            // If the neighbor and the start cell is the same add it to the list
            if (bubble.BubbleData == neighborBubble.BubbleData)
            {
                neighbor.IsChecked = true;
                foundNeighbors.Add(neighbor);
            }
            else
            {
                continue;
            }

            // Get the neighbor of the neighbor
            List<BubbleGridObject> childNeighbors = new List<BubbleGridObject>(neighbor.Neighbors);
            // Remove this cell from the child neighbors
            childNeighbors.Remove(bubbleCell);

            foreach (BubbleGridObject childNeighbor in childNeighbors)
            {

                List<BubbleGridObject> foundChildNeighbors = CheckForMatch(childNeighbor, foundNeighbors);

                for (int i = 0; i < foundChildNeighbors.Count; i++)
                {
                    Bubble childBubble = foundChildNeighbors[i].GetComponent<Bubble>();

                    if (bubble.BubbleData != childBubble.BubbleData) continue;

                    foundNeighbors.Add(foundChildNeighbors[i]);
                    //Debug.Log(foundChildNeighbors[i], foundChildNeighbors[i]);
                }
            }

            return foundNeighbors;

        }


        #region old code
        //if (bubbleCell.Neighbors.Count == 0) return foundNeighbors;

        //foreach (BubbleGridObject neighbor in bubbleCell.Neighbors)
        //{
        //    if (!neighbor) continue;
        //    Bubble neighborBubble = neighbor.GetComponent<Bubble>();
        //    Bubble mainBubble = bubbleCell.GetComponent<Bubble>();

        //    if (!mainBubble) continue;
        //    if (!neighborBubble) continue;

        //    if (mainBubble.BubbleData != neighborBubble.BubbleData) continue;

        //    if (foundNeighbors.Contains(neighbor)) continue;

        //    foundNeighbors.Add(neighbor);
        //}

        //foreach (BubbleGridObject neighbor in foundNeighbors)
        //{
        //    List<BubbleGridObject> neighbors = CheckForMatch(neighbor);

        //    foreach (BubbleGridObject childNeighbors in neighbors)
        //    {
        //        if (foundNeighbors.Contains(childNeighbors)) continue;
        //        foundNeighbors.Add(childNeighbors);
        //    }
        //}
        #endregion

        return foundNeighbors;
    }

}
