using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, ITeamInterface
{
    private readonly int forwardSpeedString = Animator.StringToHash("forwardSpeed");
    private readonly int rightSpeedString = Animator.StringToHash("rightSpeed");
    private readonly int turnSpeedString = Animator.StringToHash("turnSpeed");
    private readonly int attackingString = Animator.StringToHash("attacking");
    private readonly int switchWeaponString = Animator.StringToHash("switchWeapon");

    [Space, Header("UI - Movement")]
    [SerializeField] JoyStick moveStick;
    [SerializeField] JoyStick aimStick;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float maxMoveSpeed = 20f;
    [SerializeField] float minMoveSpeed = 5f;
    [SerializeField] float animTurnSpeed = 30f;

    [Space, Header("UI - Health - Stamina")]
    [SerializeField] UIManager uiManager;
    [SerializeField] ValueGaugeUI_Player playerHealthBar;
    [SerializeField] ValueGaugeUI_Player playerStaminaBar;
    private HealthComponent healthComponent;
    private AbilityComponent abilityComponent;

    private Vector2 moveInput;
    private Vector2 aimInput;
    private CharacterController characterController;
    private Animator anim;
    private InventoryComponent inventory;
    private MovementComponent movementComponent;

    private float animatorTurnSpeed;

    private Camera mainCamera;
    [SerializeField] CameraController cameraController;

    [SerializeField] int teamId = 1;

    public int GetTeamID() => teamId;

    #region Testing
    //[SerializeField] ShopSystem testShopSys;
    //[SerializeField] ShopItem testItem;

    //private void TestPurchase()
    //{
    //    testShopSys.TryPurchase(testItem, GetComponent<CreditComponent>());
    //}

    #endregion

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        mainCamera = Camera.main;
        inventory = GetComponent<InventoryComponent>();
        movementComponent = GetComponent<MovementComponent>();

        moveStick.onStickInputValueUpdated += MoveStick_onStickInputValueUpdated;

        aimStick.onStickInputValueUpdated += AimStick_onStickInputValueUpdated;
        aimStick.onTaped += AimStick_onTaped;

        healthComponent = GetComponent<HealthComponent>();
        healthComponent.onHealthChanged += HealthComponent_onHealthChanged;
        healthComponent.onHealthEmpty += HealthComponent_onHealthEmpty;
        healthComponent.BoardcastHealthValueImmediately();

        abilityComponent = GetComponent<AbilityComponent>();
        abilityComponent.onStaminaChange += AbilityComponent_onStaminaChange;
        abilityComponent.BroadcastStaminaChangeImmediately();

        //Invoke(nameof(TestPurchase), 5f);
    }

    private void AbilityComponent_onStaminaChange(float newValue, float maxStamina)
    {
        playerStaminaBar.UpdateValueBar(newValue, 0f, maxStamina);
    }

    private void HealthComponent_onHealthEmpty(GameObject killer)
    {
        anim.SetLayerWeight(2, 1.0f);
        anim.SetTrigger(StringCollector.deadAnim);

        uiManager.SetGameplayControlEnabled(false);
    }

    private void HealthComponent_onHealthChanged(float currentHealth, float delta, float maxHealth)
    {
        playerHealthBar.UpdateValueBar(currentHealth, delta, maxHealth);
    }

    private void AimStick_onTaped()
    {
        StartSwitchWeapon();
    }

    private void StartSwitchWeapon()
    {
        anim.SetTrigger(switchWeaponString);
    }

    private void SwitchWeapon() //called by Animation Event (Switch Weapon Anim)
    {
        inventory.NextWeapon();
    }

    private void AttackPoint()
    {
        inventory.CurrentWeapon.Attack();
    }

    private void AimStick_onStickInputValueUpdated(Vector2 aimDirection)
    {
        aimInput = aimDirection;

        anim.SetBool(attackingString, aimInput.magnitude > 0f);
    }

    private void MoveStick_onStickInputValueUpdated(Vector2 moveDirection)
    {
        moveInput = moveDirection;
    }

    private Vector3 StickInputToWorldDirection(Vector2 input)
    {
        Vector3 rightDir = mainCamera.transform.right;
        Vector3 forwardDir = Vector3.Cross(rightDir, Vector3.up);

        return rightDir * input.x + forwardDir * input.y;
    }

    private void Update()
    {
        Vector3 moveDir = StickInputToWorldDirection(moveInput);
        Move(moveDir);
        Rotate(moveDir);

        float forward = Vector3.Dot(moveDir, transform.forward);
        float right = Vector3.Dot(moveDir, transform.right);

        anim.SetFloat(forwardSpeedString, forward);
        anim.SetFloat(rightSpeedString, right);

        RotateCameraFollowPlayer();
    }

    private void RotateCameraFollowPlayer()
    {
        if (moveInput.magnitude > 0f && 
            aimInput.magnitude < 0.2f && 
            cameraController != null)
        {            
            cameraController.AddYawInput(moveInput.x);            
        }
    }

    private void Rotate(Vector3 moveDir)
    {
        Vector3 aimDir = moveDir;
        if (aimInput.magnitude > 0f)
        {
            aimDir = StickInputToWorldDirection(aimInput);
        }

        float currentTurnSpeed = movementComponent.RotateToward(aimDir);

        //if (aimDir.magnitude > 0f)
        //{
        //    Quaternion previousRot = transform.rotation;

        //    transform.rotation =
        //        Quaternion.Slerp(transform.rotation,
        //                         Quaternion.LookRotation(aimDir, Vector3.up),
        //                         turnSpeed * Time.deltaTime);

        //    Quaternion currentRot = transform.rotation;
        //    float dir = Vector3.Dot(aimDir, transform.right) > 0f ? 1 : -1;
        //    float rotationDelta = Quaternion.Angle(previousRot, currentRot) * dir;
        //    currentTurnSpeed = rotationDelta / Time.deltaTime;        
        //}
        
        animatorTurnSpeed = Mathf.Lerp(animatorTurnSpeed, currentTurnSpeed, Time.deltaTime * animTurnSpeed);
        anim.SetFloat(turnSpeedString, animatorTurnSpeed);
    }

    private void Move(Vector3 moveDir)
    {
        characterController.Move(moveDir * moveSpeed * Time.deltaTime);
        characterController.Move(Vector3.down * 10f * Time.deltaTime);
    }

    public void AddMoveSpeed(float amt)
    {
        moveSpeed += amt;
        moveSpeed = Mathf.Clamp(moveSpeed, minMoveSpeed, maxMoveSpeed);
    }
}
