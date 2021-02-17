using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace MMH.Component
{
    public class AdminTab : Tab
    {
        private GameObject testButtonObject;

        private Button testButton;

        public override void Awake()
        {
            Active = false;

            testButtonObject = transform.Find("Test Button").gameObject;
            testButton = testButtonObject.GetComponent<Button>();

            testButton.onClick.AddListener(delegate { OnTestButtonClick(); });
        }


        private void OnTestButtonClick()
        {
            print("Test Button Clicked");
        }
    }
}