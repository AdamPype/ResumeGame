using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueScript : MonoBehaviour {

    [SerializeField] private GameObject _letterPrefab;
    [SerializeField] private string[] _text;
    [SerializeField] private float _textDelay = 5;
    [SerializeField] private float _waitTime = 3;
    private float _waitTimer;
    private bool _waitForInput;
    private bool _doTimer = false;

    private int _index = 0;
    private int _line1Index = 0;
    private int _line2Index = 0;
    private int _line3Index = 0;

    [SerializeField] private HorizontalLayoutGroup _line1Object, _line2Object = null, _line3Object = null;
    [SerializeField] private RectTransform _mask; //_outline = null;
    //private Squishy _squish;
    private string _line1 = null, _line2 = null, _line3 = null;
    private List<LetterScript> _letterDump = new List<LetterScript>();
    private bool _isClosing = false;

    private LetterScript.TextEffect _currEffect = LetterScript.TextEffect.Normal;
    private bool _secondaryCol = false;
    private bool _rainbow = false;
    [SerializeField] private Color _defColor;
    [SerializeField] private Color _secColor;

    private SoundManager _snd;

    // Use this for initialization
    void Start () {
        _waitTimer = _waitTime;
        _mask.gameObject.SetActive(false);
        _mask.transform.localScale = new Vector2(0, 1);
        _snd = GetComponent<SoundManager>();
    }

    public void Say(string[] text, bool waitForInput = false, TextAlignment alignment = TextAlignment.Left)
        {
        //text
        _text = text;
        _index = 0;
        ResetLetters();
        PrintLine(true);

        //input
        _waitForInput = waitForInput;

        //alignment
        foreach (HorizontalLayoutGroup line in new HorizontalLayoutGroup[] { _line1Object, _line2Object, _line3Object})
            {
            switch (alignment)
                {
                case TextAlignment.Left:
                    line.childAlignment = TextAnchor.MiddleLeft;
                    break;
                case TextAlignment.Center:
                    line.childAlignment = TextAnchor.MiddleCenter;
                    break;
                case TextAlignment.Right:
                    line.childAlignment = TextAnchor.MiddleRight;
                    break;
                default:
                    break;
                }
            }
        }

    [ContextMenu("Test")]
    public void SayAgain()
        {
        _index = 0;
        ResetLetters();
        PrintLine(true);
        }

    public void StopSaying()
        {
        ResetLetters();
        _mask.gameObject.SetActive(false);
        _mask.transform.localScale = new Vector2(0, 1);
        }

    private void ResetLetters()
        {
        for (int i = 0; i < _letterDump.Count; i++)
            {
            if (_letterDump[i])
                Destroy(_letterDump[i].transform.parent.gameObject);
            }
        _letterDump.Clear();
        _line1Index = 0;
        _line2Index = 0;
        _line3Index = 0;
        _line1 = null;
        _line2 = null;
        _line3 = null;
        _waitTimer = _waitTime;

        _secondaryCol = false;
        _rainbow = false;
        _currEffect = LetterScript.TextEffect.Normal;
        
        _mask.gameObject.SetActive(true);
        }

    private void PrintLine(bool first = false)
        {
        //reset letters
        ResetLetters();

        //next line
        if (!first)
            {
            _index++;
            }

        //assign text to lines
        int splitIndex = _text[_index].IndexOf("#");
        int splitIndex2 = _text[_index].IndexOf("#", _text[_index].IndexOf("#") + 1);
        if (splitIndex != -1)
            {
            if (splitIndex2 != -1)
                {
                _line1 = _text[_index].Substring(0, splitIndex + 1);
                _line2 = _text[_index].Substring(splitIndex + 1, (splitIndex2 + 1) - (splitIndex + 1));
                _line3 = _text[_index].Substring(splitIndex2 + 1);
                }
            else
                {
                _line1 = _text[_index].Substring(0, splitIndex + 1);
                _line2 = _text[_index].Substring(splitIndex + 1);
                }
            }
        else
            {
            _line1 = _text[_index];
            }
        }

    public bool IsTalking()
        {
        if (_mask.transform.localScale.x > 0)
            {
            return true;
            }
        else return false;
        }

    // Update is called once per frame
    void Update () {
        

        //do text
        if (_line1 != null)
            {
            if (Time.frameCount % _textDelay == 0)
                {

                

                if (_line1Index < _line1.Length)
                    {
                    LetterScript letter = Instantiate(_letterPrefab, _line1Object.transform).transform.GetChild(0).GetComponent<LetterScript>();
                    letter.Letter = _line1.ToCharArray()[_line1Index];
                    letter.Index = _line1Index;

                    Commands(letter);

                    _line1Index++;
                    if (letter.gameObject)
                        _letterDump.Add(letter);
                    }
                else if (_line2 != null)
                    {
                    if (_line2Index < _line2.Length)
                        {
                        LetterScript letter = Instantiate(_letterPrefab, _line2Object.transform).transform.GetChild(0).GetComponent<LetterScript>();
                        letter.Letter = _line2.ToCharArray()[_line2Index];
                        letter.Index = _line2Index;

                        Commands(letter);

                        _line2Index++;
                        if (letter.gameObject)
                            _letterDump.Add(letter);
                        }
                    else if (_line3 != null)
                        {
                        if (_line3Index < _line3.Length)
                            {
                            LetterScript letter = Instantiate(_letterPrefab, _line3Object.transform).transform.GetChild(0).GetComponent<LetterScript>();
                            letter.Letter = _line3.ToCharArray()[_line3Index];
                            letter.Index = _line3Index;

                            Commands(letter);

                            _line3Index++;
                            if (letter.gameObject)
                                _letterDump.Add(letter);
                            }
                        else
                            {
                            _doTimer = true;
                            }
                        }
                    else
                        {
                        _doTimer = true;
                        }
                    }
                else
                    {
                    _doTimer = true;
                    }

                if (_doTimer)
                    {
                    //count down timer if not wait for input
                    if (!_waitForInput)
                        _waitTimer -= Time.deltaTime;
                    //press to continue
                    if (Input.GetButtonDown("Fire2"))
                        {
                        _waitTimer = 0;
                        }

                    //stop or go to next text
                    if (_waitTimer <= 0)
                        {
                        _doTimer = false;
                        if (_index < _text.Length - 1)
                            {
                            PrintLine();
                            }
                        else
                            {
                            //close animation
                            _mask.transform.localScale = Vector2.Lerp(_mask.transform.localScale, new Vector2(0, 1), 0.4f);
                            _isClosing = true;

                            if (Vector2.Distance(_mask.transform.localScale, new Vector2(0, 1)) < 0.1f)
                                {
                                _isClosing = false;
                                ResetLetters();
                                _mask.gameObject.SetActive(false);
                                _mask.transform.localScale = new Vector2(0, 1);
                                }
                            }

                        }
                    }
                }
            }

        //appear animation
        if (_mask.gameObject.activeSelf && !_isClosing)
            {
            _mask.transform.localScale = Vector2.Lerp(_mask.transform.localScale, Vector2.one, 0.1f);
            }
        }

    private void Commands(LetterScript letter)
        {
        if (!_rainbow)
            {
            if (_secondaryCol)
                {
                letter.InitEffects(_currEffect, _secColor);
                }
            else
                {
                letter.InitEffects(_currEffect, _defColor);
                }
            }
        else
            {
            letter.InitEffects(_currEffect, true);
            }

        //sound
        //if (Time.frameCount % 3 == 0)
        //    _snd.Play("Blip", false, 0);

        switch (letter.Letter)
            {
            case '#':
                Destroy(letter.transform.parent.gameObject);
                break;
            case '£':
                Destroy(letter.transform.parent.gameObject);
                if (_currEffect == LetterScript.TextEffect.Wavy)
                    {
                    _currEffect = LetterScript.TextEffect.Normal;
                    }
                else
                    _currEffect = LetterScript.TextEffect.Wavy;
                break;
            case '%':
                Destroy(letter.transform.parent.gameObject);
                if (_currEffect == LetterScript.TextEffect.Shaky)
                    {
                    _currEffect = LetterScript.TextEffect.Normal;
                    }
                else
                    _currEffect = LetterScript.TextEffect.Shaky;
                break;
            case '*':
                Destroy(letter.transform.parent.gameObject);
                _secondaryCol = !_secondaryCol;
                break;
            case '$':
                Destroy(letter.transform.parent.gameObject);
                _rainbow = !_rainbow;
                break;
            case '°':
                Destroy(letter.transform.parent.gameObject);
                break;
            default:
                break;
            }
        }
    }
