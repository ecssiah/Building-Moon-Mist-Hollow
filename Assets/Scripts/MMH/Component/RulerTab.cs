using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MMH
{
    public class RulerTab : Tab
    {
        private TextMeshProUGUI label;
        private TMP_Dropdown dropdown;

        private GameObject ruleListObject;

        private List<string> citizenBehaviors;

        public override void Awake()
        {
            Active = false;

            ruleListObject = transform.Find("Citizen Behavior").gameObject;

            label = ruleListObject.transform.Find("Label").GetComponent<TextMeshProUGUI>();
            label.text = "Citizen Behavior";

            citizenBehaviors = new List<string>
            {
                "Wander Out",
                "Gather Home"
            };

            dropdown = ruleListObject.transform.Find("Dropdown").GetComponent<TMP_Dropdown>();
            dropdown.ClearOptions();
            dropdown.AddOptions(citizenBehaviors);
            dropdown.onValueChanged.AddListener(
                delegate { OnCitizenBehaviorChange(); }
            );
        }


        private void OnCitizenBehaviorChange()
        {
            print(citizenBehaviors[dropdown.value]);
        }


        void Update()
        {
        }
    }
}