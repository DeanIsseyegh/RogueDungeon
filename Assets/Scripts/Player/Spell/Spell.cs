using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Spell : MonoBehaviour
{
    [SerializeField] private GameObject _spellPrefab;
    [SerializeField] protected float spellHeightOffset;
    [SerializeField] protected float spellForwardOffset;
    [SerializeField] protected float spellRightOffset;
    [SerializeField] protected float spellLifeTime;
    [SerializeField] private GameObject _player;
    [SerializeField] private MousePositionTracker mousePositionTracker;

    public SpellScriptableObj SpellAttributes;

    private NavMeshAgent _playerNavMeshAgent;
    private PlayerAnimation _playerAnimation;
    private PlayerInventory _playerInventory;


    private float _spellCooldown = 1;
    private float _timeSinceLastSpell = 999;

    public bool IsCastingSpell;

    private void Awake()
    {
        _playerNavMeshAgent = _player.GetComponent<NavMeshAgent>();
        _playerAnimation = _player.GetComponent<PlayerAnimation>();
        _playerInventory = _player.GetComponent<PlayerInventory>();
    }

    private void Update()
    {
        _timeSinceLastSpell += Time.deltaTime;
    }

    public bool IsOnCooldown()
    {
        return _timeSinceLastSpell < _spellCooldown;
    }

    public void Cast()
    {
        if (IsOnCooldown()) return;
        _playerNavMeshAgent.ResetPath();
        _player.transform.LookAt(mousePositionTracker.MousePos());
        _playerAnimation.StartBasicSpellAnimation();
        _playerNavMeshAgent.velocity = new Vector3(0, 0, 0);
        _timeSinceLastSpell = 0;
        IsCastingSpell = true;
        StartCoroutine(CreateSpell(_spellPrefab));
    }

    private IEnumerator CreateSpell(GameObject spellPrefab)
    {
        yield return new WaitForSeconds(0.4f);
        var yOffset = new Vector3(0, spellHeightOffset, 0);
        var spellPos = _player.transform.position + (_player.transform.forward * spellForwardOffset) + (_player.transform.right * spellRightOffset) + yOffset;
        GameObject createdSpell = Instantiate(spellPrefab,
            spellPos,
            _player.transform.rotation);
        _playerInventory.Items.ForEach(item => item.ApplyEffects(createdSpell));
        IsCastingSpell = false;
        Destroy(createdSpell, spellLifeTime);
    }
}
