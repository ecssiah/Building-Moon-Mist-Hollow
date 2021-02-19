using System.Collections.Generic;
using Unity.Mathematics;

namespace MMH.Data
{
    public class Path
    {
        private Stack<int2> positions;
        public Stack<int2> Positions { get => positions; set => positions = value; }

        private float stepProgress;
        public float StepProgress { get => stepProgress; set => stepProgress = value; }


        public Path()
        {
            stepProgress = 0f;
            positions = new Stack<int2>();
        }


        public override string ToString()
        {
            string result = "";

            foreach (int2 position in positions)
            {
                result += $" ({position.x},{position.y})";
            }

            return result;
        }
    }
}
