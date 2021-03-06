﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MMH
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
            entitySystem = GameObject.Find("EntitySystem").GetComponent<System.EntitySystem>();

            Active = false;

            ruleListObject = transform.Find("Colonist Behavior").gameObject;

            label = ruleListObject.transform.Find("Label").GetComponent<TextMeshProUGUI>();
            label.text = "Colonist Behavior";

            colonistBehaviors = new List<string>
            {
                "Wander Out",
                "Gather Home"
            };

            dropdown = ruleListObject.transform.Find("Dropdown").GetComponent<TMP_Dropdown>();
            dropdown.ClearOptions();
            dropdown.AddOptions(colonistBehaviors);
            dropdown.onValueChanged.AddListener(
                delegate { OnColonistBehaviorChange(); }
            );
        }


        private void OnColonistBehaviorChange()
        {
            entitySystem.OnColonistBehaviorChange(colonistBehaviors[dropdown.value]);
        }


        void Update()
        {
        }
    }
}