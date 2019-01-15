using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegsScript : MonoBehaviour {
    
    [SerializeField] private float _repositionDistance;

    private Transform[] _feet;
    private LineRenderer _line;

    private int _index = 0;

    // Use this for initialization
    void Start () {
        //attach components
        _line = GetComponent<LineRenderer>();
        _feet = new Transform[] { transform.GetChild(0), transform.GetChild(1) };

        //detach feet from player
        foreach (Transform foot in _feet)
            {
            foot.SetParent(null);
            }

        //set line position count
        _line.positionCount = _feet.Length + 2;
	}

    private void Update()
        {
        _line.SetPosition(0, _feet[0].position);
        _line.SetPosition(1, transform.position + Vector3.left * _repositionDistance);
        _line.SetPosition(2, transform.position + Vector3.right * _repositionDistance);
        _line.SetPosition(3, _feet[1].position);
        }

    public void UpdateLegs(Vector3 inputDir)
        {

        //get position to resposition foot to
        Vector3 newPos = transform.position + ((_index == 0 ? Vector3.left : Vector3.right) * _repositionDistance) + ((inputDir) * _repositionDistance);
        //raycast to get ground
        RaycastHit hit;
        newPos.y += 0.2f;
        if (Physics.Raycast(newPos, Vector3.down, out hit, 3, LayerMask.GetMask("Default")))
            {
            _feet[_index].position = hit.point;
            }

        _index++;
        if (_index >= _feet.Length) _index = 0;
        }
}
