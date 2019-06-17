using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleShooter
{
    [CreateAssetMenu(fileName = "New Object Pool Data")]
    [System.Serializable]
    public class ObjectData : ScriptableObject
    {
        [SerializeField] private string identifier;
        [SerializeField] private GameObject gameObject;
        [SerializeField] private int poolAmount;

        public string Identifier
        {
            get => identifier;
            set => identifier = value;
        }

        public GameObject Object
        {
            get => gameObject;
            set => gameObject = value;
        }

        public int PoolAmount
        {
            get => poolAmount;
            set => poolAmount = value;
        }
    }
}
