using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed;                 // �÷��̾� �̵� �ӵ�
    private Vector3 currentMoveInput;       // ���� ���� �̵� ��
    private Vector3 playerDir;              // �÷��̾� ����
    Animator anim;

    [Header("Look")]
    public float mouseSensitivity;          // ���콺 �ΰ���
    public float minXRot;                   // �� �Ʒ� ȭ�� ������ ����
    public float maxXRot;
    private float currentCamXRot;           // ���� ī�޶� x�� ȸ�� (���� ������)
    private float currentCamYRot;           // ���� ī�޶� y�� ȸ�� (���� ������)
    private Vector2 mouseDelta;             // ���콺 ������ ��ȭ��
    public Transform cameraContainer;

    [Header("Turn")]
    public Transform playerBody;            // �÷��̾� ��ü


    Rigidbody _rigidbody;


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (_rigidbody.velocity.magnitude > 0.5f)
        {
            Turn();
            anim.SetBool("IsMove", true);
        }
        else
        {
            anim.SetBool("IsMove", false);
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        Look();
    }

    void Move()
    {
        playerDir = cameraContainer.forward * currentMoveInput.y + cameraContainer.right * currentMoveInput.x;
        playerDir *= moveSpeed;
        playerDir.y = _rigidbody.velocity.y;
        
        _rigidbody.velocity = playerDir;
    }

    void Look()
    {
        currentCamXRot += mouseDelta.y * mouseSensitivity;
        currentCamYRot += mouseDelta.x * mouseSensitivity;
        currentCamXRot = Mathf.Clamp(currentCamXRot, minXRot, maxXRot);
        cameraContainer.localEulerAngles = new Vector3(-currentCamXRot, currentCamYRot, 0);
    }

    void Turn()
    {
        playerBody.localEulerAngles = new Vector3(0, cameraContainer.localEulerAngles.y + currentMoveInput.y, 0);
    }

    public void playerMoveInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            currentMoveInput = context.ReadValue<Vector3>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            currentMoveInput = Vector3.zero;
        }
    }

    public void playerLookInput(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }
}
