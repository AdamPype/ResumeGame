using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {
    
    [SerializeField] private float _acceleration;
    [SerializeField] private float _drag;
    [SerializeField] private float _maximumXZVelocity;
    [SerializeField] private float _jumpHeight;

    [SerializeField, Space] private float _bobFrequency;
    [SerializeField] private float _bobAmplitude;
    [SerializeField] private float _bobAngle;

    private Transform _absoluteTransform;
    private CharacterController _char;
    private LegsScript _legs;
    private Transform _sprite;
    private FaceScript _face;
    private DialogueScript _dialogue;

    private Vector3 _velocity = Vector3.zero;
    private Vector3 _inputMovement;
    private bool _jump;
    private bool _isJumping;
    private Vector3 _startPosSprite;
    private bool _run;
    private Vector3 _faceStartPos;

    void Start()
        {

        //attach components
        _char = GetComponent<CharacterController>();
        _absoluteTransform = Camera.main.transform;
        _sprite = transform.Find("Sprite");
        _legs = _sprite.Find("Legs").GetComponent<LegsScript>();
        _face = _sprite.Find("Face").GetComponent<FaceScript>();
        _dialogue = GameObject.FindGameObjectWithTag("Dialogue").GetComponent<DialogueScript>();
        _faceStartPos = _face.transform.localPosition;

        //set init vars
        _startPosSprite = _sprite.localPosition;
        }

    private void Update()
        {
        //get input
        _inputMovement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        //mouse and touch controls
        Vector2 mouseMovement = Camera.main.ScreenToViewportPoint(Input.mousePosition) - Camera.main.WorldToViewportPoint(transform.position);
        if (Input.GetMouseButton(0)) _inputMovement = new Vector3(mouseMovement.x, 0, mouseMovement.y).normalized;
        //run
        _run = Input.GetButton("Run");

        Animate();
        }

    private void Animate()
        {
        string face = "Idle";

        //on move
        if (_inputMovement != Vector3.zero)
            {
            //do legs
            if (Time.frameCount % (_run ? 4 : 8) == 0)
                _legs.UpdateLegs(_inputMovement);

            //bob sprite
            float runMultiplier = _run ? 2 : 1;
            _sprite.localPosition = _startPosSprite + Vector3.up * (_bobAmplitude * Mathf.Sin(Time.frameCount * (_bobFrequency * runMultiplier)));
            _sprite.localEulerAngles = Vector3.forward * _bobAngle * Mathf.Sin(Time.frameCount * ((_bobFrequency / 2) * runMultiplier));

            //move face
            _face.transform.localPosition = Vector3.Lerp(_face.transform.localPosition, _faceStartPos + (new Vector3(_inputMovement.x, _inputMovement.z/2).normalized * 0.05f), 0.2f);

            //change face
            if (_inputMovement.x != 0)
                face = _inputMovement.x > 0 ? "Right" : "Left";
            else
                face = "Idle";
            }
        else
            {
            _face.transform.localPosition = _faceStartPos;
            }

        //dialogue talking
        if (_dialogue.IsTalking()) face = "Talk";

        //change face
        _face.ChangeFace(face);

        }

    void FixedUpdate()
        {
        ApplyGround();
        ApplyGravity();
        ApplyMovement();
        ApplyDragOnGround();
        ApplyJump();
        LimitXZVelocity();

        DoMovement();
        }


    private void ApplyGround()
        {
        if (_char.isGrounded)
            {
            //ground velocity
            _velocity -= Vector3.Project(_velocity, Physics.gravity);
            }
        }

    private void ApplyGravity()
        {
        if (!_char.isGrounded)
            {
            //apply gravity
            _velocity += Physics.gravity * Time.deltaTime;
            }
        }

    private void ApplyMovement()
        {
        if (_char.isGrounded)
            {
            //get relative rotation from camera
            Vector3 xzForward = Vector3.Scale(_absoluteTransform.forward, new Vector3(1, 0, 1));
            Quaternion relativeRot = Quaternion.LookRotation(xzForward);

            //move in relative direction
            Vector3 relativeMov = relativeRot * _inputMovement;
            _velocity += relativeMov * _acceleration * Time.deltaTime * (_run ? 2 : 1);
            }

        }

    private void LimitXZVelocity()
        {
        Vector3 yVel = Vector3.Scale(_velocity, Vector3.up);
        Vector3 xzVel = Vector3.Scale(_velocity, new Vector3(1, 0, 1));

        xzVel = Vector3.ClampMagnitude(xzVel, _maximumXZVelocity);

        _velocity = xzVel + yVel;
        }

    private void ApplyDragOnGround()
        {
        if (_char.isGrounded)
            {
            //drag
            _velocity = _velocity * (1 - _drag * Time.deltaTime); //same as lerp
            }
        }

    private void ApplyJump()
        {
        if (_char.isGrounded && _jump)
            {
            _velocity.y += Mathf.Sqrt(2 * Physics.gravity.magnitude * _jumpHeight);
            _jump = false;
            _isJumping = true;
            }
        else if (_char.isGrounded)
            {
            _isJumping = false;
            }
        }

    private void DoMovement()
        {
        //do velocity / movement on character controller
        Vector3 movement = _velocity * Time.deltaTime;
        _char.Move(movement);
        }
    }
