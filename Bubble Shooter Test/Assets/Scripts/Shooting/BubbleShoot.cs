using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleShooter
{
    public class BubbleShoot : MonoBehaviour
    {
        public bool CanShoot => canShoot;

        [SerializeField] private Transform bubblePosition = null;
        [SerializeField] private GameObject bubblePrefab = null;
        [SerializeField] private RaycastPath raycastPath = null;
        [SerializeField] private List<BubbleData> bubbleTypes = null;
        [SerializeField] private float bubbleSpeed = 0.15f;

        private float shootProgress = 0;
        private float progressIncrement = 0;

        private bool canShoot = true;
        private GameObject firedBubble = null;
        private List<Vector3> targetPath = null;

        private void Start()
        {
            firedBubble = Instantiate(bubblePrefab, bubblePosition);
            firedBubble.transform.position = bubblePosition.position;

            BubbleData randomBubbleData = GetRandomBubbleData();
            Bubble bubble = firedBubble.GetComponent<Bubble>();
            bubble.InitializeBubble(bubbleSpeed, randomBubbleData);
        }

        // Update is called once per frame
        private void Update()
        {
            #region old code
            //if (firedBubble.gameObject.activeSelf)
            //{
            //    shootProgress += progressIncrement;

            //    if (shootProgress > 1)
            //    {
            //        targetPath.RemoveAt(0);

            //        if (targetPath.Count < 2)
            //        {
            //            //canShoot = true;
            //            firedBubble.gameObject.SetActive(false);
            //            return;
            //        }
            //        else
            //        {
            //            InitPath();
            //            canShoot = false;
            //        }
            //    }

            //    if (!bubblePrefab || targetPath == null) return;

            //    firedBubble.transform.position = Vector2.Lerp(targetPath[0], targetPath[1], shootProgress);
            //    return;
            //}
            #endregion

            Bubble bubble = firedBubble.GetComponent<Bubble>();

            if (bubble.IsMoving) return;

            if (Input.GetMouseButtonDown(0))
            {
                firedBubble.SetActive(true);
                if (!bubble.IsMoving)
                {
                    BubbleData randomBubbleData = GetRandomBubbleData();
                    bubble.InitializeBubble(bubbleSpeed, randomBubbleData);
                    firedBubble.transform.position = bubblePosition.position;
                }

            }

            if (Input.GetMouseButton(0))
            {
                raycastPath.GeneratePath(Input.mousePosition);
                targetPath = raycastPath.DotPositions;
            }

            if (Input.GetMouseButtonUp(0))
            {
                bubble.InitializePath(targetPath);

                Debug.Log("TEST");



                raycastPath.ResetPath(Input.mousePosition);

                //shootProgress = 0;
                //firedBubble.gameObject.SetActive(true);
                //firedBubble.transform.position = bubblePosition.position;
                //InitPath();

            }
        }

        private void InitPath()
        {
            Vector2 start = targetPath[0];
            Vector2 end = targetPath[1];
            float length = Vector2.Distance(start, end);
            float iterations = length / bubbleSpeed;
            shootProgress = 0f;
            progressIncrement = 1.0f / iterations;
        }

        private BubbleData GetRandomBubbleData()
        {
            if (bubbleTypes == null)
                return null;
            if (bubbleTypes.Count == 0)
                return null;

            int randomIndex = Random.Range(0, bubbleTypes.Count);
            return bubbleTypes[randomIndex];
        }
    }
}