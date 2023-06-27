using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private FixedJoystick _joystick;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _moveSpeed = 4f;

    private bool isSitting = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        rb.velocity = new Vector3(_joystick.Horizontal * _moveSpeed, rb.velocity.y, _joystick.Vertical * _moveSpeed);

        if (_joystick.Horizontal != 0 || _joystick.Vertical != 0)
        {
            rb.rotation = Quaternion.LookRotation(rb.velocity);
            _animator.SetBool("isWalking", true);
        }
        else
            _animator.SetBool("isWalking", false);
    }

    public void SitDown()
    {
        if(!isSitting)
        {
            _animator.SetBool("isSitting", true);
            isSitting = true;
        }
        else
        {
            _animator.SetBool("isSitting", false);
            isSitting = false;
        }
    }
}