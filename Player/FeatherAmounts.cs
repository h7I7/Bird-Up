using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FeatherQuantities
{
    public int quantity;
    public float chance;
}

public class FeatherAmounts : MonoBehaviour {

    public static FeatherAmounts instance;

    [SerializeField]
    private List<FeatherQuantities> m_quanitities;

    public List<FeatherQuantities> Quantities
    {
        get { return m_quanitities; }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public FeatherQuantities PickAmount()
    {
        float range = 0;
        for (int i = 0; i < m_quanitities.Count; ++i)
            range += m_quanitities[i].chance;

        float rand = UnityEngine.Random.Range(0f, range);
        float top = 0f;

        for (int i = 0; i < m_quanitities.Count; ++i)
        {
            top += m_quanitities[i].chance;
            if (rand < top)
                return m_quanitities[i];
        }

        return m_quanitities[UnityEngine.Random.Range(0, m_quanitities.Count - 1)];
    }
}
