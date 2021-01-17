using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tab : MonoBehaviour
{
    private bool _active;
    public bool Active => _active;

    private string _name;
    public string Name => _name;

    public Tab(string name)
    {
        _active = true;
        _name = name;

        gameObject.SetActive(_active);
    }
}
