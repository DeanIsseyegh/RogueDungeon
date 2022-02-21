using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public abstract class Spell : MonoBehaviour
{
    [SerializeField] protected GameObject spellPrefab;
    [SerializeField] protected Vector3 spellSpawnOffset;
    [SerializeField] protected float spellLifeTime;
    [SerializeField] protected float spellStartUp = 0.4f;
    [SerializeField] protected string animationName = "BasicSpell";

    public SpellScriptableObj SpellAttributes;

    private float _spellCooldown = 1;
    private float _timeSinceLastSpell = 999;

    public bool IsCastingSpell { get; private set; }

    protected virtual void Update()
    {
        _timeSinceLastSpell += Time.deltaTime;
    }

    public bool IsOnCooldown()
    {
        return _timeSinceLastSpell < _spellCooldown;
    }

    public virtual void Cast(NavMeshAgent navMeshAgent, AnimationHandler animationHandler, 
        PlayerInventory playerInventory)
    {
        if (IsOnCooldown()) return;
        navMeshAgent.ResetPath();
        animationHandler.SetTriggerAnimation(animationName);
        navMeshAgent.velocity = new Vector3(0, 0, 0);
        _timeSinceLastSpell = 0;
        IsCastingSpell = true;
        StartCoroutine(CreateSpell(spellPrefab, playerInventory, navMeshAgent.gameObject));
    }

    private IEnumerator CreateSpell(GameObject spellPrefab, PlayerInventory playerInventory, GameObject agent)
    {
        yield return new WaitForSeconds(spellStartUp);
        var yOffset = new Vector3(0, spellSpawnOffset.y, 0);
        var spellPos = agent.transform.position + (agent.transform.forward * spellSpawnOffset.z) 
                                                  + (agent.transform.right * spellSpawnOffset.x) + yOffset;
        GameObject createdSpell = Instantiate(spellPrefab,
            spellPos,
            agent.transform.rotation);
        playerInventory.Items.ForEach(item => item.ApplyEffects(createdSpell));
        IsCastingSpell = false;
        Destroy(createdSpell, spellLifeTime);
    }
}
