using UnityEngine;
using Priority_Queue;
using System;
using Unity.Mathematics;

namespace HPAStar
{
    public class Node : FastPriorityQueueNode
    {
        public int Index;

        public int2 Position;

        public int GScore;
        public int FScore;


        public Node(int x, int y)
        {
            Index = MMH.Util.Map.CoordsToIndex(x, y);

            Position = new int2(x, y);

            GScore = int.MaxValue;
            FScore = int.MaxValue;
        }


        public override string ToString()
        {
            return $"{Index} : {Position.x},{Position.y}, G: {GScore} F: {FScore}";
        }
    }
}
