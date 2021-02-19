using System;
using System.Linq;
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

        public override void Awake()
        {
            entitySystem = GameObject.Find("Entity System").GetComponent<System.EntitySystem>();

            Active = false;

            ruleListObject = transform.Find("Colonist Behavior").gameObject;

            SetupMovementBehaviorDropdown();
        }


        private void SetupMovementBehaviorDropdown()
        {
            label = ruleListObject.transform.Find("Label").GetComponent<TextMeshProUGUI>();
            label.text = "Colonist Movement Behavior";

            dropdown = ruleListObject.transform.Find("Dropdown").GetComponent<TMP_Dropdown>();

            List<string> movementBehaviorNames = Enum.GetNames(typeof(Type.Behavior.Movement)).ToList();

            dropdown.ClearOptions();
            dropdown.AddOptions(movementBehaviorNames);

            dropdown.onValueChanged.AddListener(
                delegate { OnColonistMovementBehaviorChange(); }
            );
        }


        private void OnColonistMovementBehaviorChange()
        {
            entitySystem.SetColonistMovementBehavior((Type.Behavior.Movement)dropdown.value);
        }
    }
}