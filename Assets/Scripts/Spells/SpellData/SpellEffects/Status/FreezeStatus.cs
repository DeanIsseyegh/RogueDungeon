using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FreezeStatus : MonoBehaviour, Status
{
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private EnemyAI _enemyAI;
    private EnemyAttack _enemyAttack;
    private Spell _spell;
    public float LifeTime { private get; set; }

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _enemyAI = GetComponent<EnemyAI>();
        _enemyAttack = GetComponent<EnemyAttack>();
        _spell = GetComponent<Spell>();
        EnableComponents(false);
    }

    void Update()
    {
        Apply();
    }

    public void Apply()
    {
        LifeTime -= Time.deltaTime;
        if (LifeTime <= 0)
        {
            Remove();
        }
    }

    public void Remove()
    {
        EnableComponents(true);
        Destroy(this);
    }

    private void EnableComponents(bool isEnabled)
    {
        _navMeshAgent.enabled = isEnabled;
        _animator.enabled = isEnabled;
        _enemyAI.enabled = isEnabled;
        if (_enemyAttack != null)
            _enemyAttack.enabled = isEnabled;
        if (_spell != null)
            _spell.enabled = isEnabled;
    }

}
