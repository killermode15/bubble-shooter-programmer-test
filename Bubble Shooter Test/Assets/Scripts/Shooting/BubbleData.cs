using UnityEngine;

namespace BubbleShooter
{

    [System.Serializable]
    [CreateAssetMenu]
    public class BubbleData : ScriptableObject
    {
        public BubbleColor ColorType => colorType;
        public Color Color => color;

        [SerializeField] private BubbleColor colorType = BubbleColor.Red;
        [SerializeField] private Color color = Color.white;
    }

}