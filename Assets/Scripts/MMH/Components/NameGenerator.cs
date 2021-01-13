using UnityEngine;

namespace MMH
{
    public static class NameGenerator
    {
        public static string GetName(Type.Group groupType)
        {
            string initial = "";
            string middle = "";
            string final = "";

            int initialIndex;
            int middleIndex;
            int finalIndex;

            switch (groupType)
            {
                case Type.Group.Guy:
                    initialIndex = Random.Range(0, Info.Name.GuyInitialElements.Length);
                    middleIndex = Random.Range(0, Info.Name.GuyMiddleElements.Length);
                    finalIndex = Random.Range(0, Info.Name.GuyFinalElements.Length);

                    initial = Info.Name.GuyInitialElements[initialIndex];
                    middle = Info.Name.GuyMiddleElements[middleIndex];
                    final = Info.Name.GuyFinalElements[finalIndex];

                    break;
                case Type.Group.Kailt:
                    initialIndex = Random.Range(0, Info.Name.KailtInitialElements.Length);
                    middleIndex = Random.Range(0, Info.Name.KailtMiddleElements.Length);
                    finalIndex = Random.Range(0, Info.Name.KailtFinalElements.Length);

                    initial = Info.Name.KailtInitialElements[initialIndex];
                    middle = Info.Name.KailtMiddleElements[middleIndex];
                    final = Info.Name.KailtFinalElements[finalIndex];

                    break;
                case Type.Group.Taylor:
                    initialIndex = Random.Range(0, Info.Name.TaylorInitialElements.Length);
                    middleIndex = Random.Range(0, Info.Name.TaylorMiddleElements.Length);
                    finalIndex = Random.Range(0, Info.Name.TaylorFinalElements.Length);

                    initial = Info.Name.TaylorInitialElements[initialIndex];
                    middle = Info.Name.TaylorMiddleElements[middleIndex];
                    final = Info.Name.TaylorFinalElements[finalIndex];

                    break;
            }

            return $"{initial}{middle}{final}";
        }
    }
}