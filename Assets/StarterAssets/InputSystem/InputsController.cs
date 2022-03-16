using UnityEngine;
using UnityEngine.Serialization;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class InputsController : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool attack1;
		public bool attack2;
		public bool attack3;
		public bool attack4;

		[Header("Movement Settings")]
		public bool analogMovement;

#if !UNITY_IOS || !UNITY_ANDROID
		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;
#endif

		public bool AnyAttack()
		{
			return attack1 | attack2 | attack3 | attack4;
		}

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

		public void OnAttack1(InputValue value)
		{
			AttackInput1(value.isPressed);
		}
		
		public void OnAttack2(InputValue value)
		{
			AttackInput2(value.isPressed);
		}
		
		public void OnAttack3(InputValue value)
		{
			AttackInput3(value.isPressed);
		}
		
		public void OnAttack4(InputValue value)
		{
			AttackInput4(value.isPressed);
		}


#else
	// old input sys if we do decide to have it (most likely wont)...
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
		
		private void AttackInput1(bool valueIsPressed)
		{
			attack1 = valueIsPressed;
		}
		
		private void AttackInput2(bool valueIsPressed)
		{
			attack2 = valueIsPressed;
		}
		
		private void AttackInput3(bool valueIsPressed)
		{
			attack3 = valueIsPressed;
		}
		
		private void AttackInput4(bool valueIsPressed)
		{
			attack4 = valueIsPressed;
		}


#if !UNITY_IOS || !UNITY_ANDROID

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}

#endif

	}
	
}