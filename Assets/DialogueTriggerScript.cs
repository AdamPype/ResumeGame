using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTriggerScript : MonoBehaviour {

    [SerializeField] private bool _onlyOnce = true;
    [SerializeField, TextArea] private string[] _text;
    [SerializeField] private TextAlignment _alignment;
    private DialogueScript _dialogue;

	// Use this for initialization
	void Start () {
        _dialogue = GameObject.FindGameObjectWithTag("Dialogue").GetComponent<DialogueScript>();
	}

    private void OnTriggerEnter(Collider other)
        {
        if (other.CompareTag("Player"))
            {
            _dialogue.Say(_text, false, _alignment);
            if (_onlyOnce) Destroy(gameObject);
            }
        }
    }
