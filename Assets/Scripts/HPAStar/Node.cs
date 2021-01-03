﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;


public class Node : FastPriorityQueueNode
{
    public int Index;
    public Vector2Int Position;


    public override string ToString()
    {
        return $"{Index} : {Position.x},{Position.y}";
    }
}
