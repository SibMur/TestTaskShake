using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int _healingHP = 2;

    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<PlayerControl>())
            Destroy(gameObject);
    }

    private void OnDestroy() {
        GameManager.Instance.LevelManager.Player.Combat.Heal(_healingHP);
    }
}
