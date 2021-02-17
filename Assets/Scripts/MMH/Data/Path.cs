using System.Collections.Generic;
using Unity.Mathematics;

namespace MMH.Data
{
    public class Path
    {
        private int index;
        public int Index { get => index; set => index = value; }

        private List<int2> positions;
        public List<int2> Positions { get => positions; set => positions = value; }

        private float stepProgress;
        public float StepProgress { get => stepProgress; set => stepProgress = value; }

        public bool Active => index < positions.Count;


        public Path()
        {
            index = 0;
            positions = new List<int2>();

            stepProgress = 0f;
        }


        public override string ToString()
        {
            string result = $"Path:";

            foreach (int2 position in positions)
            {
                result += $" ({position.x},{position.y})";
            }

            return result;
        }
    }
}
