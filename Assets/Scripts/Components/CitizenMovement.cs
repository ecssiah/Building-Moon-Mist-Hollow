using System;
using UnityEngine;

public class CitizenMovement : MonoBehaviour
{
    public Action<Vector2> OnDirectionChange;

    private float decisionTimer;
    private float decisionDeadline;


    void Awake()
    {
        decisionTimer = 0;
        decisionDeadline = UnityEngine.Random.Range(2f, 6f);
    }


    public void Think()
    {
        decisionTimer += Time.deltaTime;

        if (decisionTimer >= decisionDeadline)
        {
            decisionTimer -= decisionDeadline;
            decisionDeadline = UnityEngine.Random.Range(2f, 6f);

            Decide();
        }

    }


    private void Decide()
    {
        OnDirectionChange(GetRandomIsoDirection());    
    }


    private Vector2 GetRandomIsoDirection()
    {
        var newIsoDirection = new Vector2(
            UnityEngine.Random.Range(-1, 2),
            UnityEngine.Random.Range(-1, 2)
        );

        return MapUtil.IsoToWorld(newIsoDirection);
    }
}
