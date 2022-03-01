using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationManager : MonoBehaviour
{
    [SerializeField] private Vector3 startingPos;
    [SerializeField] private RandomSpellGenerator randomSpellGenerator;
    [SerializeField] private RandomItemGenerator randomItemGenerator;
    [SerializeField] private RoomGenerator roomGenerator;
    [SerializeField] private GameObject player;

    [SerializeField] private List<RoomData> roomsData;

    void Start()
    {
        GenerateRooms();
        // GenerateRandomSpell();
        // GenerateRandomItem();
    }

    public void GenerateRooms()
    {
        List<GeneratedRoom> generatedRooms = new List<GeneratedRoom>();
        
        GeneratedRoom firstRoom = roomGenerator
            .GenerateFloor(startingPos, roomsData[0]);
        generatedRooms.Add(firstRoom);
        GeneratedRoom prevRoom = firstRoom;
        for (var i = 1; i < roomsData.Count; i++)
        {
            var newRoomStartPos = roomGenerator.CalculateStartPosBasedOnPrevRoom(prevRoom, roomsData[i]);
            var newRoom = roomGenerator.GenerateFloor(newRoomStartPos, roomsData[i]);
            generatedRooms.Add(newRoom);
            prevRoom = newRoom;
        }
        
        generatedRooms.ForEach(room => room.BuildNavmesh());
    }

    private void GenerateRandomSpell()
    {
        Vector3 startingSpellForwardOffset = Vector3.forward * -1 * 3;
        Vector3 startingSpellHeightOffset = Vector3.up * 1.5f;
        Vector3 startingSpellRightOffset = Vector3.right;
        randomSpellGenerator.Generate(player.transform.position + startingSpellForwardOffset +
                                      startingSpellHeightOffset + startingSpellRightOffset);
    }

    private void GenerateRandomItem()
    {
        Vector3 startingItemForwardOffset = Vector3.forward * -1 * 3;
        Vector3 startingItemHeightOffset = Vector3.up * 1.5f;
        Vector3 startingItemRightOffset = Vector3.right * -1;
        randomItemGenerator.Generate(player.transform.position + startingItemForwardOffset +
                                     startingItemHeightOffset + startingItemRightOffset);
    }

    // Update is called once per frame
    void Update()
    {
    }
}