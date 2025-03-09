using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed;                 // 플레이어 이동 속도
    private Vector3 currentMoveInput;       // 현재 눌린 이동 값
    private Vector3 playerDir;              // 플레이어 방향
    Animator anim;

    [Header("Look")]
    public float mouseSensitivity;          // 마우스 민감도
    public float minXRot;                   // 위 아래 화면 움직임 제한
    public float maxXRot;
    private float currentCamXRot;           // 현재 카메라 x축 회전 (수직 움직임)
    private float currentCamYRot;           // 현재 카메라 y축 회전 (수평 움직임)
    private Vector2 mouseDelta;             // 마우스 움직임 변화량
    public Transform cameraContainer;

    [Header("Turn")]
    public Transform playerBody;            // 플레이어 몸체


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
