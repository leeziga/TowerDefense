using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static readonly Dictionary<int, int> _killByLevel = new Dictionary<int, int>()
    {
        { 1, 0 },
        { 2, 3 },
        { 3, 6 }
    };

    private int _numKilled = 0;

    private int _currentScore = 0;

    public int CurrentScore { get { return _currentScore; } }

    private int _currentLevel = 0;
    public int CurrentLevel { get { return _currentLevel; } }

    private UIManager _uiManager = null;

    private bool gameLost = false;

    public GameManager Initialize(int startLevel)
    {
        GameLoader.CallOnComplete(OnGameLoaderComplete);
        SetLevel(startLevel);
        return this;
    }

    private void OnGameLoaderComplete()
    {
        _uiManager = ServiceLocator.Get<UIManager>();
    }

    private void SetLevel(int level)
    {
        _currentLevel = level;
    }

    private void LoadNextLevel()
    {
        int nextLevel = ++_currentLevel;
        if (nextLevel > _killByLevel.Count)
        {
            _uiManager.DisplayMessage("YOU WIN!");
        }
        else
        {
            SceneManager.LoadScene(nextLevel);
            SetLevel(nextLevel);
            _numKilled = 0;
            _uiManager.DisplayMessage("");
        }
    }

    public void IncrementEnemyKilled()
    {
        _numKilled++;
        StartCoroutine(CheckWinCondition());
    }

    public void UpdateScore(int score)
    {
        _currentScore += score;
        _numKilled++;
        StartCoroutine( CheckWinCondition());
    }

    private IEnumerator CheckWinCondition()
    {
        int numberRequiredToWin = _killByLevel[_currentLevel];
        if (_numKilled >= numberRequiredToWin)
        {
            _uiManager.DisplayMessage("Level Completed");
            yield return new WaitForSeconds(2.0f);
            LoadNextLevel();
        }
        yield return null;
    }

    public void LoseGame()
    {
        gameLost = true;
        _uiManager.DisplayMessage("You lose!");
    }


}
