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

        public Node Previous;


        public override string ToString()
        {
            return $"{Index} : {Position.x},{Position.y}, G: {GScore} F: {FScore}";
        }
    }
}
