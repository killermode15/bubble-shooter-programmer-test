using System.Collections;
using System.Collections.Generic;
using BubbleShooter;
using UnityEngine;

[RequireComponent(typeof(RaycastPath))]
public class RaycastPathUI : MonoBehaviour
{
    private const string POOL_IDENTIFIER = "Dots";

    [SerializeField] private Pool pool;
    [SerializeField] private float dotGapSize;
    [SerializeField] private int maxDots;

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
        //pool.ResetPool(POOL_IDENTIFIER);

        //float alpha = 1.0f / maxDots;
        //float startAlpha = 1.0f;

        //foreach (Vector3 dotPosition in dots)
        foreach (Vector3 dotPosition in dots)
        {
            //if (i > dots.Count) break;

            //Debug.Log(i + " - " + pool.GetCount(POOL_IDENTIFIER));

            GameObject dot = pool.Instantiate(POOL_IDENTIFIER, dotPosition, Quaternion.identity);
            //SpriteRenderer spriteRenderer = dot.GetComponent<SpriteRenderer>();

            //Color spriteColor = spriteRenderer.color;

            //spriteColor.a = startAlpha - alpha;
            //startAlpha -= alpha;
            //spriteRenderer.color = spriteColor;
        }
    }

    //private void DiplaySubPath(List<Vector3> dots, int start, int end)
    //{
    //    if (dots.Count < end)
    //        return;

    //    float pathLength = Vector2.Distance(dots[start], dots[end]);
    //    int numberOfDots = Mathf.RoundToInt((float) pathLength / dotGapSize);
    //    float dotProgress = 1.0f / numberOfDots;

    //    int dotCount = 0;

    //    for (float perc = 0; perc < 1; perc += dotProgress)
    //    {
    //        float nextX = dots[start].x + perc * (dots[end].x - dots[start].x);
    //        float nextY = dots[start].y + perc * (dots[end].y - dots[start].y);

    //        if (dotCount < pool.GetCount(POOL_IDENTIFIER))
    //        {
    //            GameObject dot = pool.Instantiate(POOL_IDENTIFIER, new Vector2(nextX, nextY), Quaternion.identity);
    //            dotCount++;
    //        }

    //        perc += dotProgress;
    //    }
    //}

}

