using Unity.Netcode;
using UnityEngine;

/// <summary>
/// Just a crappy character controller for the video
/// </summary>
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    public BoxCollider corner;
    public GameObject egg;
    public Vector3 eggPlace;

    public Transform meeple;

    public bool hadEgg;

    [Header("Dash")]
    [SerializeField] private float dashPower;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _cam = Camera.main;
        corner = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        _input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        meeple = transform.Find("Egg");

        if (meeple == null)
        {
            hadEgg = false;
            //corner.enabled = false;
        }
        else
        {
            hadEgg = true;
            //corner.enabled = true;

        }



    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleRotation();
    }

    #region Movement

    [SerializeField] private float _acceleration = 80;
    [SerializeField] private float _maxVelocity = 10;
    private Vector3 _input;
    private Rigidbody _rb;

    private void HandleMovement()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _rb.AddForce(transform.forward * dashPower, ForceMode.Impulse);
        }

        _rb.velocity += _input.normalized * (_acceleration * Time.deltaTime);
        _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, _maxVelocity);
    }

    #endregion

    #region Rotation

    [SerializeField] private float _rotationSpeed = 450;
    private Plane _groundPlane = new Plane(Vector3.up, Vector3.zero);
    private Camera _cam;

    private void HandleRotation()
    {
        var ray = _cam.ScreenPointToRay(Input.mousePosition);

        if (_groundPlane.Raycast(ray, out var enter))
        {
            var hitPoint = ray.GetPoint(enter);

            var dir = hitPoint - transform.position;
            var rot = Quaternion.LookRotation(dir);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, _rotationSpeed * Time.deltaTime);
        }
    }

    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Dead Wall"))
        {
            //destroy object
        }
    }
}