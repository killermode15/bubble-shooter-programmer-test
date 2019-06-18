using System;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleShooter
{
    public enum BubbleColor
    {
        Red = 0,
        Blue = 1,
        Green = 2,
        Violet = 3,
        Yellow = 4,
    }

    public class Bubble : MonoBehaviour
    {
        public BubbleData BubbleData => bubbleData;
        public bool IsMoving => isMoving;
        public bool IsInitialized => isInitialized;

        [SerializeField] private BubbleData bubbleData = null;

        private bool isInitialized = false;
        private bool isMoving = false;
        private float bubbleSpeed = 0.15f;
        private float pathProgress = 0;
        private float progressionSpeed = 0;
        private List<Vector3> path = null;
        private SpriteRenderer spriteRenderer = null;

        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (!gameObject.activeSelf || path == null) return;

            pathProgress += progressionSpeed;

            if (pathProgress > 1 && path.Count > 0)
            {
                path.RemoveAt(0);

                if (path.Count < 2)
                {
                    isMoving = false;
                    isInitialized = false;
                    return;
                }
                else
                {
                    InitializePath(path);
                }
            }

            if (path.Count == 0) return;
            transform.position = Vector2.Lerp(path[0], path[1], pathProgress);
        }

        public void InitializeBubble(float speed, BubbleData bubbleType)
        {
            if (!spriteRenderer)
                spriteRenderer = GetComponent<SpriteRenderer>();

            if (isInitialized) return;
            isInitialized = true;
            bubbleSpeed = speed;
            bubbleData = bubbleType;
            spriteRenderer.color = bubbleData.Color;
        }

        public void InitializePath(List<Vector3> newPath)
        {
            path = new List<Vector3>(newPath);

            if (newPath == null) throw new NullReferenceException("Path not found ");
            if (path.Count == 0) return;

            Vector2 start = path[0];
            Vector2 end = path[1];

            float length = Vector2.Distance(start, end);
            float iteration = length / bubbleSpeed;
            pathProgress = 0.0f;
            progressionSpeed = 1.0f / iteration;

            isMoving = true;
        }
    }
}