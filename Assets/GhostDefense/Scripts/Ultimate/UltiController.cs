using System.Collections;
using System.Collections.Generic;
using UDEV.GhostDefense;
using UnityEngine;

namespace UDEV.GhostDefense { }
public class UltiController : MonoBehaviour, IDamageCreater
{
    [Range(0f, 1f)]
    public float rate;

    private Actor m_owner;

    public Actor Owner { get => m_owner; set => m_owner = value; }

    public virtual void DealDamage()
    {
        
    }
}
