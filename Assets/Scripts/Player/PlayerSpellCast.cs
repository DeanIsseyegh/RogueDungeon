using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PlayerSpellCast : MonoBehaviour
{
    [SerializeField] private GameObject basicSpell;
    [SerializeField] private float spellHeightOffset;
    [SerializeField] private float spellForwardOffset;
    [SerializeField] private float spellRightOffset;
    [SerializeField] private Camera mainCamera;
    
    private NavMeshAgent _playerNavMeshAgent;
    private PlayerAnimation _playerAnimation;
    private float _spellCooldown = 1;
    private float _timeSinceLastSpell = 999;

    private void Start()
    {
        _playerNavMeshAgent = GetComponent<NavMeshAgent>();
        _playerAnimation = GetComponent<PlayerAnimation>();
    }

    private void Update()
    {
        _timeSinceLastSpell += Time.deltaTime;
    }

    public void CastSpell(Transform playerTransform)
    {
        if (IsNotCastingSpell())
        {
            RaycastHit hit;
            Ray screenPointToRay = mainCamera.ScreenPointToRay(Input.mousePosition);
            bool isRaycast = Physics.Raycast(screenPointToRay, out hit, 100);
            if (isRaycast)
            {
                _playerNavMeshAgent.ResetPath();
                playerTransform.LookAt(hit.point);
                _playerAnimation.StartBasicSpellAnimation();
                _playerNavMeshAgent.velocity = new Vector3(0, 0, 0);
                _timeSinceLastSpell = 0;
                StartCoroutine(CreateSpell(playerTransform));
            }
        }
    }

    public bool IsNotCastingSpell()
    {
        return _timeSinceLastSpell > _spellCooldown;
    }

    private IEnumerator CreateSpell(Transform playerTransform)
    {
        yield return new WaitForSeconds(0.4f);
        var yOffset = new Vector3(0, spellHeightOffset, 0);
        var spellPos = playerTransform.position + (playerTransform.forward * spellForwardOffset) + (playerTransform.right * spellRightOffset) + yOffset;
        Instantiate(basicSpell,
            spellPos,
            playerTransform.rotation);
    }
}