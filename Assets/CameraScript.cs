using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    private Transform _player;
    private Vector3 _vel;

    [SerializeField] private float _speed;
    private DialogueScript _dialogue;

    // Use this for initialization
    void Start () {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _dialogue = GameObject.FindGameObjectWithTag("Dialogue").GetComponent<DialogueScript>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 targPos = transform.position;
        Vector2 _viewportPos = Camera.main.WorldToViewportPoint(_player.position);
        if (_dialogue.IsTalking() || _viewportPos.x > 0.7f || _viewportPos.x < 0.3f)
            {
            targPos.x = _player.position.x;
            }
        transform.position = Vector3.SmoothDamp(transform.position, targPos, ref _vel, _dialogue.IsTalking() ? 0.2f : _speed);
	    }
    }
