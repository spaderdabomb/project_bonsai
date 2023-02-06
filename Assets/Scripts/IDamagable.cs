using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    public ItemData.ItemEnum[] itemsCanDamage { get; set; }

    public void UpdateHealthbar();
    public void PositionHealthLabel();
    public void DestroyObject();
    public float Damage(float damage);
    public bool CanDamage(ItemData.ItemEnum itemEnum);
}
