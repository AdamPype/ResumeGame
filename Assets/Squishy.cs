using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squishy : MonoBehaviour {

    //SCRIPT BY PAPERCOOKIES.ITCH.IO

    //Attach script to your own script and use Squish() to squish :-)

    private float _squishyScaleX = 1;
    private float _squishyScaleY = 1;

    private float _squish = 0;

    [SerializeField, Range(0, 1)] private float _squishSlowdownSpeed = 0.1f;
    [SerializeField] private float _squichinessLength = 4;
    [SerializeField] private float _squishSpeed = 3;
    [SerializeField, Range(0, 1)] private float _lerpSpeed = 1f;
    [SerializeField] private float _squishSize = 1f;

    private float[] originalParameters = new float[5];

    private void Start()
        {
        //save original parameters
        originalParameters[0] = _squishSlowdownSpeed;
        originalParameters[1] = _squichinessLength;
        originalParameters[2] = _squishSpeed;
        originalParameters[3] = _lerpSpeed;
        originalParameters[4] = _squishSize;
        }

    private void Update()
        {

        //lerp squish
        _squish = Mathf.Lerp(_squish, 0, _squishSlowdownSpeed);
        //edit scale with squish
        _squishyScaleX = Mathf.Lerp(_squishyScaleX, 1 + SinWave(_squichinessLength - _squish, _squish * _squishSpeed, _squish), _lerpSpeed);
        _squishyScaleY = Mathf.Lerp(_squishyScaleX, 1 - SinWave(_squichinessLength - _squish, _squish * _squishSpeed, _squish), _lerpSpeed);
        transform.localScale = new Vector2(_squishyScaleX, _squishyScaleY);
        }

    private float SinWave(float t, float speed, float amplitude)
        {
        return Mathf.Sin(t * speed) * amplitude;
        }

    /// <summary> 
    /// Squishes the object with the parameters defined in the inspector.
    /// </summary>
    public void Squish()
        {
        ResetParameters();
        //start squish
        _squish = _squishSize;
        }

    /// <summary>
    /// Call this every frame to freeze the squish.
    /// </summary>
    public void FreezeSquish()
        {
        _squish = _squishSize;
        }

    /// <summary> 
    /// Squishes the object and overrides the parameters defined in the inspector.
    /// </summary>
    public void Squish(float squichinessLength, float squishSpeed, float lerpSpeed = 1f, float squishSize = 1f, float squishSlowdownSpeed = 0.1f)
        {
        //override vars
        _squichinessLength = squichinessLength;
        _squishSpeed = squishSpeed;
        _lerpSpeed = lerpSpeed;
        _squishSlowdownSpeed = squishSlowdownSpeed;
        _squishSize = squishSize;
        //start squish
        _squish = _squishSize;
        }

    private void ResetParameters()
        {
        _squishSlowdownSpeed = originalParameters[0];
        _squichinessLength = originalParameters[1];
        _squishSpeed = originalParameters[2];
        _lerpSpeed = originalParameters[3];
        _squishSize = originalParameters[4];
        }
    }