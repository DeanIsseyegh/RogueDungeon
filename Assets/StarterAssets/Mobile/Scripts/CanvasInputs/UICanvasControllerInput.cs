using UnityEngine;
using UnityEngine.Serialization;

namespace StarterAssets
{
    public class UICanvasControllerInput : MonoBehaviour
    {

        [FormerlySerializedAs("starterAssetsInputs")] [Header("Output")]
        public InputsController inputsController;

        public void VirtualMoveInput(Vector2 virtualMoveDirection)
        {
            inputsController.MoveInput(virtualMoveDirection);
        }

        public void VirtualLookInput(Vector2 virtualLookDirection)
        {
            inputsController.LookInput(virtualLookDirection);
        }

        public void VirtualJumpInput(bool virtualJumpState)
        {
            inputsController.JumpInput(virtualJumpState);
        }

        public void VirtualSprintInput(bool virtualSprintState)
        {
            inputsController.SprintInput(virtualSprintState);
        }
        
    }

}
