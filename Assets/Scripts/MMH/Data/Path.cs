using System.Collections.Generic;
using Unity.Mathematics;

namespace MMH.Data
{
    public class Path
    {
        private int index;
        public int Index { get => index; set => index = value; }

        private float progress;
        public float Progress { get => progress; set => progress = value; }

        private List<int2> positions;
        public List<int2> Positions { get => positions; set => positions = value; }


        public Path()
        {
            index = 0;
            progress = 0f;

            positions = new List<int2>();
        }

    }
}
