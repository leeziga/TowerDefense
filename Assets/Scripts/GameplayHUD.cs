using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayHUD : MonoBehaviour
{
    [SerializeField] private Text MessageText = null;

    public void Initialize()
    {
        MessageText.text = string.Empty;
    }

    public void SetGameplayHUDActive(bool shouldBeActive)
    {
        gameObject.SetActive(shouldBeActive);
    }

    public void UpdateMessageText(string message)
    {
        MessageText.text = message;
    }
}
