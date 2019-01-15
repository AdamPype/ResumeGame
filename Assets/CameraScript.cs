using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    private Transform _player;
    private Vector3 _vel;

    [SerializeField] private float _speed;
    [SerializeField] private float _focusTime;
    private float _focusTimer;

    // Use this for initialization
    void Start () {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 targPos = transform.position;
        Vector2 _viewportPos = Camera.main.WorldToViewportPoint(_player.position);
        if (_focusTimer > 0 || _viewportPos.x > 0.7f || _viewportPos.x < 0.3f)
            {
            targPos.x = _player.position.x;
            }
        transform.position = Vector3.SmoothDamp(transform.position, targPos, ref _vel, _focusTimer > 0 ? 0.4f : _speed);
        if (_focusTimer > 0) _focusTimer -= Time.deltaTime;
	}

    public void FocusPlayer()
        {
        _focusTimer = _focusTime;
        }
    }
