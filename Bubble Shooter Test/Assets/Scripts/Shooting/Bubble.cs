using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleShooter
{

    public class Bubble : MonoBehaviour
    {
        public bool IsInitialized => isInitialized;
        public bool BubbleData => bubbleData;

        [SerializeField] private BubbleData bubbleData  = null;

        private bool isInitialized                      = false;
        private SpriteRenderer spriteRenderer           = null;
        private CircleCollider2D circleCollider         = null;

        private void Start()
        {
            circleCollider = GetComponent<CircleCollider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            //circleCollider.enabled = false;
        }

        public void ResetBubble()
        {
            bubbleData = null;
            isInitialized = false;
            spriteRenderer.color = new Color(1, 1, 1, 0.15f);
            circleCollider.enabled = false;
        }

        public void SetBubble(BubbleData bubbleData)
        {
            // Return when bubble is already initialized
            if (isInitialized) return;

            // Set the bubble data
            this.bubbleData = bubbleData;
            // Set the bubble as initialized
            isInitialized = true;

            // Return if the bubble data is null and set initialized to false
            if (!bubbleData)
            { 
                isInitialized = false;
                return;
            }

            if (!spriteRenderer)
                spriteRenderer = GetComponent<SpriteRenderer>();

            // Set the bubble data color
            spriteRenderer.color = bubbleData.Color;

            if(!circleCollider)
                circleCollider = GetComponent<CircleCollider2D>();

            // Turn on the circleCollider
            circleCollider.enabled = true;
        }

        #region old code
        /*
        public BubbleData BubbleData => bubbleData;
        public bool IsMoving => isMoving;
        public bool IsInitialized => isInitialized;

        public bool IsStatic
        {
            get => isStatic;
            set => isStatic = value;
        }

        [SerializeField] private BubbleData bubbleData = null;

        private bool isStatic = false;
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

            if (!isInitialized)
            {
                isMoving = false;
                return;
            }

            if (path == null) return;

            pathProgress += progressionSpeed;

            if (pathProgress > 1 && path.Count > 0)
            {
                path.RemoveAt(0);

                if (path.Count < 2)
                {
                    OnPathEnd();
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

            gameObject.SetActive(true);
            isInitialized = true;
            bubbleSpeed = speed;
            bubbleData = bubbleType;
            spriteRenderer.color = bubbleData.Color;
        }

        public void InitializePath(List<Vector3> newPath)
        {

            if (newPath == null) throw new NullReferenceException("Path not found ");
            path = new List<Vector3>(newPath);
            if (path.Count == 0) return;

            GetComponent<CircleCollider2D>().enabled = true;

            Vector2 start = path[0];
            Vector2 end = path[1];

            float length = Vector2.Distance(start, end);
            float iteration = length / bubbleSpeed;
            pathProgress = 0.0f;
            progressionSpeed = 1.0f / iteration;

            isMoving = true;
            //Shoot();
        }

        private void OnPathEnd()
        {
            isMoving = false;
            isInitialized = false;
            transform.localPosition = Vector3.zero;
            GetComponent<CircleCollider2D>().enabled = false;
            path = null;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (isStatic || !isInitialized || !IsMoving) return;

            if (collision.gameObject.CompareTag("Top Wall") || collision.gameObject.CompareTag("Bubble"))
            {
                Destroy(gameObject);
                Vector2 hitPoint = collision.contacts[0].point;
                BubbleGridObject cell = FindObjectOfType<BubbleGrid>().FindCell(hitPoint);
                cell.GetComponent<CircleCollider2D>().enabled = true;

                Bubble bubbleScript = cell.GetComponent<Bubble>();
                bubbleScript.InitializeBubble(0, bubbleData);
                bubbleScript.isStatic = true;
            }
        }

        //private void OnTriggerEnter2D(Collider2D collision)
        //{
        //    if(collision.CompareTag("Top Wall"))
        //    {
        //        BubbleGridObject cell = FindObjectOfType<BubbleGrid>().FindCell(transform.position);
        //        cell.GetComponent<CircleCollider2D>().enabled = true;
        //        cell.GetComponent<CircleCollider2D>().isTrigger = true;

        //        Bubble bubbleScript = cell.GetComponent<Bubble>();
        //        bubbleScript.InitializeBubble(0, bubbleData);
        //        bubbleScript.isStatic = true;
        //        Debug.Log("TEST");
        //    }

        //    if (!collision.CompareTag("Bubble") || isStatic) return;

        //    Bubble bubble = collision.GetComponent<Bubble>();
        //    if (!bubble.IsInitialized) return;

        //    Destroy(gameObject);
        //    bubble.InitializeBubble(0, bubbleData);
        //}



        #region Test Code

        public void Shoot()
        {
            StartCoroutine(MoveBubbleCR());
        }

        private IEnumerator MoveBubbleCR()
        {
            if (path == null) yield break;
            if (path.Count == 0) yield break;
            if (!isInitialized) yield break;

            Vector3 start = path[0];
            Vector3 end = path[1];

            float length = Vector2.Distance(start, end);
            float iteration = length / bubbleSpeed;

            float pathProgress = 0;
            float progressionSpeed = 1.0f / iteration;

            while (pathProgress < 1 && path.Count > 2)
            {
                pathProgress += progressionSpeed;

                path.RemoveAt(0);

                if (path.Count < 2)
                {
                    OnPathEnd();
                    yield break;
                }
                else
                {
                    InitializePath(path);
                }

                if (path.Count == 0) yield break;

                transform.position = Vector2.Lerp(path[0], path[1], pathProgress);

                yield return new WaitForEndOfFrame();
            }
        }
        #endregion
        */
        #endregion
    }
}