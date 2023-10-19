using Common;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RedTrap : MonoBehaviour
{
    [SerializeField]
    private List<MeshRenderer> renderers;
    private List<List<Material>> materials = new();

    [SerializeField]
    private Material orangeMaterial;

    [SerializeField]
    private Material redMaterial;

    [SerializeField]
    private float preparingTime;

    [SerializeField]
    private float activeTime;

    [SerializeField]
    private float reloadTime;

    [SerializeField]
    private float trapDamage;
    private float timer;
    private HashSet<PlayerCore> contactsPlayers = new();

    [SerializeField]
    [InspectorReadOnly]
    private TrapState state;

    private enum TrapState
    {
        Waiting,
        Preparing,
        Active,
        Reloading,
    }

    private void Awake()
    {
        foreach (CollisionEvent collider in GetComponentsInChildren<CollisionEvent>())
        {
            collider.OnCollisionEnterEvent.AddListener(OnCollisionEnterChild);
            collider.OnCollisionExitEvent.AddListener(OnCollisionExitChild);
        }
        renderers = GetComponentsInChildren<MeshRenderer>().ToList();
        foreach (MeshRenderer renderer in renderers)
        {
            materials.Add(renderer.materials.ToList());
        }
    }

    private void Update()
    {
        if (
            state == TrapState.Preparing
            || state == TrapState.Active
            || state == TrapState.Reloading
        )
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                switch (state)
                {
                    case TrapState.Preparing:
                        ApplyMaterial(redMaterial);
                        foreach (PlayerCore player in contactsPlayers)
                        {
                            player.TakeDamage(trapDamage);
                        }
                        timer = activeTime;
                        state = TrapState.Active;
                        break;
                    case TrapState.Active:
                        ResetMaterials();
                        timer = reloadTime;
                        state = TrapState.Reloading;
                        break;
                    case TrapState.Reloading:
                        state = TrapState.Waiting;
                        break;
                }
            }
        }
    }

    private void OnCollisionEnterChild(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (
                contact.otherCollider.TryGetComponent(out PlayerCore player)
                && !contactsPlayers.Contains(player)
            )
            {
                _ = contactsPlayers.Add(player);
                if (state == TrapState.Active)
                {
                    player.TakeDamage(trapDamage);
                }
            }
        }
        if (state == TrapState.Waiting)
        {
            state = TrapState.Preparing;
            ApplyMaterial(orangeMaterial);
            timer = preparingTime;
        }
    }

    private void OnCollisionExitChild(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (
                contact.otherCollider.TryGetComponent(out PlayerCore player)
                && contactsPlayers.Contains(player)
            )
            {
                _ = contactsPlayers.Remove(player);
            }
        }
    }

    private void ApplyMaterial(Material material)
    {
        for (int i = 0; i < renderers.Count; i++)
        {
            renderers[i].materials = materials[i].Append(material).ToArray();
        }
    }

    private void ResetMaterials()
    {
        for (int i = 0; i < renderers.Count; i++)
        {
            renderers[i].materials = materials[i].ToArray();
        }
    }
}
