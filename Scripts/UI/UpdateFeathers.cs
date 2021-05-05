using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class UpdateFeathers : MonoBehaviour
{
    public static UpdateFeathers instance;

    [SerializeField]
    private Text m_adderText;

    [SerializeField]
    [Range(1f, 10f)]
    private float m_adderTextAlphaSpeed;

    private int m_feathersToAdd;

    [SerializeField]
    private float m_wait = 1.5f;
    private float m_t = 0;

    public bool fastTrackAdding = false;

    public void FastTrackFeathers()
    {
        fastTrackAdding = true;
    }

    public void AddFeathers(int a_feathersToAdd)
    {
        m_t = Time.time;
        m_feathersToAdd += a_feathersToAdd;
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Update()
    {
        if (fastTrackAdding)
        {
            SaveManager.Instance.state.Feathers += m_feathersToAdd;
            m_feathersToAdd = 0;
            fastTrackAdding = false;
        }

        m_adderText.text = "+ " + m_feathersToAdd.ToString();

        Color textColor = m_adderText.color;

        if (m_feathersToAdd == 0)
        {
            textColor.a = Mathf.Clamp01(Mathf.Lerp(textColor.a, -0.5f, Time.deltaTime * m_adderTextAlphaSpeed));
        }
        else
        {
            textColor.a = Mathf.Clamp01(Mathf.Lerp(textColor.a, 1.5f, Time.deltaTime * m_adderTextAlphaSpeed));
        }

        m_adderText.color = textColor;

        if (Time.time - m_t > m_wait)
        {
            if (m_feathersToAdd > 0 && PlayerController.instance.Grounded)
            {
                Apply();
            }
        }
    }

    void Apply()
    {
        m_feathersToAdd--;
        SaveManager.Instance.state.Feathers++;
    }
}
