using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Target : Interactable
{
    public override string PromptMessage { get => $"Fix target ({damageable.GetHealth()})"; set => PromptMessage = value; }
    Damageable damageable;
    public TextMeshPro targetHealthIndicator;
    private void Start()
    {
        damageable = GetComponent<Damageable>();
        targetHealthIndicator.text = $"{damageable.GetHealth()}/{damageable.MaxHp}";
        damageable.OnObjectDestroyed += () =>
        {
            damageable.enabled = false;
            Debug.Log("Destroyed");
        };
        damageable.OnObjectDamaged += () => targetHealthIndicator.text = $"{damageable.GetHealth()}/{damageable.MaxHp}";
    }
    protected override void Interact()
    {
        damageable.SetHealth(damageable.MaxHp);
        damageable.enabled = true;
        targetHealthIndicator.text = $"{damageable.GetHealth()}/{damageable.MaxHp}";
        Debug.Log("Fixed Target");
    }
}
