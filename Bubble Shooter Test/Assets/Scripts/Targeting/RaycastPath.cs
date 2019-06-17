using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace BubbleShooter
{
    public class RaycastPath : MonoBehaviour
    {
        private const string POOL_IDENTIFIER = "Dots";
        private const string WALL_TAG = "Side Wall";

        public List<Vector3> DotPositions
        {
            get => dotPositions;
            private set => dotPositions = value;
        }

        [SerializeField] private Pool pool;
        [SerializeField] private Transform bubblePosition;

        [SerializeField] private GameObject[] colorsGameObject;

        private List<Vector3> dotPositions;

        // Start is called before the first frame update
        private void Start()
        {
            dotPositions = new List<Vector3>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (!pool.IsInitialized(POOL_IDENTIFIER) || dotPositions == null)
                return;

            if (Input.GetMouseButtonDown(0))
            {
                HandleTouchDown(Input.mousePosition);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                HandleTouchUp(Input.mousePosition);
            }
            else if (Input.GetMouseButton(0))
            {
                HandleTouchMove(Input.mousePosition);
            }
        }

        private void HandleTouchDown(Vector2 touch)
        {

        }

        private void HandleTouchUp(Vector2 touch)
        {
            if (dotPositions == null || dotPositions.Count < 2)
                return;

            dotPositions.Clear();
            pool.ResetPool(POOL_IDENTIFIER);
        }

        private void HandleTouchMove(Vector2 touch)
        {
            if (dotPositions == null)
                return;

            dotPositions.Clear();
            Vector2 point = Camera.main.ScreenToWorldPoint(touch);
            if (!bubblePosition)
                return;

            Vector2 direction = new Vector2(point.x - bubblePosition.position.x, point.y - bubblePosition.position.y);
            RaycastHit2D hit = Physics2D.Raycast(bubblePosition.position, direction);
            if (!hit.collider)
                return;

            if (hit.collider.CompareTag(WALL_TAG))
            {
                CalculatePath(hit, direction);
                return;
            }

            dotPositions.Add(hit.point);
            //DrawPath(dotPositions);


        }

        private void CalculatePath(RaycastHit2D previousHit, Vector2 directionIn)
        {
            while (true)
            {
                if (dotPositions.Count > 30)
                    return;

                // Add the previous hit to the list of dot positions
                dotPositions.Add(previousHit.point);

                // Calculate the tangent of the previous hit's normal
                float normal = Mathf.Atan2(previousHit.normal.y, previousHit.normal.x);
                // Calculate new direction
                float newDir = normal + (normal - Mathf.Atan2(directionIn.y, directionIn.x));
                // Calculate the reflection vector
                Vector2 reflection = new Vector2(-Mathf.Cos(newDir), -Mathf.Sin(newDir));
                // Create new raycast start point
                Vector2 newCastPoint = previousHit.point + (2 * reflection);

                RaycastHit2D hit = Physics2D.Raycast(newCastPoint, reflection);

                //if (!hit.collider)
                //{
                //    DrawPath(dotPositions);
                //    break;
                //}
                if (!hit.collider) break;

                if (hit.collider.CompareTag(WALL_TAG))
                {
                    previousHit = hit;
                    directionIn = reflection;
                    continue;
                }
                else
                {
                    dotPositions.Add(hit.point);
                    //DrawPath(dotPositions);
                }

                break;
            }
        }

        private void DrawPath(List<Vector3> dots)
        {
            pool.ResetPool(POOL_IDENTIFIER);

            foreach (Vector3 dotPosition in dots)
            {
                pool.Instantiate(POOL_IDENTIFIER, dotPosition, Quaternion.identity);
            }
        }
    }
}