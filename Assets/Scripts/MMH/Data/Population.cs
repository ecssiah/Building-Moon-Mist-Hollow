using System;
using System.Collections.Generic;

namespace MMH.Data
{
    [Serializable]
    public class Population
    {
        public int NextId;

        public List<Component.Colonist> Colonists;
    }
}
