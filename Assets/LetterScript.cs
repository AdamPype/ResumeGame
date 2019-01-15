using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetterScript : MonoBehaviour {

    public char Letter;
    [HideInInspector] public int Index;
    [HideInInspector] public enum TextEffect {
        Wavy,
        Shaky,
        Normal
        }
    private TextEffect _effect = TextEffect.Normal;
    private Text _text;
    private Squishy _squish;
    private bool _squished = false;
    private RectTransform _rect;
    private Vector2 _startPos;

    [SerializeField] float _amplitude;
    [SerializeField] float _frequency;
    [SerializeField] float _offset;
    [Space, SerializeField] float _shakeSize;
    [Space, SerializeField, Range(5, 200)] int _rainbowHueOffset;


    // Use this for initialization
    void Awake () {
        _text = GetComponent<Text>();
        _rect = GetComponent<RectTransform>();
        _startPos = _rect.anchoredPosition;
        _squish = GetComponent<Squishy>();
	}

    private void Start()
        {
        _text.text = Letter.ToString();
        }


    public void InitEffects(TextEffect effect, Color color)
        {
        _effect = effect;
        _text.color = color;
        }

    public void InitEffects(TextEffect effect, bool rainbow)
        {
        Color textColor = Color.black;
        if (rainbow)
            {
            float hue = (float)Index * ((float)_rainbowHueOffset / 255);
            hue %= 1;
            textColor = Color.HSVToRGB(hue, 1, 1);
            }
        InitEffects(effect, textColor);
        }

    private void Update()
        {
        Vector2 newPos = _rect.anchoredPosition;
        switch (_effect)
            {
            case TextEffect.Wavy:
                newPos.y = _startPos.y + (_amplitude * Mathf.Sin((Time.frameCount * _frequency) + (Index * _offset)));
                break;
            case TextEffect.Shaky:
                newPos.y = _startPos.y + Random.Range(-_shakeSize, _shakeSize);
                break;
            case TextEffect.Normal:
                break;
            default:
                break;
            }
        
        _rect.anchoredPosition = newPos;

        if (!_squished)
            {
            _squished = true;
            _squish.Squish();
            }
        }
    }
