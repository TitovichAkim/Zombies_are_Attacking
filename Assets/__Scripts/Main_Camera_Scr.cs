using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_Camera_Scr : MonoBehaviour {


    [SerializeField] private FixedJoystick _joystick;

    public GameObject _player;
    public float damping = 1.5f;

    public Vector2 correct = Vector2.zero;

    private void Start() {
        transform.position = _player.transform.position;
        }

    void FixedUpdate() {

        if (_joystick.Horizontal != 0 && _joystick.Vertical != 0) {
            correct = new Vector2(_joystick.Horizontal, _joystick.Vertical).normalized*2;
        }
            Vector3 target = new Vector3(_player.transform.position.x + correct.x, _player.transform.position.y + correct.y, -10);
        transform.position = Vector3.Lerp(transform.position, target, damping * Time.deltaTime);
    }
}
