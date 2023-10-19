using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct WindConfig
{
    public Vector3 Direction;
    public GameObject WindParticles;
}

public class WindTrap : MonoBehaviour
{
    [SerializeField]
    private List<WindConfig> winds;

    [SerializeField]
    private float windForce;

    [SerializeField]
    private float changeTime;

    private float timer;
    private IEnumerator<WindConfig> iterator;

    private void Awake()
    {
        foreach (WindConfig item in winds)
        {
            item.WindParticles.SetActive(false);
        }
        iterator = winds.GetEnumerator();
        _ = iterator.MoveNext();
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            iterator.Current.WindParticles.SetActive(false);
            timer = changeTime;
            if (!iterator.MoveNext())
            {
                iterator.Reset();
                _ = iterator.MoveNext();
            }
            iterator.Current.WindParticles.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Rigidbody rigidBody))
        {
            rigidBody.AddForce(
                transform.TransformDirection(iterator.Current.Direction) * windForce,
                ForceMode.Force
            );
        }
    }
}
