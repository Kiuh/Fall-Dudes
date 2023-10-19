using UnityEngine;
using UnityEngine.Events;

public class CollisionEvent : MonoBehaviour
{
    public UnityEvent<Collision> OnCollisionEnterEvent;
    public UnityEvent<Collision> OnCollisionExitEvent;

    public void OnCollisionEnter(Collision collision)
    {
        OnCollisionEnterEvent?.Invoke(collision);
    }

    public void OnCollisionExit(Collision collision)
    {
        OnCollisionExitEvent?.Invoke(collision);
    }
}
