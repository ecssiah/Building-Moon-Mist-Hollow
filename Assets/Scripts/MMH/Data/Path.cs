using System.Collections.Generic;

namespace MMH
{
    namespace Data
    {
        public struct Path
        {
            public float Progress;
            public List<HPAStar.Node> Nodes;

            public bool Valid
            {
                get
                {
                    if (Nodes is null)
                    {
                        return false;
                    }

                    return Nodes.Count >= 1;
                }
            }


            public override string ToString()
            {
                string output = $"Path: {Valid}";

                if (Valid)
                {
                    for (int i = 0; i < Nodes.Count; i++)
                    {
                        output += $" - {Nodes[i].Position}";
                    }
                }

                return output;
            }
        }
    }
}
