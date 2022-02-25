using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public abstract class Spell : MonoBehaviour
{
    public SpellData data;
    private float _timeSinceLastSpell = 999;

    public bool IsCastingSpell { get; private set; }

    protected virtual void Update()
    {
        _timeSinceLastSpell += Time.deltaTime;
    }

    public bool IsOnCooldown()
    {
        return _timeSinceLastSpell < data.spellCooldown;
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
        navMeshAgent.ResetPath();
        animator.SetTrigger(data.animationName);
        navMeshAgent.velocity = new Vector3(0, 0, 0);
        _timeSinceLastSpell = 0;
        IsCastingSpell = true;
        StartCoroutine(CreateSpell(data.spellPrefab, playerInventory, navMeshAgent.gameObject));
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
        createdSpell.GetComponent<DamagingAttack>().Damage = data.damage;
        playerInventory.Items.ForEach(item => item.ApplyEffects(createdSpell));
        IsCastingSpell = false;
        Destroy(createdSpell, data.spellLifeTime);
    }
}
