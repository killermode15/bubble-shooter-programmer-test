using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleShooter
{

    [System.Serializable]
    public class LevelColumn
    {
        public List<BubbleColor> BubbleDataList
        {
            get => bubbleDataList;
            set => bubbleDataList = value;
        }

        [SerializeField] private string rowName;
        [SerializeField] private List<BubbleColor> bubbleDataList;

        public LevelColumn(int columnIdx)
        {
            rowName = "Column [" + columnIdx + "]";
            bubbleDataList = new List<BubbleColor>();
        }
    }

    [System.Serializable]
    [CreateAssetMenu]
    public class LevelGrid : ScriptableObject
    {

        public List<LevelColumn> Grid => grid;

        [SerializeField] private List<BubbleData> bubbleData;
        [SerializeField] private List<LevelColumn> grid;
        [SerializeField] private int height;
        [SerializeField] private int width;

        public BubbleData GetBubbleData(BubbleColor color)
        {
            return bubbleData.Find(x => x.ColorType == color);
        }

        [ContextMenu("Setup List")]
        public void SetupList()
        {
            for (int y = 0; y < height; y++)
            {
                grid.Add(new LevelColumn(y + 1));
                grid[y].BubbleDataList = new List<BubbleColor>();
                if (y % 2 == 0)
                {
                    width += 1;
                }
                for (int x = 0; x < width; x++)
                {
                    grid[y].BubbleDataList.Add(BubbleColor.Null);
                }
                if (y % 2 == 0)
                {
                    width -= 1;
                }
            }
        }
    }
}