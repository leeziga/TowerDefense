﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneScript : MonoBehaviour
{
    public void PlayGameClicked()
    {
        SceneManager.LoadScene(1);
    }
}
