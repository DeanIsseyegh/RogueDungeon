using System;
using System.Collections;
using StarterAssets;
using UnityEngine;
using UnityEngine.AI;

public abstract class Spell : MonoBehaviour
{
    public SpellData data;
    private Coroutine _spellCoroutine;
    public float TimeSinceLastSpell { get; private set; } = 999;

    public bool IsCastingSpell { get; protected set; }

    protected virtual void Update()
    {
        TimeSinceLastSpell += Time.deltaTime;
    }

    public bool IsOnCooldown()
    {
        return TimeSinceLastSpell < data.spellCooldown;
    }

    public void Cast(Animator animator, PlayerInventory playerInventory, PlayerMana playerMana)
    {
        if (!IsOnCooldown() && playerMana.CurrentMana >= data.manaCost)
        {
            IsCastingSpell = true;
            playerMana.UseMana(data.manaCost);
            Cast(null, animator, playerInventory);
        }
    }

    public void Cast(NavMeshAgent navMeshAgent, Animator animator, PlayerInventory playerInventory)
    {
        if (IsOnCooldown()) return;
        if (navMeshAgent != null && navMeshAgent.enabled)
        {
            navMeshAgent.ResetPath();
            navMeshAgent.velocity = new Vector3(0, 0, 0);
        }

        animator.SetTrigger(data.animationName);
        TimeSinceLastSpell = 0;
        IsCastingSpell = true;
        _spellCoroutine = StartCoroutine(CreateSpell(data.spellPrefab, playerInventory, animator.gameObject));
    }

    private IEnumerator CreateSpell(GameObject spellPrefab, PlayerInventory playerInventory, GameObject caster)
    {
        yield return new WaitForSeconds(data.spellStartUp);
        if (caster == null) yield break;
        var yOffset = new Vector3(0, data.spellSpawnOffset.y, 0);
        var spellPos = caster.transform.position + (caster.transform.forward * data.spellSpawnOffset.z)
                                                 + (caster.transform.right * data.spellSpawnOffset.x) + yOffset;
        
        var spellRotation = CalculateSpellRotation(caster, spellPos);
        GameObject createdSpell = Instantiate(spellPrefab,
            spellPos,
            spellRotation);

        ApplyEffectsToSpell(createdSpell, playerInventory);
        Destroy(createdSpell, data.spellLifeTime);
        yield return new WaitForSeconds(0.2f);
        IsCastingSpell = false;
    }

    protected abstract void ApplyEffectsToSpell(GameObject spellPrefab, PlayerInventory playerInventory);

    protected abstract Quaternion CalculateSpellRotation(GameObject caster, Vector3 spellPos);

    private void OnDisable()
    {
        if (_spellCoroutine != null)
        {
            StopCoroutine(_spellCoroutine);
            IsCastingSpell = false;
            TimeSinceLastSpell = 0;
        }
    }
}