using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Handles all Input and sends input data to other classes which require it.
public class InputManager : MonoBehaviour
{
    PlayerInput playerInput;
    PlayerInput.OnFootActions onFoot;
    PlayerMotor playerMotor;
    PlayerLook playerLook;
    PlayerInteract playerInteract;
    GunManager gunManager;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;

        playerMotor = GetComponent<PlayerMotor>();
        playerLook = GetComponent<PlayerLook>();
        playerInteract = GetComponent<PlayerInteract>();
        gunManager = GetComponent<GunManager>();

        onFoot.Jump.performed += ctx => playerMotor.Jump();
        onFoot.Interact.performed += ctx => playerInteract.InteractWithCurrentObject();

        //Weapon Inputs
        onFoot.Reload.performed += ctx => gunManager.Reload();
    }


    private void Update()
    {
        playerMotor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
        playerLook.ProcessLook(onFoot.Look.ReadValue<Vector2>());

        // Weapon Inputs
        gunManager.WeaponSway(onFoot.Look.ReadValue<Vector2>());
        if (onFoot.LeftClick.IsPressed())
            gunManager.FireGun(onFoot.LeftClick.WasPerformedThisFrame());

        gunManager.BobWeapon(onFoot.Movement.ReadValue<Vector2>());
    }


    private void OnEnable()
    {
        onFoot.Enable();
    }
    private void OnDisable()
    {
        onFoot.Disable();
    }
}
