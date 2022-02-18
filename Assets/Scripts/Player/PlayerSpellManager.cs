using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class PlayerSpellManager : MonoBehaviour
{
    [SerializeField] private List<Spell> spellPrefabs;
    [SerializeField] private SpellObjManager spellObjManager;
    [SerializeField] protected float spellHeightOffset;
    [SerializeField] protected float spellForwardOffset;
    [SerializeField] protected float spellRightOffset;

    private Camera _mainCamera;
    private NavMeshAgent _playerNavMeshAgent;
    private PlayerAnimation _playerAnimation;
    private PlayerInventory _inventory;
    private float _spellCooldown = 1;
    private float _timeSinceLastSpell = 999;
    private Dictionary<KeyCode, Spell> _spellInputsMap;


    private void Start()
    {
        List<KeyCode> spellInputs = new List<KeyCode>()
        {
            KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4
        };
        _spellInputsMap = spellPrefabs.Select((spell, index) =>
        {
            var spellKeyCode = spellInputs[index];
            return new {spellKeyCode, spell};
        }).ToDictionary(e => e.spellKeyCode, e => e.spell);
        spellObjManager.AddSpells(spellPrefabs);

        _mainCamera = Camera.main;
        _playerNavMeshAgent = GetComponent<NavMeshAgent>();
        _playerAnimation = GetComponent<PlayerAnimation>();
        _inventory = GetComponent<PlayerInventory>();
    }

    private void Update()
    {
        _timeSinceLastSpell += Time.deltaTime;
    }

    public bool IsNotCastingSpell()
    {
        return _timeSinceLastSpell > _spellCooldown;
    }

    public void HandleSpell(Transform playerTransform)
    {
        if (IsNotCastingSpell())
        {
            KeyCode spellKeyPressed = _spellInputsMap.Keys.FirstOrDefault(Input.GetKeyDown);
            if (spellKeyPressed == KeyCode.None) return;
            Spell spellPrefab = _spellInputsMap[spellKeyPressed];
            Cast(spellPrefab);
        }
    }

    public void Cast(Spell spellPrefab)
    {
        RaycastHit hit;
        Ray screenPointToRay = _mainCamera.ScreenPointToRay(Input.mousePosition);
        bool isRaycast = Physics.Raycast(screenPointToRay, out hit, 100);
        if (isRaycast)
        {
            _playerNavMeshAgent.ResetPath();
            transform.LookAt(hit.point);
            _playerAnimation.StartBasicSpellAnimation();
            _playerNavMeshAgent.velocity = new Vector3(0, 0, 0);
            _timeSinceLastSpell = 0;
            StartCoroutine(CreateSpell(spellPrefab));
        }
    }

    private IEnumerator CreateSpell(Spell spellPrefab)
    {
        yield return new WaitForSeconds(0.4f);
        var yOffset = new Vector3(0, spellHeightOffset, 0);
        var spellPos = this.transform.position + (this.transform.forward * spellForwardOffset) + (this.transform.right * spellRightOffset) + yOffset;
        Spell spell = Instantiate(spellPrefab,
            spellPos,
            transform.rotation);
        _inventory.Items.ForEach(item => item.ApplyEffects(spell));
    }
}