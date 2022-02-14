using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

public class PlayerControl : MonoBehaviour
{
    private static readonly int BasicSpell = Animator.StringToHash("BasicSpell");

    [SerializeField] private GameObject clickIndicator;
    [SerializeField] private GameObject basicSpell;
    [SerializeField] private float spellHeight;
    [SerializeField] private float spellForwardOffset;
    
    private NavMeshAgent _navMeshAgent;
    private Ray _lastRay;
    private float _spellCooldown = 1;
    private float _timeSinceLastSpell = 999;
    private PlayerAnimation _playerAnimation;

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _playerAnimation = GetComponent<PlayerAnimation>();
    }

    void Update()
    {
        if (_hasSpellBeenCast)
        {
            _hasSpellBeenCast = false;
        }
        HandleCoolDowns();
        HandleMove();
        HandleSpell();

        DrawRay(_lastRay);
    }

    private void HandleCoolDowns()
    {
        if (_timeSinceLastSpell > _spellCooldown)
        {
            _playerAnimation.CanMove = true;
        }
        else
        {
            _playerAnimation.CanMove = false;
        }
    }

    private void HandleMove()
    {
        if (_timeSinceLastSpell > _spellCooldown && Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit hit;
            Ray screenPointToRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            _lastRay = screenPointToRay;
            bool isRaycast = Physics.Raycast(screenPointToRay, out hit, 999);
            if (isRaycast)
            {
                _navMeshAgent.SetDestination(hit.point);
                clickIndicator.transform.position = hit.point;
            }
        }
    }

    private void HandleSpell()
    {
        _timeSinceLastSpell += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space) && _timeSinceLastSpell > _spellCooldown)
        {
            RaycastHit hit;
            Ray screenPointToRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            _lastRay = screenPointToRay;
            bool isRaycast = Physics.Raycast(screenPointToRay, out hit, 999);
            if (isRaycast)
            {
                _navMeshAgent.ResetPath();
                transform.LookAt(hit.point);
                _playerAnimation.StartBasicSpellAnimation();
                _navMeshAgent.velocity = new Vector3(0, 0, 0);
                _timeSinceLastSpell = 0;
                StartCoroutine(CreateSpell());
            }

        }
    }
    
    private bool _hasSpellBeenCast = false;

    private IEnumerator CreateSpell()
    {
        yield return new WaitForSeconds(0.4f);
        var offset = new Vector3(0, 1.1f, 0);
        var spellPos = transform.position + (transform.forward * 1.2f) + (transform.right * 0.2f) + offset;
        Instantiate(basicSpell, 
            spellPos, 
            transform.rotation);
        _hasSpellBeenCast = true;
    }

    void DrawRay(Ray ray)
    {
        Debug.DrawRay(ray.origin, ray.direction * 20, Color.magenta);
    }
}