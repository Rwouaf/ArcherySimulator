using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[RequireComponent(typeof(BoxCollider))]
public class DamageMakerVolume : MonoBehaviour
{
    [SerializeField] int m_Damage;

    BoxCollider m_BoxCollider;

    void Awake()
    {
        m_BoxCollider = GetComponent<BoxCollider>();
        m_BoxCollider.isTrigger = true;

        m_BoxCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        bool isColliderDestroyable = other.TryGetComponent(out IDestroyable destroyable);

        if (!isColliderDestroyable || other.gameObject.layer == transform.root.gameObject.layer)
            return;

        destroyable.MakeDamage(m_Damage);
    }


    public void OnInitDamage(Component sender, object data)
    {
        if (!ReferenceEquals(sender.gameObject, transform.root.gameObject)) return;

        m_Damage = (int)(data as int?);
    }

    public void OnSomeoneStartAttack(Component sender, object data)
    {
        if (!ReferenceEquals(sender.gameObject, transform.root.gameObject)) return;

        m_BoxCollider.enabled = true;
    }
    public void OnSomeoneEndAttack(Component sender, object data)
    {
        if (!ReferenceEquals(sender.gameObject, transform.root.gameObject)) return;


        m_BoxCollider.enabled = false;
    }

}
