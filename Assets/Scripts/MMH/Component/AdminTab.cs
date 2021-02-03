using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace MMH.Component
{
    public class AdminTab : Tab
    {
        private System.PathfindingSystem pathfindingSystem;

        private GameObject testButtonObject;

        private Button testButton;

        public override void Awake()
        {
            Active = false;

            pathfindingSystem = GameObject.Find("Pathfinding System").GetComponent<System.PathfindingSystem>();

            testButtonObject = transform.Find("Test Button").gameObject;
            testButton = testButtonObject.GetComponent<Button>();

            testButton.onClick.AddListener(delegate { OnTestButtonClick(); });
        }


        private void OnTestButtonClick()
        {
            Data.Path pathData = pathfindingSystem.FindPath(new int2(0, 0), new int2(2, 0));

            print("Is this the path yo?");
            print(pathData);
        }
    }
}