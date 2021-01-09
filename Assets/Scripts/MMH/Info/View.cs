﻿using UnityEngine;

namespace MMH
{
    namespace Info
    {
        public struct View
        {
            public static readonly int DefaultOrthographicSize = 4;

            public static float CameraZoomRatio => DefaultOrthographicSize / Camera.main.orthographicSize;

            public static readonly float MinimumOrthographicSize = 1f;
            public static readonly float MaximumOrthographicSize = 90f;
        }
    }
}