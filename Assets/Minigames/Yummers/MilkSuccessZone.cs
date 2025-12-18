using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkSuccessZone : MonoBehaviour, IMilkDropArea
{
    [SerializeField] MilkWinManager winManager;

    public void OnMilkDrop(Milk milk)
    {

        // Snap milk into place
        milk.transform.position = transform.position;

        // Prevent further dragging
        milk.enabled = false;

        // Trigger win state
        winManager.OnMilkDelivered();
    }
}
