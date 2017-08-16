using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterpTrail : MonoBehaviour
{
    [SerializeField] Interpolator m_interpolator = null;
    [SerializeField] [Range(0.0f, 2.0f)] float m_time = 0.5f;
    [SerializeField] Transform m_startTransform = null;
    [SerializeField] Transform m_endTransform = null;

    TrailRenderer m_trailRenderer = null;

    public Vector3 start { get; set; }
    public Vector3 end { get; set; }
    
    private void Start()
    {
        m_trailRenderer = GetComponent<TrailRenderer>();

        start = m_startTransform.position;
        end = m_endTransform.position;
    }

    void Update()
    {
        Vector3 position;

        position.x = Mathf.LerpUnclamped(start.x, end.x, m_interpolator.time);
        position.y = Mathf.LerpUnclamped(start.y, end.y, m_interpolator.interp);
        position.z = gameObject.transform.position.z;

        float distance = (gameObject.transform.position - position).sqrMagnitude;
        m_trailRenderer.time = (distance < 0.5f) ? m_time : 0.0f;

        gameObject.transform.position = position;
    }
}
