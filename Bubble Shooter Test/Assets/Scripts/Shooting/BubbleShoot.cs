using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BubbleShooter
{
    public class BubbleShoot : MonoBehaviour
    {
        [SerializeField] private Transform shootTransform = null;
        [SerializeField] private Transform nextBubbleTransform = null;
        [SerializeField] private GameObject bubblePrefab = null;
        [SerializeField] private RaycastPath raycastPath = null;
        [SerializeField] private List<BubbleData> bubbleDatas = null;
        [SerializeField] private float shootSpeed = 0.15f;

        private GameObject preppedBubble;
        private GameObject nextBubble;

        private void Update()
        {
            if (!preppedBubble)
            {
                preppedBubble = GenerateBubble();
                //GetNextBubble();
            }

            if (preppedBubble && preppedBubble.GetComponent<BubbleBullet>().IsMoving) return;

            if (Input.GetMouseButton(0))
            {
                raycastPath.GeneratePath(Input.mousePosition);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                Shoot();
                raycastPath.ResetPath();
            }
        }

        private void Shoot()
        {
            // Return if the bubble prefab is not set
            if (!bubblePrefab) throw new NullReferenceException("Bubble bullet prefab is null");

            if (!preppedBubble) throw new NullReferenceException("Fired bubble is null");


            BubbleBullet bubbleBullet = preppedBubble?.GetComponent<BubbleBullet>();

            // Return if bubbleBullet is null
            if (!bubbleBullet) throw new NullReferenceException("Bubble bullet game object is null");

            List<Vector3> path = raycastPath.DotPositions;

            // Return if path is null or has nothing
            if (path == null || path?.Count == 0) throw new NullReferenceException("Path has no content or is null");

            bubbleBullet.Initialize(shootSpeed, path);
            bubbleBullet.Shoot();
        }

        private BubbleData GetRandomBubbleData()
        {
            if (bubbleDatas == null)
                return null;
            if (bubbleDatas.Count == 0)
                return null;

            int randomIndex = Random.Range(0, bubbleDatas.Count);
            return bubbleDatas[randomIndex];
        }

        private GameObject GenerateBubble()
        {
            GameObject bubble = Instantiate(bubblePrefab);
            // Set the bubble's parent as the start position;
            bubble.transform.SetParent(shootTransform);
            bubble.transform.localPosition = Vector3.zero;

            BubbleData bubbleData = GetRandomBubbleData();

            // Return if no bubble data is generated
            if (!bubbleData) throw new NullReferenceException("No bubble data generated");

            bubble.GetComponent<BubbleBullet>().SetBubbleData(bubbleData);

            return bubble;
        }

        private void GetNextBubble()
        {
            if (!nextBubbleTransform) throw new NullReferenceException("Next Bubble transform is null");

            nextBubble = GenerateBubble();
        }

        /*
        public bool CanShoot => canShoot;

        [SerializeField] private Transform bubblePosition = null;
        [SerializeField] private GameObject bubblePrefab = null;
        [SerializeField] private RaycastPath raycastPath = null;
        [SerializeField] private List<BubbleData> bubbleTypes = null;
        [SerializeField] private float bubbleSpeed = 0.15f;
        

        private bool canShoot = true;
        private GameObject preppedBubble = null;
        private List<Vector3> targetPath = null;

        private void Start()
        {
            
        }

        // Update is called once per frame
        private void Update()
        {
            if (!preppedBubble)
            {
                preppedBubble = Instantiate(bubblePrefab);
                preppedBubble.transform.SetParent(bubblePosition);
                preppedBubble.transform.position = bubblePosition.position;

                raycastPath.GeneratePath(Input.mousePosition);
                targetPath = raycastPath.DotPositions;

                preppedBubble.GetComponent<Bubble>().InitializePath(targetPath);
                raycastPath.ResetPath(Input.mousePosition);

                InitializeBubble();
            }

            Bubble bubble = preppedBubble.GetComponent<Bubble>();

            if (bubble.IsMoving) return;

            if (!bubble.IsInitialized)
            {
                preppedBubble.SetActive(true);
                if (!bubble.IsMoving)
                {
                    InitializeBubble();
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
                raycastPath.ResetPath(Input.mousePosition);

            }
        }

        private void InitializeBubble()
        {
            BubbleData randomBubbleData = GetRandomBubbleData();
            Bubble bubble = preppedBubble.GetComponent<Bubble>();
            bubble.InitializeBubble(bubbleSpeed, randomBubbleData);
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
        */
    }
}