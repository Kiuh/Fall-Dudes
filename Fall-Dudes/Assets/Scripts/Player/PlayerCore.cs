using Common;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCore : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;
    public PlayerController PlayerController => playerController;

    [SerializeField]
    private float startPlayerHealth;
    public float StartPlayerHealth => startPlayerHealth;

    [SerializeField]
    [InspectorReadOnly]
    private float health;
    public float Health => health;

    public UnityEvent OnDeath;

    public void SetHealth(float value)
    {
        health = value;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            OnDeath?.Invoke();
        }
    }
}
