﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public Button exitGameButton;
    public Button minimapButton;
    public GameObject mainMenu;
    public GameObject minimap;
    public GameObject minimapMenu;
    private Stack<GameObject> menuStack = new Stack<GameObject>();

    void Start(){
        exitGameButton.onClick.AddListener(delegate (){
            OnClick(exitGameButton.gameObject);
        });
        minimapButton.onClick.AddListener(delegate (){
            OnClick(minimapButton.gameObject);
        });
        DrawMinimap();
    }

    void OnClick(GameObject go)
    {
        if (go == exitGameButton.gameObject)
        {
            // TODO: PERMISSION: can add listener to CONFIRM button and invoke later
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
        else if(go == minimapButton.gameObject){
            HideAndActive(minimapMenu);
        }
    }

    public void HideAndActive(GameObject menu){
        if(menuStack.Count != 0) menuStack.Peek().SetActive(false);
        menuStack.Push(menu);
        menu.SetActive(true);
    }

    /// <summary>
    /// Draw map of the dungeon on GUI minimap
    /// </summary>
    public void DrawMinimap(){
        int mapSize = GameManager.instance.GetMapSize();
        RectTransform minimapRT = minimap.GetComponent<RectTransform>();
        float width = minimapRT.rect.width / mapSize;
        float height = minimapRT.rect.height / mapSize;
        float posX = - minimapRT.rect.width / 2 + width / 2;
        float posY = - minimapRT.rect.width / 2 + width / 2;
        Debug.LogFormat("width:{0}, height:{1}, posX:{2}, posY:{3}", width, height, posX, posY);
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                int roomIdx = mapSize * i + j;
                GameObject room = new GameObject("room" + roomIdx);
                room.transform.SetParent(minimapRT);
                room.transform.localScale = Vector3.one * 0.7f;

                RectTransform rt = room.AddComponent<RectTransform>();
                rt.anchoredPosition = new Vector2(posX + width * j, posY + height * i);
                rt.sizeDelta = new Vector2(width, height);
                
                room.AddComponent<CanvasRenderer>();
                Image img = room.AddComponent<Image>();
                img.sprite = Resources.Load<Sprite>("Icons/frame");
                switch (GameManager.instance.GetRoomTypeByIndex(roomIdx))
                {
                    case RoomType.BattleRoom: img.sprite = Resources.Load<Sprite>("Icons/sword");break;
                    case RoomType.BossRoom: img.sprite = Resources.Load<Sprite>("Icons/scroll");break;
                    case RoomType.RewardRoom: img.sprite = Resources.Load<Sprite>("Icons/coins");break;
                    case RoomType.StartingRoom: img.sprite = Resources.Load<Sprite>("Icons/helmets");break;
                    case RoomType.EmptyRoom: img.sprite = Resources.Load<Sprite>("Icons/frame");break;
                    default:break;
                }
            }
        }
    }

    void Update()
    {
        if(Input.GetButtonDown("Cancel")){
            if (menuStack.Count == 0) {
                HideAndActive(mainMenu);
            }
            else
            {
                menuStack.Peek().SetActive(false);
                menuStack.Pop();
                if(menuStack.Count != 0) menuStack.Peek().SetActive(true);
            }
        }    
    }
}
