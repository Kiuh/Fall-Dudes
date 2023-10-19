using Common;
using System.Collections.Generic;
using UnityEngine;

public class FallingBlock : MonoBehaviour
{
    [SerializeField]
    private float shakeTime;

    [SerializeField]
    private List<Vector3> rotations;

    [SerializeField]
    private float shakeIntervalTime;

    [SerializeField]
    private AnimationCurve shakeCurve;

    [SerializeField]
    private float fallTime;

    [SerializeField]
    private float fallSpeed;

    [SerializeField]
    private Vector3 fallDirection;
    private float timer;
    private float intervalTimer;
    private IEnumerator<Vector3> iterator1;
    private IEnumerator<Vector3> iterator2;
    private Vector3 startPosition;
    private Quaternion startRotation;

    private void Awake()
    {
        iterator1 = rotations.GetEnumerator();
        _ = iterator1.MoveNext();
        iterator2 = rotations.GetEnumerator();
        _ = iterator2.MoveNext();
        _ = iterator2.MoveNext();
    }

    private void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    public void ResetBlock()
    {
        transform.SetPositionAndRotation(startPosition, startRotation);
        gameObject.SetActive(true);
        state = TrapState.Wait;
    }

    [SerializeField]
    [InspectorReadOnly]
    private TrapState state = TrapState.Wait;

    private enum TrapState
    {
        Wait,
        Shake,
        Fall
    }

    private void Update()
    {
        if (state == TrapState.Shake || state == TrapState.Fall)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                switch (state)
                {
                    case TrapState.Shake:
                        timer = fallTime;
                        state = TrapState.Fall;
                        break;
                    case TrapState.Fall:
                        gameObject.SetActive(false);
                        break;
                }
            }
            switch (state)
            {
                case TrapState.Shake:
                    intervalTimer -= Time.deltaTime;
                    if (intervalTimer <= 0)
                    {
                        intervalTimer = shakeIntervalTime;
                        if (!iterator1.MoveNext())
                        {
                            iterator1.Reset();
                            _ = iterator1.MoveNext();
                        }
                        if (!iterator2.MoveNext())
                        {
                            iterator2.Reset();
                            _ = iterator2.MoveNext();
                        }
                    }
                    transform.localRotation = Quaternion.Euler(
                        Vector3.Lerp(
                            iterator1.Current,
                            iterator2.Current,
                            shakeCurve.Evaluate(intervalTimer / shakeIntervalTime)
                        )
                    );
                    break;
                case TrapState.Fall:
                    transform.position += fallSpeed * Time.deltaTime * fallDirection.normalized;
                    break;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state != TrapState.Wait)
        {
            return;
        }

        foreach (ContactPoint contact in collision.contacts)
        {
            if (contact.otherCollider.TryGetComponent(out PlayerCore _))
            {
                state = TrapState.Shake;
                timer = shakeTime;
                intervalTimer = shakeIntervalTime;
            }
        }
    }
}
