using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Level.RoomEvents;
using UnityEngine;

public abstract class CollectibleRoomStartEvent : RoomStartEvent
{
    private Vector3 _collectibleYOffset;
    private Vector3 _collectibleXOffset;
    private RandomGameObjGenerator CollectibleGenerator { get; set; }
    public Vector3 MiddleOfRoomPos { private get; set; }

    private bool _isCollectiblesSpawned;

    private List<GameObject> _createdCollectibles;
    private UIManager _uiManager;

    private int numOfCollectibles = 2;

    protected abstract string GetCollectibleGeneratorTag();

    protected virtual void Awake()
    {
        CollectibleGenerator = GameObject.FindWithTag(GetCollectibleGeneratorTag())
            .GetComponent<RandomGameObjGenerator>();
        _uiManager = GameObject.FindWithTag("UIManager").GetComponent<UIManager>();
        _collectibleYOffset = Vector3.up * 1.5f;
        _collectibleXOffset = Vector3.right * 1.5f;
        _createdCollectibles = new List<GameObject>();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CloseEntrance();
            SpawnCollectibles();
            _isCollectiblesSpawned = true;
            List<Collectible> collectibles = _createdCollectibles
                .Select(it => it.GetComponent<OnPickUp>())
                .Select(it => it.Collectible)
                .ToList();
            _uiManager.ShowChoices(collectibles);
            GetComponent<BoxCollider>().enabled = false;
        }
    }

    private void SpawnCollectibles()
    {
        Vector3 spawnPos = MiddleOfRoomPos + _collectibleYOffset + _collectibleXOffset;
        GameObject lastCreatedObj = null;
        for (int i = 0; i < numOfCollectibles; i++)
        {
            if (CollectibleGenerator.ObjectsLeft() == 0) break;
            GameObject createdCollectible = CollectibleGenerator.Generate(spawnPos, lastCreatedObj);
            spawnPos -= _collectibleXOffset * 2;
            _createdCollectibles.Add(createdCollectible);
            lastCreatedObj = createdCollectible;
        }
    }

    public bool HasEventFinished()
    {
        return _isCollectiblesSpawned &&
               (_createdCollectibles.Any(it => it == null) || _createdCollectibles.Count == 0);
    }

    public void HideChoiceUi()
    {
        _uiManager.HideChoices();
    }

    public void RemoveCollectibles()
    {
        _createdCollectibles.ForEach(it => Destroy(it));
    }
}