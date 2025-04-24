using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
        public bool weapon;
        public bool aim;

        [Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

        private void Start()
        {
            SetCursorState(cursorLocked);
        }

#if ENABLE_INPUT_SYSTEM
        public void OnMove(InputValue value)
		{
			if(cursorLocked == false)
			{
				return;
			}
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
        {
            if (cursorLocked == false)
            {
                return;
            }
            if (cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
        {
            if (cursorLocked == false)
            {
                return;
            }
            JumpInput(value.isPressed);
        }

        public void OnSprint(InputValue value)
        {
            if (cursorLocked == false)
            {
                return;
            }
            SprintInput(value.isPressed);
        }

        public void OnWeapon(InputValue value)
        {
            if (cursorLocked == false)
            {
                return;
            }
            WeaponInput(value.isPressed);
        }

        public void OnAim(InputValue value)
        {
            if (cursorLocked == false)
            {
                return;
            }
            AimInput(value.isPressed);
        }

		public void OnEsc(InputValue value)
		{
			if (value.isPressed)
			{
				cursorLocked = !cursorLocked;

                SetCursorState(cursorLocked);
            }
		}
#endif


        public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
        }

        public void SprintInput(bool newSprintState)
        {
            sprint = newSprintState;
        }

        public void WeaponInput(bool newWeaponState)
        {
            weapon = newWeaponState;
        }

        public void AimInput(bool newAimState)
        {
            aim = newAimState;
        }

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}