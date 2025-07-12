using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnite : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<PlayerControl>())
            Destroy(gameObject);
    }

    private void OnDestroy() {
        WaveManager.Instance.CollectMagnite();
    }
}
