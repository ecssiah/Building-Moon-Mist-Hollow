using UnityEngine;
using Priority_Queue;
using System;

namespace HPAStar
{
    public class Node : FastPriorityQueueNode
    {
        public int Index;

        public Vector2Int Position;

        public int GScore;
        public int FScore;


        public Node(int x, int y)
        {
            Index = MMH.Util.Map.CoordsToIndex(x, y);

            Position = new Vector2Int(x, y);

            GScore = int.MaxValue;
            FScore = int.MaxValue;
        }


        public override string ToString()
        {
            return $"{Index} : {Position.x},{Position.y}, G: {GScore} F: {FScore}";
        }
    }
}
