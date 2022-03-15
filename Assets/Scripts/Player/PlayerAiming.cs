using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using Werewolf.StatusIndicators.Components;

public class PlayerAiming : MonoBehaviour
{
    [SerializeField] private MousePositionTracker mousePositionTracker;
    private GameObject _player;
    private bool _isAiming;
    private Action _finishAimingCallback;
    private SplatManager _splatManager;
    private NavMeshAgent _playerNavMeshAgent;

    private SpellIndicator _currentSpellIndicator;
    private Action _cancelAimingCallback;


    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _splatManager = _player.GetComponentInChildren<SplatManager>();
        // _playerNavMeshAgent = _player.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        var posOnFloor = mousePositionTracker.MousePosOnFloor();
        if (_isAiming)
        {
            Vector3 direction = posOnFloor - transform.position;
            direction.y = 0;
            _player.transform.forward = direction;

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                StopAiming();
                _finishAimingCallback.Invoke();
            } else if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                StopAiming();
                _cancelAimingCallback.Invoke();
            }
        }
    }

    private void StopAiming()
    {
        _isAiming = false;
        // _playerNavMeshAgent.enabled = true;
        _splatManager.CancelSpellIndicator();
    }

    public void StartAiming(Action finishAimingCallback, Action cancelAimingCallback, SpellData spellData)
    {
        _isAiming = true;
        // _playerNavMeshAgent.enabled = false;
        _splatManager.SelectSpellIndicator(spellData.spellIndicator);
        _currentSpellIndicator = _splatManager.CurrentSpellIndicator;
        _finishAimingCallback = finishAimingCallback;
        _cancelAimingCallback = cancelAimingCallback;
    }

}