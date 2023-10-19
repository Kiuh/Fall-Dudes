using Common;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct MaceTrapState
{
    public Vector3 Rotation;
    public Vector3 ForceDirection;
}

public class MaceTrap : MonoBehaviour
{
    [SerializeField]
    private List<MaceTrapState> trapStates;

    [SerializeField]
    private AnimationCurve curve;

    [SerializeField]
    private float hitForce;

    [SerializeField]
    private float flyTime;

    [SerializeField]
    private float freezeTime;
    private float timer;
    private IEnumerator<MaceTrapState> iterator1;
    private IEnumerator<MaceTrapState> iterator2;

    [SerializeField]
    [InspectorReadOnly]
    private TrapState trapState;

    private enum TrapState
    {
        Frizzed,
        Fly
    }

    private void Awake()
    {
        iterator1 = trapStates.GetEnumerator();
        _ = iterator1.MoveNext();
        iterator2 = trapStates.GetEnumerator();
        _ = iterator2.MoveNext();
        _ = iterator2.MoveNext();
        foreach (CollisionEvent collider in GetComponentsInChildren<CollisionEvent>())
        {
            collider.OnCollisionEnterEvent.AddListener(OnCollisionEnterChild);
        }
    }

    private void OnCollisionEnterChild(Collision collision)
    {
        if (trapState == TrapState.Fly)
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                if (contact.otherCollider.TryGetComponent(out Rigidbody rigidBody))
                {
                    rigidBody.AddForce(
                        transform.TransformDirection(iterator1.Current.ForceDirection.normalized)
                            * hitForce,
                        ForceMode.Impulse
                    );
                }
            }
        }
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (trapState == TrapState.Frizzed)
        {
            if (timer <= 0)
            {
                trapState = TrapState.Fly;
                timer = flyTime;
            }
        }
        else if (trapState == TrapState.Fly)
        {
            transform.localRotation = Quaternion.Euler(
                Vector3.Lerp(
                    iterator1.Current.Rotation,
                    iterator2.Current.Rotation,
                    curve.Evaluate(timer / flyTime)
                )
            );
            if (timer <= 0)
            {
                trapState = TrapState.Frizzed;
                timer = freezeTime;
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
        }
    }
}
