using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.State
{
    public class PlayerStateCtx
    {
        public Animator Animator { get; }
        public GameObject Player { get; }
        public Camera MainCamera { get; }
        public CharacterController CharController { get; }
        public ThirdPersonController ThirdPersonController { get; }
        public PlayerSpellManager SpellManager { get; }
        public PlayerInventory Inventory { get; }
        public InputsController InputsController { get; }
        public PlayerMana Mana { get; }
        public PlayerInput PlayerInput { get; }

        public PlayerStateCtx(GameObject player, Animator animator, Camera mainCamera, InputsController inputsController)
        {
            MainCamera = mainCamera;
            Animator = animator;
            Player = player;
            CharController = player.GetComponent<CharacterController>();
            ThirdPersonController = player.GetComponent<ThirdPersonController>();
            SpellManager = player.GetComponent<PlayerSpellManager>();
            Inventory = player.GetComponent<PlayerInventory>();
            Mana = player.GetComponent<PlayerMana>();
            PlayerInput = player.GetComponent<PlayerInput>();
            InputsController = inputsController;
        }
    }
}
