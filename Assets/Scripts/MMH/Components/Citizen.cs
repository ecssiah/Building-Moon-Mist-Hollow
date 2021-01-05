using UnityEngine;

public class Citizen : MonoBehaviour
{
    public EntityData EntityData;

    public IdData IdData;
    public InventoryData InventoryData;

    private CitizenAnimator citizenAnimator;
    private CitizenMovement citizenMovement;


    void Awake()
    {
        citizenAnimator = gameObject.AddComponent<CitizenAnimator>();
        citizenMovement = gameObject.AddComponent<CitizenMovement>();

        citizenMovement.OnDirectionChange = citizenAnimator.OnDirectionChange;
    }
}
