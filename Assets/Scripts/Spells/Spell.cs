using System;
using System.Collections;
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

    public virtual void Cast(NavMeshAgent navMeshAgent, Animator animator,
        PlayerInventory playerInventory, PlayerMana playerMana)
    {
        Cast(navMeshAgent, animator, playerInventory);
    }

    public virtual void Cast(NavMeshAgent navMeshAgent, Animator animator,
        PlayerInventory playerInventory)
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

    private IEnumerator CreateSpell(GameObject spellPrefab, PlayerInventory playerInventory, GameObject agent)
    {
        yield return new WaitForSeconds(data.spellStartUp);
        if (agent == null) yield break;
        var yOffset = new Vector3(0, data.spellSpawnOffset.y, 0);
        var spellPos = agent.transform.position + (agent.transform.forward * data.spellSpawnOffset.z)
                                                + (agent.transform.right * data.spellSpawnOffset.x) + yOffset;
        GameObject createdSpell = Instantiate(spellPrefab,
            spellPos,
            agent.transform.rotation);
        ApplyEffectsToSpell(createdSpell, playerInventory);
        IsCastingSpell = false;
        Destroy(createdSpell, data.spellLifeTime);
    }

    protected abstract void ApplyEffectsToSpell(GameObject spellPrefab, PlayerInventory playerInventory);

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