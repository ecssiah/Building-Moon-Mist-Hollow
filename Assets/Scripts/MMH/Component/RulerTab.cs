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
            Active = true;

            testTextContent = gameObject.AddComponent<TextMeshProUGUI>();
            testTextContent.text = "I'm testing heeeerrreee!";
        }


        void Update()
        {
        }
    }
}