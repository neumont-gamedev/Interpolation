using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterpText : MonoBehaviour
{
    [SerializeField] Interpolator m_interpolator = null;

    TMPro.TextMeshPro m_textMeshPro = null;
        
    void Start()
    {
        m_textMeshPro = GetComponent<TMPro.TextMeshPro>();
    }
	
	void Update()
    {
        if (m_textMeshPro != null)
        {
            m_textMeshPro.text = m_interpolator.type.ToString();
        }

        Vector3 position = m_interpolator.position;
        position.x = gameObject.transform.position.x;
        position.z = gameObject.transform.position.z;
        gameObject.transform.position = position;
    }
}
