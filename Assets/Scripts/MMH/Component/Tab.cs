using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MMH.Component
{
    public class Tab : MonoBehaviour
    {
        protected bool _active;
        public bool Active
        {
            get => _active;
            set
            {
                _active = value;
                gameObject.SetActive(_active);
            }
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => _name = value;
        }


        public virtual void Awake()
        {
            Active = false;
        }
    }
}