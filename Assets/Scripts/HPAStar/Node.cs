using UnityEngine;
using Priority_Queue;

namespace HPAStar
{
    public class Node : FastPriorityQueueNode
    {
        public int Index;

        public Vector2Int Position;

        public float GScore;
        public float FScore;


        public Node(int x, int y)
        {
            Index = MMH.Util.Map.CoordsToIndex(x, y);

            Position = new Vector2Int(x, y);

            GScore = Mathf.Infinity;
            FScore = Mathf.Infinity;
        }


        public override string ToString()
        {
            return $"{Index} : {Position.x},{Position.y}, G: {GScore} F: {FScore}";
        }
    }
}
