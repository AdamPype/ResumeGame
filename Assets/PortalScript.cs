using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalScript : MonoBehaviour {

    [SerializeField] private float _speed;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _timeToTeleport;
    [SerializeField] private string _link;

    private float _rotationSpeed;
    private bool _colliding = false;
    private Squishy _squish;
    private Transform _sprite;

    private SavePosition _save;

	// Use this for initialization
	void Start () {
        _rotationSpeed = _speed;
        _sprite = transform.Find("Sprite");
        _squish = _sprite.GetComponent<Squishy>();
        _save = GameObject.FindGameObjectWithTag("Player").GetComponent<SavePosition>();
	}
	
	// Update is called once per frame
	void Update () {
        _sprite.Rotate(Vector3.forward, _rotationSpeed);

        _rotationSpeed = Mathf.MoveTowards(_rotationSpeed, _colliding ? _maxSpeed : _speed, (_maxSpeed / _timeToTeleport) * Time.deltaTime);

        if (_rotationSpeed == _maxSpeed)
            {
            //go to link
            Application.OpenURL(_link);
            if (_save.transform.position.x > 15)
                {
                _save.Save();
                }
            _colliding = false;
            _rotationSpeed = _speed;
            }
	}

    private void OnTriggerEnter(Collider other)
        {
        if (other.CompareTag("Player"))
            {
            _colliding = true;
            _squish.Squish();
            }
        }

    private void OnTriggerExit(Collider other)
        {
        if (other.CompareTag("Player"))
            {
            _colliding = false;
            _squish.Squish();
            _rotationSpeed = _speed;
            }
        }
    }
