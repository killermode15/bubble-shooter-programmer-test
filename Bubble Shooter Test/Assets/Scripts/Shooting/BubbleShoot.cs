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
        [SerializeField] private float mouseDistanceToShoot = 2.0f;

        private GameObject preppedBubble;
        private GameObject nextBubble;


        private void OnMouseDown()
        {
            SwitchBubbles();
        }

        private void SwitchBubbles()
        {
            GameObject currentBubble = preppedBubble;
            preppedBubble = nextBubble;

            preppedBubble.transform.SetParent(shootTransform);
            preppedBubble.transform.localPosition = Vector3.zero;
            preppedBubble.transform.localScale = bubblePrefab.transform.localScale;

            nextBubble = currentBubble;


            nextBubble.transform.SetParent(nextBubbleTransform);
            nextBubble.transform.localPosition = Vector3.zero;
            nextBubble.transform.localScale = new Vector3(1.8f, 1.8f, 1.8f);
        }

        private void Update()
        {
            if (!preppedBubble)
            {
                if (!nextBubble)
                {
                    nextBubble = GenerateBubble();
                }

                preppedBubble = nextBubble;
                preppedBubble.transform.SetParent(shootTransform);
                preppedBubble.transform.localPosition = Vector3.zero;
                preppedBubble.transform.localScale = bubblePrefab.transform.localScale;
                GetNextBubble();
            }

            if (preppedBubble && preppedBubble.GetComponent<BubbleBullet>().IsMoving) return;

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float mouseDist = Vector2.Distance(shootTransform.position, mousePos);
            if (mouseDist < mouseDistanceToShoot)
            {
                raycastPath.ResetPath();
                return;
            }

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
            nextBubble.transform.SetParent(nextBubbleTransform);
            nextBubble.transform.localPosition = Vector3.zero;
            nextBubble.transform.localScale = new Vector3(1.8f, 1.8f, 1.8f);
        }
    }
}