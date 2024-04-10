using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public float MaxHp;
    float currentHp;
    public delegate void ObjectDestroyedDelegate();
    public event ObjectDestroyedDelegate OnObjectDestroyed;
    public event ObjectDestroyedDelegate OnObjectDamaged;
    public bool isDamageable = true;
    private void Awake()
    {
        SetHealth(MaxHp);
    }
    public void DoDamage(float damageAmount)
    {
        currentHp -= damageAmount;
        if (OnObjectDamaged != null)
            OnObjectDamaged.Invoke();
        CheckIfDestroyed();
    }
    public void HealDamage(float healAmount)
    {
        currentHp += healAmount;
    }
    public void SetHealth(float hpAmount)
    {
        currentHp = hpAmount;
        CheckIfDestroyed();
    }
    public float GetHealth()
    {
        return currentHp;
    }

    bool CheckIfDestroyed()
    {
        if (currentHp <= 0)
        {
            if (OnObjectDestroyed != null)
                OnObjectDestroyed.Invoke();
            return true;
        }
        else return false;
    }

    private void OnEnable()
    {
        isDamageable = true;
    }
    private void OnDisable()
    {
        isDamageable = false;
    }
}
