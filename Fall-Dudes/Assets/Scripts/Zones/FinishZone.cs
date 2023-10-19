using UnityEngine;
using UnityEngine.Events;

public class FinishZone : MonoBehaviour
{
    public UnityEvent OnPlayerReachFinish;

    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerCore _))
        {
            OnPlayerReachFinish?.Invoke();
        }
    }
}
