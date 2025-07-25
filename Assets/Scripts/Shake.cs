using UnityEngine;
using System.Collections;

[RequireComponent(typeof(RectTransform))]
public class Shake : MonoBehaviour
{
    [Header("Shake Settings")]
    public float shakeDuration = 0.5f;
    public float maxAngle = 2f;
    public bool useCurve = true;
    public AnimationCurve shakeCurve;
    
    [Header("Vibration Settings")]
    public bool enableVibration = true;
    public VibrationType vibrationType = VibrationType.Pop;
    
    public enum VibrationType { Pop, Peek, Normal }

    private RectTransform rectTransform;
    private Quaternion originalRotation;

    public static Shake instance;

    void Awake()
    {
        instance = this;
        rectTransform = GetComponent<RectTransform>();
        originalRotation = rectTransform.localRotation;
        
        if (shakeCurve == null || shakeCurve.length == 0)
        {
            CreateDefaultCurve();
        }
    }

    void CreateDefaultCurve()
    {
        shakeCurve = new AnimationCurve(
            new Keyframe(0.0f, 0.0f),
            new Keyframe(0.25f, 1.0f),
            new Keyframe(0.5f, 0.0f),
            new Keyframe(0.75f, -1.0f),
            new Keyframe(1.0f, 0.0f)
        );
    }

    public void ScreenShake()
    {
        StopAllCoroutines();
        StartCoroutine(DoShakeAnimation());
        
        if (enableVibration)
        {
            switch (vibrationType)
            {
                case VibrationType.Pop:
                    Vibration.VibratePop();
                    break;
                case VibrationType.Peek:
                    Vibration.VibratePeek();
                    break;
                case VibrationType.Normal:
                    Vibration.Vibrate();
                    break;
            }
        }
    }

    IEnumerator DoShakeAnimation()
    {
        if (useCurve)
        {
            float timer = 0f;
            while (timer < shakeDuration)
            {
                timer += Time.deltaTime;
                float progress = Mathf.Clamp01(timer / shakeDuration);
                float curveValue = shakeCurve.Evaluate(progress);
                rectTransform.localRotation = originalRotation * Quaternion.Euler(0, 0, curveValue * maxAngle);
                yield return null;
            }
        }
        else
        {
            float segmentDuration = shakeDuration / 4f;
            yield return RotateTo(originalRotation.eulerAngles.z, maxAngle, segmentDuration);
            yield return RotateTo(maxAngle, 0f, segmentDuration);
            yield return RotateTo(0f, -maxAngle, segmentDuration);
            yield return RotateTo(-maxAngle, 0f, segmentDuration);
        }

        rectTransform.localRotation = originalRotation;
    }

    IEnumerator RotateTo(float fromAngle, float toAngle, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float currentAngle = Mathf.Lerp(fromAngle, toAngle, timer / duration);
            rectTransform.localRotation = originalRotation * Quaternion.Euler(0, 0, currentAngle);
            yield return null;
        }
    }

    void OnDisable()
    {
        rectTransform.localRotation = originalRotation;
    }
}
