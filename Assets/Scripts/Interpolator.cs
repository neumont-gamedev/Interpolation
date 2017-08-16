using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Interpolator : MonoBehaviour
{
    public enum eInterpolationType
    {
        LINEAR,
        SMOOTH_STEP,
        SMOOTHER_STEP,
        SINE_IN,
        SINE_OUT,
        SINE_INOUT,
        QUADRATIC_IN,
        QUADRATIC_OUT,
        QUADRATIC_INOUT,
        CUBIC_IN,
        CUBIC_OUT,
        CUBIC_INOUT,
        QUARTIC_IN,
        QUARTIC_OUT,
        QUARTIC_INOUT,
        QUINTIC_IN,
        QUINTIC_OUT,
        QUINTIC_INOUT,
        CIRCULAR_IN,
        CIRCULAR_OUT,
        CIRCULAR_INOUT,
        EXPO_IN,
        EXPO_OUT,
        EXPO_INOUT,
        BACK_IN,
        BACK_OUT,
        BACK_INOUT,
        BOUNCE_IN,
        BOUNCE_OUT,
        BOUNCE_INOUT,
        ELASTIC_IN,
        ELASTIC_OUT,
        ELASTIC_INOUT,
        NUM
    }

    public enum eTimeType
    {
        ONETIME,
        REPEAT,
        BOUNCE
    }

    [SerializeField] eInterpolationType m_interpolationType = eInterpolationType.LINEAR;
    [SerializeField] eTimeType m_timeType = eTimeType.ONETIME;
    [SerializeField] [Range(0.0f, 5.0f)] float m_rate = 1.0f;
    [SerializeField] Transform m_startTransform = null;
    [SerializeField] Transform m_endTransform = null;
    
    delegate float InterpolationFunction(float t);
    Dictionary<eInterpolationType, InterpolationFunction> m_interpolations = new Dictionary<eInterpolationType, InterpolationFunction>();

    float direction { get; set; }
    float timer { get; set; }

    public Vector3 start { get; set; }
    public Vector3 end { get; set; }
    public Vector3 position { get; set; }
    public float time { get; set; }
    public float interp { get; set; }
    public eInterpolationType type { get { return m_interpolationType; } set { m_interpolationType = value; } }

    private void Awake()
    {
        m_interpolations[eInterpolationType.LINEAR] = Interpolation.Linear;
        m_interpolations[eInterpolationType.SMOOTH_STEP] = Interpolation.SmoothStep;
        m_interpolations[eInterpolationType.SMOOTHER_STEP] = Interpolation.SmootherStep;
        m_interpolations[eInterpolationType.SINE_IN] = Interpolation.SineIn;
        m_interpolations[eInterpolationType.SINE_OUT] = Interpolation.SineOut;
        m_interpolations[eInterpolationType.SINE_INOUT] = Interpolation.SineInOut;
        m_interpolations[eInterpolationType.QUADRATIC_IN] = Interpolation.QuadraticIn;
        m_interpolations[eInterpolationType.QUADRATIC_OUT] = Interpolation.QuadraticOut;
        m_interpolations[eInterpolationType.QUADRATIC_INOUT] = Interpolation.QuadraticInOut;
        m_interpolations[eInterpolationType.CUBIC_IN] = Interpolation.CubicIn;
        m_interpolations[eInterpolationType.CUBIC_OUT] = Interpolation.CubicOut;
        m_interpolations[eInterpolationType.CUBIC_INOUT] = Interpolation.CubicInOut;
        m_interpolations[eInterpolationType.QUARTIC_IN] = Interpolation.QuarticIn;
        m_interpolations[eInterpolationType.QUARTIC_OUT] = Interpolation.QuarticOut;
        m_interpolations[eInterpolationType.QUARTIC_INOUT] = Interpolation.QuarticInOut;
        m_interpolations[eInterpolationType.QUINTIC_IN] = Interpolation.QuinticIn;
        m_interpolations[eInterpolationType.QUINTIC_OUT] = Interpolation.QuinticOut;
        m_interpolations[eInterpolationType.QUINTIC_INOUT] = Interpolation.QuinticInOut;
        m_interpolations[eInterpolationType.CIRCULAR_IN] = Interpolation.CircularIn;
        m_interpolations[eInterpolationType.CIRCULAR_OUT] = Interpolation.CircularOut;
        m_interpolations[eInterpolationType.CIRCULAR_INOUT] = Interpolation.CircularInOut;
        m_interpolations[eInterpolationType.EXPO_IN] = Interpolation.ExpoIn;
        m_interpolations[eInterpolationType.EXPO_OUT] = Interpolation.ExpoOut;
        m_interpolations[eInterpolationType.EXPO_INOUT] = Interpolation.ExpoInOut;
        m_interpolations[eInterpolationType.BACK_IN] = Interpolation.BackIn;
        m_interpolations[eInterpolationType.BACK_OUT] = Interpolation.BackOut;
        m_interpolations[eInterpolationType.BACK_INOUT] = Interpolation.BackInOut;
        m_interpolations[eInterpolationType.BOUNCE_IN] = Interpolation.BounceIn;
        m_interpolations[eInterpolationType.BOUNCE_OUT] = Interpolation.BounceOut;
        m_interpolations[eInterpolationType.BOUNCE_INOUT] = Interpolation.BounceInOut;
        m_interpolations[eInterpolationType.ELASTIC_IN] = Interpolation.ElasticIn;
        m_interpolations[eInterpolationType.ELASTIC_OUT] = Interpolation.ElasticOut;
        m_interpolations[eInterpolationType.ELASTIC_INOUT] = Interpolation.ElasticInOut;

        time = 0.0f;
        timer = 0.0f;
        direction = 1.0f;
        position = Vector3.zero;
        interp = 0.0f;
    }

    void Start()
    {
        Assert.IsNotNull(m_endTransform);
        if (m_startTransform == null)
        {
            m_startTransform = gameObject.transform;
        }
        start = m_startTransform.position;
        end = m_endTransform.position;
    }
		
	void Update()
    {
        float dt = Time.deltaTime;
        timer = timer + (dt * direction);
        time = timer / m_rate;
        if (time >= 1.0f || time <= 0.0f)
        {
            time = Mathf.Clamp01(time);
            switch (m_timeType)
            {
                case eTimeType.ONETIME: break;
                case eTimeType.BOUNCE: direction = direction * -1.0f; break;
                case eTimeType.REPEAT: timer = (time == 1.0f) ? 0.0f : m_rate; break;
            }
        }

        interp = GetInterpolation(time);
        Vector3 iposition = Vector3.LerpUnclamped(start, end, interp);

        position = iposition;
    }

    public float GetInterpolation(float t)
    {
        return (float)m_interpolations[m_interpolationType].DynamicInvoke(t);
    }

    public float GetInterpolation(eInterpolationType type, float t)
    {
        return (float)m_interpolations[type].DynamicInvoke(t);
    }
}
