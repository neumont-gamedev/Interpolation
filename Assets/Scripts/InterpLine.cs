using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterpLine : MonoBehaviour
{
    [SerializeField] Interpolator m_interpolator = null;
    [SerializeField] [Range(1, 100)] int m_size = 20;
    [SerializeField] Transform m_startTransform = null;
    [SerializeField] Transform m_endTransform = null;

    LineRenderer m_lineRenderer = null;

    public Vector3 start { get; set; }
    public Vector3 end { get; set; }

    private void Start()
    {
        m_lineRenderer = GetComponent<LineRenderer>();
        m_lineRenderer.positionCount = m_size;

        start = m_startTransform.position;
        end = m_endTransform.position;
    }
    
    void Update()
    {
        m_lineRenderer.positionCount = m_size;
        float di = 1.0f / (float)(m_size - 1);
        for (int i = 0; i < m_size; i++)
        {
            float time = di * i;
            float interp = m_interpolator.GetInterpolation(time);

            Vector3 position;
            position.x = Mathf.LerpUnclamped(start.x, end.x, time);
            position.y = Mathf.LerpUnclamped(start.y, end.y, interp);
            position.z = gameObject.transform.position.z;
                        
            m_lineRenderer.SetPosition(i, position);
        }
	}
}
