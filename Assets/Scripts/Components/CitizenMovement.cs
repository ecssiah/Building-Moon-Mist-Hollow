using System;
using UnityEngine;

public class CitizenMovement : MonoBehaviour
{
    public Action<Vector2> OnDirectionChange;

    private float decisionTimer;
    private float decisionDeadline;

    private Vector2 direction;


    void Awake()
    {
        decisionTimer = 0;
        decisionDeadline = 4f;
    }


    public void Think()
    {
        decisionTimer += Time.deltaTime;

        if (decisionTimer >= decisionDeadline)
        {
            decisionTimer = 0;

            Decide();
        }
    }


    private void Decide()
    {
        OnDirectionChange(GetRandomIsoDirection());
    }


    private Vector2 GetRandomIsoDirection()
    {
        var newDirection = new Vector3(
            UnityEngine.Random.Range(-1, 2),
            UnityEngine.Random.Range(-1, 2),
            0
        );

        return MapUtil.IsoToWorld(newDirection);
    }
}
