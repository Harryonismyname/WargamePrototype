using UnityEngine;
using UnityEngine.InputSystem;
using System;

namespace PolearmStudios.Input
{
    [RequireComponent(typeof(PlayerInput))]
    public class InputManager : MonoBehaviour
    {
        public InputScheme CurrentInputScheme { get; private set; }

        public event Action OnRightTriggerPulled;
        public event Action OnRightTriggerReleased;
        public event Action OnLeftTriggerPulled;
        public event Action OnLeftTriggerReleased;


        // TODO:Remove Unused Actions
        public event Action OnSelectPressed;
        public event Action OnStartPressed;
        public event Action OnJumpStart;
        public event Action OnJumpCanceled;
        public event Action OnSprintStart;
        public event Action OnSprintCanceled;
        public event Action OnCrouchStart;
        public event Action OnCrouchCanceled;
        public event Action OnSlot4Pressed;

        public event Action<Vector2> OnMove;
        public event Action<Vector2> OnAim;

        public event Action OnControlsLockout;

        // TODO: Remove Unused InputActions
        PlayerInput input;
        InputAction aimInput;
        InputAction moveInput;
        InputAction rightTrigger;
        InputAction leftTrigger;
        InputAction select;
        InputAction start;
        InputAction jump;
        InputAction sprint;
        InputAction crouch;
        InputAction slot4;

        Vector2 movement;
        Vector2 aim;

        public bool Locked { get; private set; }

        readonly string MOUSE_AND_KEYBOARD_BINDING = "DefaultPC";
        readonly string CONTROLLER_BINDING = "ControllerInput";

        private void Awake()
        {
            CurrentInputScheme = InputScheme.MOUSE_AND_KEYBOARD;
            input = GetComponent<PlayerInput>();
            CacheActions();
            SubscribeToEvents();

        }

        private void Start() => OnControlsChanged(input);

        private void FixedUpdate()
        {
            if (input != null)
            {
                movement = moveInput.ReadValue<Vector2>();
                if (movement.magnitude > 0.01f && !Locked) OnMove?.Invoke(movement);
            }
        }

        private void Update()
        {
            if (input != null)
            {
                aim = aimInput.ReadValue<Vector2>();
                if (aim.magnitude > 0.01f) OnAim?.Invoke(aim);
            }
        }

        private void OnDestroy() => UnsubscribeToEvents();

        private void SetControlScheme(int scheme) => CurrentInputScheme = scheme >= 1 ? InputScheme.CONTROLLER : InputScheme.MOUSE_AND_KEYBOARD;

        public void LockoutControls()
        {
            Locked = true;
            OnControlsLockout?.Invoke();
        }

        public void UnlockControls()
        {
            Locked = false;
        }

        public void OnControlsChanged(PlayerInput input)
        {
            int newScheme = 0;
            if (input.currentControlScheme == CONTROLLER_BINDING)
            {
                LockMouse();
                newScheme = 1;
            }
            SetControlScheme(newScheme);
        }

        public void LockMouse()
        {
            if (CurrentInputScheme != InputScheme.MOUSE_AND_KEYBOARD) { return; }
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void UnlockMouse()
        {
            if (CurrentInputScheme != InputScheme.MOUSE_AND_KEYBOARD) { return; }
            Cursor.lockState = CursorLockMode.None;
        }

        private void RightTriggerPull(InputAction.CallbackContext context) { if (!Locked) OnRightTriggerPulled?.Invoke(); }

        private void ReleaseRightTrigger(InputAction.CallbackContext context) { if (!Locked) OnRightTriggerReleased?.Invoke(); }

        private void LeftTriggerPull(InputAction.CallbackContext context) { if (!Locked) OnLeftTriggerPulled?.Invoke(); }

        private void ReleaseLeftTrigger(InputAction.CallbackContext context) { if (!Locked) OnLeftTriggerReleased?.Invoke(); }

        private void SelectPerformed(InputAction.CallbackContext context) { if (!Locked) OnSelectPressed?.Invoke(); }

        private void StartPerformed(InputAction.CallbackContext context) { if (!Locked) OnStartPressed?.Invoke(); }

        private void BeginJump(InputAction.CallbackContext context) { if (!Locked) OnJumpStart?.Invoke(); }

        private void CancelJump(InputAction.CallbackContext context) { if (!Locked) OnJumpCanceled?.Invoke(); }

        private void BeginSprint(InputAction.CallbackContext context) { if (!Locked) OnSprintStart?.Invoke(); }

        private void CancelSprint(InputAction.CallbackContext context) { if (!Locked) OnSprintCanceled?.Invoke(); }

        private void BeginCrouch(InputAction.CallbackContext context) { if (!Locked) OnCrouchStart?.Invoke(); }
        private void CancelCrouch(InputAction.CallbackContext context) { if (!Locked) OnCrouchCanceled?.Invoke(); }

        private void ActivateSlot4(InputAction.CallbackContext context) { if (!Locked) OnSlot4Pressed?.Invoke(); }

        private void SubscribeToEvents()
        {
            rightTrigger.performed += RightTriggerPull;
            rightTrigger.canceled += ReleaseRightTrigger;

            leftTrigger.performed += LeftTriggerPull;
            leftTrigger.canceled += ReleaseLeftTrigger;

            jump.performed += BeginJump;
            jump.canceled += CancelJump;

            sprint.performed += BeginSprint;
            sprint.canceled += CancelSprint;

            crouch.performed += BeginCrouch;
            crouch.canceled += CancelCrouch;
        }

        private void UnsubscribeToEvents()
        {
            rightTrigger.performed -= RightTriggerPull;
            rightTrigger.canceled -= ReleaseRightTrigger;

            leftTrigger.performed -= LeftTriggerPull;
            leftTrigger.canceled -= ReleaseLeftTrigger;

            jump.performed -= BeginJump;
            jump.canceled -= CancelJump;

            sprint.performed -= BeginSprint;
            sprint.canceled -= CancelSprint;

            crouch.performed -= BeginCrouch;
            crouch.canceled -= CancelCrouch;
        }

        private void CacheActions()
        {
            moveInput = input.actions["Walk"];
            aimInput = input.actions["Look"];
            rightTrigger = input.actions["RightHand"];
            leftTrigger = input.actions["LeftHand"];
            jump = input.actions["Jump"];
            sprint = input.actions["Sprint"];
            crouch = input.actions["Crouch"];
        }
    }

    public enum InputScheme
    {
        None = 0,
        MOUSE_AND_KEYBOARD,
        CONTROLLER,
    }
}

