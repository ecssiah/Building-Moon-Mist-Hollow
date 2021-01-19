using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MMH
{
    public class RulerTab : Tab
    {
        private TextMeshProUGUI testTextContent;

        public override void Awake()
        {
            Active = false;

            testTextContent = gameObject.AddComponent<TextMeshProUGUI>();
            testTextContent.text = "Ruler";
        }


        void Update()
        {
        }
    }
}