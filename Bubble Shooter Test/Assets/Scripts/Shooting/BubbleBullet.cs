using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace BubbleShooter
{
    public class BubbleBullet : MonoBehaviour
    {
        public bool IsMoving => isMoving;
        public bool IsInitialized => isInitialized;

        [SerializeField] private BubbleData bubbleData = null;
        [SerializeField] private BubbleGrid grid = null;

        private bool isMoving = false;
        private bool isInitialized = false;
        private bool hasGeneratedBubble = false;
        private float bubbleSpeed = 0.25f;
        private List<Vector3> path = new List<Vector3>();
        private SpriteRenderer spriteRenderer = null;
        private CircleCollider2D circleCollider = null;

        // Start is called before the first frame update
        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            circleCollider = GetComponent<CircleCollider2D>();
            grid = FindObjectOfType<BubbleGrid>();
        }

        // Update is called once per frame
        private void Update()
        {

        }

        public void SetBubbleData(BubbleData bubbleData)
        {
            if (!bubbleData) throw new NullReferenceException("Bubble Data is null");

            // Set the new bubble data
            this.bubbleData = bubbleData;

            // Get SpriteRenderer component if spriteRenderer is null
            if (spriteRenderer == null)
                spriteRenderer = GetComponent<SpriteRenderer>();

            // Set the color of the bubble
            spriteRenderer.color = this.bubbleData.Color;
        }

        public void Initialize(float speed, List<Vector3> newPath)
        {
            // Return if bubble is already initialized
            if (isInitialized) return;

            // Set the bubble as initialized
            isInitialized = true;
            // Set the bubble speed
            bubbleSpeed = speed;

            // Enable collider
            circleCollider.enabled = true;


            // Return if newPath is null
            if (newPath == null) return;
            // Return if the newPath count is 0
            if (newPath.Count == 0) return;

            path = new List<Vector3>(newPath);
        }

        public void Shoot()
        {
            StartCoroutine(BulletUpdateCR());
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!isInitialized) throw new WarningException("Bubble bullet is not initialized");

            GameObject hit = other.gameObject;

            if (!hit) return;

            if(hit.CompareTag("Bottom Wall"))
            {
                Destroy(gameObject);
            }

            if (!(hit.CompareTag("Top Wall") || hit.CompareTag("Bubble"))) return;

            if (hasGeneratedBubble) return;

            hasGeneratedBubble = true;
            Vector2 hitPoint = new Vector2(transform.position.x, transform.position.y);
            BubbleGridCell cell = grid.FindCell(hitPoint);
            cell.GetComponent<CircleCollider2D>().enabled = true;
            cell.GetComponent<BubbleGridCell>().IsConnected = true;

            Bubble bubbleScript = cell.GetComponent<Bubble>();
            bubbleScript.SetBubble(bubbleData);

            Destroy(gameObject);
            if (!cell) return;

            EventManager.Instance.OnBubbleHit.Invoke(cell.gameObject);


        }

        private void OnPathEnd()
        {
            path = null;
        }

        private float CalculateProgressionSpeed()
        {
            Vector2 start = path[0];
            Vector2 end = path[1];

            float length = Vector2.Distance(start, end);
            float iteration = length / bubbleSpeed;
            float progressionSpeed = 1.0f / iteration;

            return progressionSpeed;
        }

        private IEnumerator BulletUpdateCR()
        {
            // Stop coroutine if the bubble bullet is not initialized
            if (!isInitialized)
            {
                isMoving = false;
                yield break;
            }

            // If the path is null, stop the coroutine
            if (path == null) yield break;

            float pathProgress = 0;
            float progressionSpeed = CalculateProgressionSpeed();

            isMoving = true;

            while (true)
            {
                pathProgress += progressionSpeed;

                // If the current path is finished and there is another path
                if (pathProgress > 1 && path.Count > 0)
                {
                    // Remove the first path position
                    path.RemoveAt(0);

                    // Stop coroutine if the remaining path is less than 2
                    if (path.Count < 2)
                    {
                        OnPathEnd();
                        break;
                    }
                    else
                    {
                        //  Get new progression speed after adjusting path
                        progressionSpeed = CalculateProgressionSpeed();
                        // Set the path progress back to 0
                        pathProgress = 0;
                    }
                }

                if (path.Count == 0) break;

                transform.position = Vector2.Lerp(path[0], path[1], pathProgress);

                yield return new WaitForEndOfFrame();
            }

            isMoving = false;
        }
    }
}