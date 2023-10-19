using UnityEngine;
using UnityEngine.Events;

public class StartLineZone : MonoBehaviour
{
    public UnityEvent OnPlayerReachStartLine;

    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerCore _))
        {
            OnPlayerReachStartLine?.Invoke();
        }
    }
}
