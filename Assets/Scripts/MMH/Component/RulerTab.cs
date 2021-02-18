using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MMH.Component
{
    public class RulerTab : Tab
    {
        private System.EntitySystem entitySystem;

        private TextMeshProUGUI label;
        private TMP_Dropdown dropdown;

        private GameObject ruleListObject;

        private List<string> colonistBehaviors;

        public override void Awake()
        {
            entitySystem = GameObject.Find("Entity System").GetComponent<System.EntitySystem>();

            Active = false;

            ruleListObject = transform.Find("Colonist Behavior").gameObject;

            SetupBehaviorDropdown();
        }


        private void SetupBehaviorDropdown()
        {
            label = ruleListObject.transform.Find("Label").GetComponent<TextMeshProUGUI>();
            label.text = "Colonist Behavior";

            dropdown = ruleListObject.transform.Find("Dropdown").GetComponent<TMP_Dropdown>();

            colonistBehaviors = new List<string>
            {
                "None",
                "Wander",
                "Gather"
            };

            dropdown.ClearOptions();
            dropdown.AddOptions(colonistBehaviors);

            dropdown.onValueChanged.AddListener(
                delegate { UpdateColonistBehavior(); }
            );
        }


        private void UpdateColonistBehavior()
        {
            entitySystem.OnColonistBehaviorChange(colonistBehaviors[dropdown.value]);
        }
    }
}