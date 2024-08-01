using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTrial : MonoBehaviour
{
    public GameObject challengeParent; // Empty GameObject que contiene las esferas

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TrialManager challengeManager = challengeParent.GetComponent<TrialManager>();
            if (challengeManager != null)
            {
                challengeManager.ActivateChallenge();
                this.gameObject.SetActive(false);
            }
        }
    }
}
