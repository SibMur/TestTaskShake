using UnityEngine;

public class Diamond : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if(other.GetComponent<PlayerControl>())          
            Destroy(gameObject);        
    }

    private void OnDestroy() {
        WaveManager.Instance.CollectDiamond();
    }
}
