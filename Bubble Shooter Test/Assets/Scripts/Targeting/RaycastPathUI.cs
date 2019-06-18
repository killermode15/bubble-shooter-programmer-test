using System.Collections;
using System.Collections.Generic;
using BubbleShooter;
using UnityEngine;

[RequireComponent(typeof(RaycastPath))]
public class RaycastPathUI : MonoBehaviour
{
    private const string POOL_IDENTIFIER = "Dots";

    // Reference to object pool
    [SerializeField] private Pool pool = null;
    // Gap size between the dots in the sub path
    [SerializeField] private float dotGapSize = 0.36f;
    // Maximum number of dots that can be displayed
    [SerializeField] private int maxDots = 15;
    // Max number of bounces to display
    [SerializeField] private int maxBounce = 2;
    
    private RaycastPath path;

    // Start is called before the first frame update
    private void Start()
    {
        path = GetComponent<RaycastPath>();

        float alpha = 1.0f / maxDots;
        float startAlpha = 1.0f;

        for (int i = 0; i < maxDots; i++)
        {
            GameObject dot = pool.Instantiate(POOL_IDENTIFIER);
            SpriteRenderer spriteRenderer = dot.GetComponent<SpriteRenderer>();

            Color spriteColor = spriteRenderer.color;

            spriteColor.a = startAlpha - alpha;
            startAlpha -= alpha;
            spriteRenderer.color = spriteColor;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        DisplayPath(path.DotPositions);
    }

    private void DisplayPath(List<Vector3> dots)
    {
        pool.ResetPool(POOL_IDENTIFIER);

        for (int i = 1; i < maxBounce + 1; i++)
        {
            DisplaySubPath(dots, i - 1, i);
        }
    }

    private void DisplaySubPath(List<Vector3> dots, int start, int end)
    {
        // If the start or end is higher or equal to the number of dot positions, return
        if (dots.Count <= start || dots.Count <= end)
            return;

        // Get the length of two main adjacent dots
        float pathLength = Vector2.Distance(dots[start], dots[end]);
        // Get the number of dots to display between the two main adjacent dots
        int numberOfDots = Mathf.RoundToInt((float)pathLength / dotGapSize);
        // Get the progression for the number of dots
        float dotProgress = 1.0f / numberOfDots;

        // Number of visible dots
        int dotCount = 0;

        for (float perc = 0; perc < 1; perc += dotProgress)
        {
            if (dotCount < pool.GetCount(POOL_IDENTIFIER) && dotCount < maxDots)
            {
                GameObject dot = pool.Instantiate(POOL_IDENTIFIER);
                dot.transform.position = Vector2.Lerp(dots[start], dots[end], perc);
                dotCount++;
            }
            perc += dotProgress;
        }
    }

}

