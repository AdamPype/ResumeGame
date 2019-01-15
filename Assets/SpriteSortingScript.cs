using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSortingScript : MonoBehaviour {

    [SerializeField] private SpriteRenderer[] _sprites;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float depth = -transform.position.z * 10;
        for (int i = 0; i < _sprites.Length; i++)
            {
            _sprites[i].sortingOrder = (int)depth + i;
            }
	}
}
