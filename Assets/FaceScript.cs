using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceScript : MonoBehaviour {

    [SerializeField] private Sprite[] _faces;
    [SerializeField] private string[] _names;

    private SpriteRenderer _rend;

	// Use this for initialization
	void Start () {
        _rend = GetComponent<SpriteRenderer>();
	}

    /// <summary>
    /// Changes the current displayed face.
    /// </summary>
    /// <param name="name">Name of the face.</param>
    /// <param name="direction">Direction to flip the face to.</param>
    public void ChangeFace(string name, int direction = 1)
        {
        transform.localScale = new Vector3(Mathf.Sign(direction), 1, 1);
        if (name == "Talk")
            {
            //do talking
            if (Time.frameCount % 5 == 0)
                _rend.sprite = _rend.sprite == _faces[Array.IndexOf(_names, "Talk")] ? _faces[Array.IndexOf(_names, "Idle")] : _faces[Array.IndexOf(_names, "Talk")];
            }
        else
            _rend.sprite = _faces[Array.IndexOf(_names, name)];
        }
}
