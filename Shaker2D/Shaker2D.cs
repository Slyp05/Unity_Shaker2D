using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*

    Shaker2D created by Lucas Sarkadi.

    Creative Commons Zero v1.0 Universal licence, 
    meaning it's free to use in any project with no need to ask permission or credits the author.

    Check out the github page for more informations:
    https://github.com/Slyp05/Unity_Shaker2D

*/
public class Shaker2D : MonoBehaviour
{
    // consts
    public const float defaultMaxShakeTranslateX = 5;
    public const float defaultMaxShakeTranslateY = 5;
    public const float defaultMaxShakeAngle = 30;
    public const float defaultPerlinNoiseMultiplier = 8;

    public const float defaultTraumaToShakePower = 2;
    public const float defaultTraumaDecreasePerSec = 1;

    // flag enum
    [System.Flags]
    public enum Type
    {
        None = 0,
        TranslationX = 1,
        TranslationY = 2,
        Rotation = 4,
    }
    public const Type allType = (Type)~0;

    // editor parameters
    [SerializeField] Transform _shakePivot = null;

    public Type possibleShake = allType;
    public Type defaultShake = allType; // + ou = restrictif a possibleShake

    public float maxShakeTranslateX = defaultMaxShakeTranslateX;
    public float maxShakeTranslateY = defaultMaxShakeTranslateY;
    public float maxShakeAngle = defaultMaxShakeAngle;
    public float perlinNoiseMultiplier = defaultPerlinNoiseMultiplier;

    public float traumaToShakePower = defaultTraumaToShakePower;
    public float traumaDecreasePerSec = defaultTraumaDecreasePerSec;

    // public access
    public Transform shakePivot { get { return _shakePivot; } set { SetNewPivot(value); } }

    /// <summary>
    /// Add some trauma, the more trauma added the more shake will be produced
    /// </summary>
    /// <param name="trauma">A value between 0 and 1, with 1 being maximum shake</param>
    /// <param name="shakeType">What type of shake should we do</param>
    public void AddTrauma(float trauma, Type shakeType)
    {
        if ((shakeType & possibleShake & Type.TranslationX) != Type.None)
            traumaX = Mathf.Clamp01(traumaX + trauma);
        if ((shakeType & possibleShake & Type.TranslationY) != Type.None)
            traumaY = Mathf.Clamp01(traumaY + trauma);
        if ((shakeType & possibleShake & Type.Rotation) != Type.None)
            traumaRot = Mathf.Clamp01(traumaRot + trauma);
    }
    /// <summary>
    /// Add some trauma using the defaultShake defined in the inspector, the more trauma added the more shake will be produced
    /// </summary>
    /// <param name="trauma">A value between 0 and 1, with 1 being maximum shake</param>
    public void AddTrauma(float trauma)
    {
        AddTrauma(trauma, defaultShake);
    }

    // internal logic
    [SerializeField] float traumaX;
    [SerializeField] float traumaY;
    [SerializeField] float traumaRot;

    void Update()
    {
        // default pivot to current transform if needed
        if (shakePivot == null)
            shakePivot = transform;

        // get shake values
        float pow = (traumaToShakePower > 0 ? traumaToShakePower : defaultTraumaToShakePower);
        float shakeX = Mathf.Pow(traumaX, pow);
        float shakeY = Mathf.Pow(traumaY, pow);
        float shakeRot = Mathf.Pow(traumaRot, pow);

        // do the shake
        float multiplier = (perlinNoiseMultiplier > 0 ? perlinNoiseMultiplier : defaultPerlinNoiseMultiplier);
        shakePivot.localPosition = startLocalPosition.Value
            + new Vector2(maxShakeTranslateX * shakeX * (Mathf.PerlinNoise(Time.time * multiplier, 0) * 2 - 1),
                          maxShakeTranslateY * shakeY * (Mathf.PerlinNoise(Time.time * multiplier, 10) * 2 - 1));
        shakePivot.localEulerAngles = new Vector3(0, 0,
            startLocalAngle.Value + maxShakeAngle * shakeRot * (Mathf.PerlinNoise(Time.time * multiplier, 20) * 2 - 1));

        // decrease traumas values
        float decrease = (traumaDecreasePerSec > 0 ? traumaDecreasePerSec : defaultTraumaDecreasePerSec) * Time.deltaTime;
        traumaX = Mathf.Clamp01(traumaX - decrease);
        traumaY = Mathf.Clamp01(traumaY - decrease);
        traumaRot = Mathf.Clamp01(traumaRot - decrease);
    }

    Vector2? startLocalPosition;
    float? startLocalAngle;

    void SetNewPivot(Transform newPivot)
    {
        // reset old pivot to original local position
        if (_shakePivot != null)
        {
            if (startLocalPosition.HasValue)
                _shakePivot.localPosition = startLocalPosition.Value;
            if (startLocalAngle.HasValue)
                _shakePivot.localEulerAngles = new Vector3(0, 0, startLocalAngle.Value);
        }

        // set new pivot
        _shakePivot = newPivot;
        if (newPivot != null)
        {
            startLocalAngle = newPivot.localEulerAngles.z;
            startLocalPosition = newPivot.localPosition;
        }
    }
}
