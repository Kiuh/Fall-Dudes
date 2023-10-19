using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [SerializeField]
    private float damage = 9999;

    public void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (contact.otherCollider.TryGetComponent(out PlayerCore player))
            {
                player.TakeDamage(damage);
            }
        }
    }
}
