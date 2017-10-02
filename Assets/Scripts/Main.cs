using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField] Camera m_camera = null;
    [SerializeField] GameObject m_gameObject = null;
    [SerializeField] [Range(1.0f, 50.0f)] float m_width = 20.0f;
    [SerializeField] [Range(1.0f, 50.0f)] float m_height = 20.0f;
    [SerializeField] [Range(1.0f,  5.0f)] float m_cameraZoomDistance = 5.0f;
    [SerializeField] [Range(0.1f,  5.0f)] float m_cameraZoomRate = 1.0f;

    Vector3 m_cameraDefault = Vector3.zero;
    Vector3 m_cameraSource = Vector3.zero;
    Vector3 m_cameraTarget = Vector3.zero;

    float time { get; set; }

    void Start()
    {
        int num = (int)Interpolator.eInterpolationType.NUM;
        int rc = Mathf.CeilToInt(Mathf.Sqrt((float)num));

        float sx = m_width / rc;
        float sy = m_height / rc;

        for (int i = 0; i < num; i++)
        {
            Vector3 position = Vector3.zero;
            position.x = (sx * (i % rc)) - (m_width * 0.5f) + (sx * 0.5f);
            position.y = (sy * (rc - (i / rc))) - (m_height * 0.5f) - (sy * 0.5f);

            GameObject gameObject = Instantiate(m_gameObject, position, Quaternion.identity, this.gameObject.transform);

            Interpolator interpolator = gameObject.GetComponent<Interpolator>();
            interpolator.type = (Interpolator.eInterpolationType)i;
        }

        m_cameraDefault = m_camera.transform.position;
        m_cameraSource = m_cameraDefault;
        m_cameraTarget = m_cameraDefault;

        time = 1.0f;
    }
	
	void Update()
    {
        if (Input.GetMouseButtonDown(0) && time == 1.0f)
        {
            Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.tag == "Frame")
                {
                    m_cameraTarget = hit.collider.gameObject.transform.position + Vector3.back * m_cameraZoomDistance;
                }
            }
            else
            {
                m_cameraTarget = m_cameraDefault;
            }

            m_cameraSource = m_camera.transform.position;
            time = 0.0f;
        }

        time = time + (Time.deltaTime * m_cameraZoomRate);
        time = Mathf.Clamp01(time);
        m_camera.transform.position = Vector3.Lerp(m_cameraSource, m_cameraTarget, time);

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
