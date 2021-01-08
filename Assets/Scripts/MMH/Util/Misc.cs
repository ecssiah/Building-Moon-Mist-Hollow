using System;
using Random = UnityEngine.Random;

namespace MMH
{
    namespace Util
    {
        public struct Misc
        {
            public static T RandomEnumValue<T>()
            {
                Array values = Enum.GetValues(typeof(T));

                return (T)values.GetValue(Random.Range(0, values.Length));
            }
        }
    }
}