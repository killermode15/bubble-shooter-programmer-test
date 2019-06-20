using UnityEngine;

namespace BubbleShooter
{
    public enum BubbleColor
    {
        Red = 0,
        Blue = 1,
        Green = 2,
        Violet = 3,
        Yellow = 4,
    }

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