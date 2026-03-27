using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;


public class FirstPersonController : MonoBehaviour
{
    public bool ShowHUD = true;

    [Header("Default sprite the player reticle uses.")]
    public Sprite interactIcon;
    [Header("The sprite used when looking at an interactable.")]
    public Sprite defaultreticleIcon;

    [Header("Move Variables")]
    public float moveSpeed = 5f;
    public float runSpeed = 10f;
    [Tooltip("Default value it'll be at on load.")]
    public bool canMove = true;
    [Tooltip("Default value it'll be at on load.")]
    public bool canRun = true;
    [Space(15)]

    [Header("Jump & Fall Variables")]
    public float jumpSpeed = 8f;
    public float fallAccelMultiplier = 2f;
    [Tooltip("Default value it'll be at on load.")]
    public bool canJump = true;
    [Space(15)]

    [Header("Look Variables")]
    public float yawSensitivity = 1f;
    private float yawAngle = 0f;
    public bool invertPitch = false;
    public float pitchSensitivity = 1f;
    public float maxPitchAngle = 85f;
    public float minPitchAngle = -85f;
    [Tooltip("Default value it'll be at on load.")]
    public bool canLook = true;

    [Space(15)]
    private Rigidbody playerRB;
    [Header("Important Reference Variables.\nDon't change these unless you know what you're doing.")]
    public MeshFilter PlayerHand;
    public Image playerReticle;

    #region UnderTheHoodVariables
    //Script variable declarations--don't touch these unless you know what you're doing.
    private GroundDetector groundDetector;
    private Camera cam;
    private float pitchAngle = 0f;
    private int CurrentHandItem = 0;
    private IInteractable PreviousInteractable;
    private Transform cameraTransform;

    [HideInInspector]
    public bool PlayerActive = true;
    //-----------------------------------------------------------------------------------
    #endregion


    // Start is called before the first frame update
    void Start()
    {

        //Assigning------------------------------
        playerRB = GetComponent<Rigidbody>();
        groundDetector = GetComponentInChildren<GroundDetector>();
        cam = Camera.main;
        cameraTransform = cam.transform;
        //---------------------------------------

        //Cursor locking-------------------------
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        //---------------------------------------

    }

    // Update is called once per frame
    void Update()
    {
        HUDHandler.HUD.gameObject.SetActive(ShowHUD);

        if (PlayerActive)
        {
            //Movement functions---------------------
            if (canMove)
                VelocityMove();
            if (canJump)
                Jump();
            //---------------------------------------

            //Camera functions-----------------------
            if (canLook)
            {
                HorizontalCameraControls();
                VerticalCameraControls();
                RaycastInteract();
            }
            //---------------------------------------
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, yawAngle, 0f);
            cameraTransform.localRotation = Quaternion.Euler(pitchAngle, 0f, 0f);
            playerRB.velocity = Vector3.zero;
        }

        //Fall Gravity Multiplier----------------
        if (!groundDetector.IsGrounded() && playerRB.velocity.y <= 0f)
        {
            playerRB.AddForce(new Vector3(0f, -fallAccelMultiplier, 0f), ForceMode.Acceleration);
        }
        //---------------------------------------

        //Hand & Inventory-----------------------
        HandChange();
        //---------------------------------------
    }

    private void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && groundDetector.IsGrounded())
        {
            playerRB.velocity = new Vector3(playerRB.velocity.x, jumpSpeed, playerRB.velocity.z);
        }
    }

    private void VelocityMove()
    {
        Vector3 dir = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            dir += transform.forward;
        }

        if (Input.GetKey(KeyCode.A))
        {
            dir -= transform.right;
        }

        if (Input.GetKey(KeyCode.S))
        {
            dir -= transform.forward;
        }

        if (Input.GetKey(KeyCode.D))
        {
            dir += transform.right;
        }

        if(Input.GetKey(KeyCode.LeftShift) && canRun)
        {
            dir = dir.normalized * runSpeed;
        }
        else
        {
            dir = dir.normalized * moveSpeed;
        }

        //Apply velocity into desired x/z direction but allow y velocity to be the same.
        playerRB.velocity = new Vector3(dir.x, playerRB.velocity.y, dir.z);
    }

    private void HorizontalCameraControls()
    {
        yawAngle += Input.GetAxis("Mouse X") * yawSensitivity;
        transform.rotation = Quaternion.Euler(0f, yawAngle, 0f);
    }

    private void VerticalCameraControls()
    {
        pitchAngle -= Input.GetAxis("Mouse Y") * pitchSensitivity * (invertPitch ? -1f : 1f);
        pitchAngle = Mathf.Clamp(pitchAngle, minPitchAngle, maxPitchAngle);
        cameraTransform.localRotation = Quaternion.Euler(pitchAngle, 0f, 0f);
    }

    private void RaycastInteract()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        Sprite Reticle = defaultreticleIcon;
        Vector2 reticleScale = new Vector2(15,15);

        if (Physics.Raycast(ray, out hitInfo, 3))
        {
            IInteractable interactable = hitInfo.collider.gameObject.GetComponent<IInteractable>();

            if (interactable != null)
            {
                Reticle = interactIcon;
                reticleScale = new Vector2(100, 100);

                interactable.Highlight();

                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactable.Interact();
                }

                if (PreviousInteractable != interactable && PreviousInteractable != null)
                {
                    PreviousInteractable.Unhighlight();
                }
                PreviousInteractable = interactable;
            }
        }
        else
        {
            if(PreviousInteractable != null)
            {
                PreviousInteractable.Unhighlight();
            }
        }

        playerReticle.sprite = Reticle;
        playerReticle.gameObject.GetComponent<RectTransform>().sizeDelta = reticleScale;
    }

    private void HandChange()
    {
        PlayerHand.mesh = null;

        if (HUDHandler.HUD.Inventory[CurrentHandItem] != null)
            PlayerHand.mesh = HUDHandler.HUD.Inventory[CurrentHandItem].gameObject.GetComponent<MeshFilter>().mesh;

        if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (CurrentHandItem == HUDHandler.HUD.Inventory.Length - 1)
                CurrentHandItem = 0;
            else
                ++CurrentHandItem;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (CurrentHandItem == 0)
                CurrentHandItem = HUDHandler.HUD.Inventory.Length - 1;
            else
                --CurrentHandItem;
        }
    }

    public void EnablePlayer()
    {
        PlayerActive = true;
    }

    public void DisablePlayer()
    {
        PlayerActive = false;
    }
}