using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject hudPrefab = null;
    private GameplayHUD _hud = null;

    private void Awake()
    {
        if (hudPrefab == null)
        {
            Debug.Log("UIManager has no HUD prefab assigned.");
            return;
        }

        GameLoader.CallOnComplete(Initialize);
    }

    public void Initialize()
    {
        var hudObject = Instantiate(hudPrefab);
        hudObject.transform.SetParent(transform);
        _hud = hudObject.GetComponent<GameplayHUD>();
        if (_hud == null)
        {
            Debug.LogError("GameplayHUD is NULL");
            return;
        }

        _hud.Initialize();
    }

    public void DisplayMessage(string message)
    {
        _hud.UpdateMessageText(message);
    }
}
