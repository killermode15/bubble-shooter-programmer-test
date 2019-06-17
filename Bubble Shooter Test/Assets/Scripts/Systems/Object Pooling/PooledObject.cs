using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleShooter
{
    public class PooledObject : MonoBehaviour
    {
        private string identifier;
        private Transform originalParent;

        public string Identifier
        {
            get => identifier;
            set => identifier = value;
        }

        public Transform OriginalParent
        {
            get => originalParent;
            set => originalParent = value;
        }
    }
}
