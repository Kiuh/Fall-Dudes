using Common;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField]
    [InspectorReadOnly]
    private float timer = 0;
    public float CurrentTime => timer;

    private bool lockTimer = true;

    private void Update()
    {
        if (!lockTimer)
        {
            timer += Time.deltaTime;
        }
    }

    public void StartTimer()
    {
        lockTimer = false;
    }

    public void Reset()
    {
        timer = 0;
    }

    public void Stop()
    {
        lockTimer = true;
    }
}
