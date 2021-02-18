﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MMH.Handler
{
    public interface IEntityEventHandler : IEventSystemHandler
    {
        void SetColonistBehavior(string behaviorName);
    }
}
