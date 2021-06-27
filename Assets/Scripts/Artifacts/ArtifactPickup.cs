using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactPickup : MonoBehaviour
{
    private const float POLLING_RATE = 0.1f;

    [SerializeField] private Artifact artifact;
    [SerializeField] private float pickupRadius = 1f;
    [SerializeField] private LayerMask playerLayer;

    private float sqrPickupDist;
    private YieldInstruction pollingRateInstruction;

    void Awake()
    {
        sqrPickupDist = pickupRadius * pickupRadius;
        pollingRateInstruction = new WaitForSeconds(POLLING_RATE);
    }

    void OnEnable()
    {
        StartCoroutine(UpdatePickups());
    }

    private IEnumerator UpdatePickups()
    {
        while (enabled)
        {
            CheckPlayerPickup(GameManager.Player1);
            CheckPlayerPickup(GameManager.Player2);
            yield return pollingRateInstruction;
        }
    }

    private void CheckPlayerPickup(Player player)
    {
        float sqrDist = (player.transform.position - transform.position).sqrMagnitude;
        if (sqrDist < sqrPickupDist)
        {
            // activate tooltip

            // tell player they can pickup

            // give player Artifact
        }
    }
}
