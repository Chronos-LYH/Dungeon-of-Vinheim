﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomType
{
    EmptyRoom,
    StartingRoom,
    BattleRoom,
    RewardRoom,
    BossRoom
}

// Now GameManager can create instance of ScriptableObject Room
[CreateAssetMenu]
public class Room : ScriptableObject
{
    // private int roomIndex;
    [SerializeField]
    private RoomType roomType;
    [SerializeField]
    private GameObject roomExtra;
    [SerializeField]
    private List<GameObject> enemyList;
    [SerializeField]
    private List<GameObject> itemList;
    
    public Room SetUp(RoomType type, GameObject extra=null){
        roomType = type;
        roomExtra = extra;
        // TODO: create enemies & items here
        enemyList = null;
        itemList = null;
        return this;
    }

    public bool Empty(){
        return roomType == RoomType.EmptyRoom;
    }
    // just for debug.log(), can delete later on
    public RoomType GetRoomType(){
        return roomType;
    }

    public void SetRoomActive(bool value){
        // rotate the content of the boss room, so player will be in the front of the boss
        // iN fact the player can only enter from direction "down" or "left"
        if(value){
            if(roomType == RoomType.BossRoom){
                float angleY = GameManager.instance.playerTransform.position == GameManager.positions[0] ? 180 : 270;
                roomExtra.transform.rotation = Quaternion.Euler(0, angleY, 0);
            }
        }
        roomExtra.SetActive(value);
        if (enemyList != null)
        {
            foreach (var GameObject in enemyList) GameObject.SetActive(value);
        }
        if (itemList != null)
        {
            foreach (var GameObject in itemList) GameObject.SetActive(value);
        }
    }
}