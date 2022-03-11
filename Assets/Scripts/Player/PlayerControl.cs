using UnityEngine;
using UnityEngine.AI;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private GameObject clickIndicator;
    [SerializeField] private MousePositionTracker mousePositionTracker;

    [SerializeField] private float playerSpeed = 20f;
    [SerializeField] private float mouseSensitivty = 5f;

    private NavMeshAgent _navMeshAgent;
    private AnimationHandler _animationHandler;
    private bool _canPlayerMove = true;
    private PlayerMovement _playerMovement;
    private PlayerSpellManager _playerSpellManager;
    private Rigidbody _rb;

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animationHandler = GetComponent<AnimationHandler>();
        _playerSpellManager = GetComponent<PlayerSpellManager>();
        _rb = GetComponent<Rigidbody>();
        // _playerMovement = new PlayerMovement(_navMeshAgent, clickIndicator);
    }

    void Update()
    {
        _canPlayerMove = !_playerSpellManager.IsCastingSpell();
        _animationHandler.CanMove = _canPlayerMove;
        HandleMove();
        HandleSpell();
    }

    private void HandleMove()
    {
        if (_canPlayerMove)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivty;
            // _rb.MoveRotation(Quaternion.Euler(0, mouseX, 0));
            transform.Rotate(Vector3.up, mouseX);

            float xInput = Input.GetAxis("Horizontal");
            float zInput = Input.GetAxis("Vertical");

            var moveDirection = (transform.forward * zInput) + (transform.right * xInput);
            _rb.velocity = moveDirection * Time.deltaTime * playerSpeed;
            // if (Input.GetKey(KeyCode.W))
            // {
                // _rb.velocity = transform.forward * Time.deltaTime * playerSpeed;
            // } else if (Input.GetKey(KeyCode.S))
            // {
                // _rb.velocity = -(transform.forward * Time.deltaTime * playerSpeed);
            // }
            
        }
    }

    private void HandleSpell()
    {
        if (Input.anyKeyDown)
        {
            _playerSpellManager.HandleSpell();
        }
    }

}