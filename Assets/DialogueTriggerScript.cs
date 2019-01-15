using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTriggerScript : MonoBehaviour {

    [SerializeField] private bool _onlyOnce = true;
    [SerializeField, TextArea] private string[] _text;
    [SerializeField] private TextAlignment _alignment;
    private DialogueScript _dialogue;
    private CameraScript _cam;

	// Use this for initialization
	void Start () {
        _dialogue = GameObject.FindGameObjectWithTag("Dialogue").GetComponent<DialogueScript>();
        _cam = Camera.main.GetComponent<CameraScript>();
	}

    private void OnTriggerEnter(Collider other)
        {
        if (other.CompareTag("Player"))
            {
            _dialogue.Say(_text, false, _alignment);
            _cam.FocusPlayer();
            if (_onlyOnce) Destroy(gameObject);
            }
        }
    }
