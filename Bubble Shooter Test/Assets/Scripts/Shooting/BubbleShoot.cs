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
        

        private bool canShoot = true;
        private GameObject firedBubble = null;
        private List<Vector3> targetPath = null;

        private void Start()
        {
            
        }

        // Update is called once per frame
        private void Update()
        {
            if (!firedBubble)
            {
                firedBubble = Instantiate(bubblePrefab);
                firedBubble.transform.SetParent(bubblePosition);
                firedBubble.transform.position = bubblePosition.position;

                raycastPath.GeneratePath(Input.mousePosition);
                targetPath = raycastPath.DotPositions;

                firedBubble.GetComponent<Bubble>().InitializePath(targetPath);
                raycastPath.ResetPath(Input.mousePosition);

                InitializeBubble();
            }

            Bubble bubble = firedBubble.GetComponent<Bubble>();

            if (bubble.IsMoving) return;

            if (!bubble.IsInitialized)
            {
                firedBubble.SetActive(true);
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
            Bubble bubble = firedBubble.GetComponent<Bubble>();
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
    }
}