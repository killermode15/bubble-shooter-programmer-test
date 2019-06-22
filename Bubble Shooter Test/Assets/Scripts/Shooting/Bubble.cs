using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleShooter
{

    public class Bubble : MonoBehaviour
    {
        public bool IsInitialized => isInitialized;
        public BubbleData BubbleData => bubbleData;

        [SerializeField] private BubbleData bubbleData  = null;
        
        [SerializeField] private bool isInitialized                      = false;
        private SpriteRenderer spriteRenderer           = null;
        private CircleCollider2D circleCollider         = null;

        private void Start()
        {
            circleCollider = GetComponent<CircleCollider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
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

            GetComponentInParent<BubbleGrid>().UpdateNeighbors();
        }

    }
}