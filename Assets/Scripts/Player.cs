using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Transform cameraContainer;
    public Transform playerBody;
    public LayerMask groundLayerMask;

    Rigidbody _rigidbody;
    Animator _animator;

    [Header("Move")]
    public float moveSpeed;
    public float jumpPower;
    private Vector2 currentMoveInput;
    private Vector3 moveDir;
    private bool isJump;

    [Header("Look")]
    public float lookSensitivity;
    public float minCamXRot;
    public float maxCamXRot;
    private float curCamXRot;
    private float curCamYRot;
    private Vector2 mouseDelta;

    [Header("Health")]
    public float maxHealth;
    private float currentHealth;
    public Image healthBar;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        currentHealth = maxHealth;
    }

    private void Update()
    {
        HealthUpdate();

        
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
        Vector3 camForward = cameraContainer.forward;
        camForward.y = 0; // y축 성분을 제거하여 수평 방향만 사용
        camForward.Normalize(); // 방향 벡터를 정규화하여 크기를 1로 설정

        Vector3 camRight = cameraContainer.right;
        camRight.y = 0; // y축 성분을 제거하여 수평 방향만 사용
        camRight.Normalize(); // 방향 벡터를 정규화하여 크기를 1로 설정

        moveDir = camForward * currentMoveInput.y + camRight * currentMoveInput.x;
        moveDir *= moveSpeed;
        moveDir.y = _rigidbody.velocity.y;
        _rigidbody.velocity = moveDir;
        _animator.SetBool("IsMove", currentMoveInput != Vector2.zero);
        if (currentMoveInput != Vector2.zero)
        {
            TurnBody();
        }
    }

    void Look()
    {
        curCamXRot += mouseDelta.y * lookSensitivity;
        curCamXRot = Mathf.Clamp(curCamXRot, minCamXRot, maxCamXRot);
        curCamYRot += mouseDelta.x * lookSensitivity;
        cameraContainer.localEulerAngles = new Vector3(-curCamXRot, curCamYRot, 0);
    }

    void TurnBody()
    {
        playerBody.forward = moveDir;
        playerBody.localEulerAngles = new Vector3(0, playerBody.localEulerAngles.y, 0);
    }

    void HealthUpdate()
    {
        healthBar.fillAmount = currentHealth / 100;
    }

    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right* 0.2f) + (transform.up * 0.01f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                return true;
            }
        }
        return false;
    }

    public void OnPlayerMoveInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            currentMoveInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            currentMoveInput = Vector2.zero;
        }
    }

    public void OnPlayerLookInput(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnPlayerJumpInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            _rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            _animator.SetTrigger("DoJump");
        }
    }

    public void OnPlayerSprintInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            moveSpeed *= 1.5f;
            _animator.SetBool("IsSprint", true);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            moveSpeed = moveSpeed * 2 / 3;
            _animator.SetBool("IsSprint", false);
        }
    }
}
