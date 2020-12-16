using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : AsyncLoader
{
    public int sceneIndexToLoad = 1;
    public LoadingScreen loadingScreen = null;
    public List<Component> GameModules;
    private static int _sceneIndex = 1;
    private static GameLoader _instance; //This only singleton that we should have

    protected override void Awake()
    {
        Debug.Log("GameLoader Starting");

        //Safety Check
        if (_instance != null && _instance != this)
        {
            Debug.Log("A duplicated instace of the GameLoader was found, and will be ignored. Only one instance is permitted.");
            Destroy(gameObject);
            return;
        }

        //Set reference to this instance
        _instance = this;

        //Make this object persistent
        DontDestroyOnLoad(gameObject);

        //Scene Index Check
        if (sceneIndexToLoad < 0 || sceneIndexToLoad >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log($"Invalid Scene Index {sceneIndexToLoad} ... using default value of {_sceneIndex}");
        }
        else
        {
            _sceneIndex = sceneIndexToLoad;
        }

        // Setup system GameObject
        GameObject systemGO = new GameObject("[Services]");
        systemGO.tag = "Services";
        Transform systemParent = systemGO.transform;
        DontDestroyOnLoad(systemGO);

        loadingScreen.UpdateLoadingStep("Loading Game Systems");

        //Enqueue RoutineInfos to my queue;
        Enqueue(InitializeCoreSystems(systemParent), 50, UpdateCoreSystemsProgress);
        Enqueue(InitializeModularSystems(systemParent), 50, UpdateModularSystemsProgress);

        //Setting the completion callback
        CallOnComplete(OnComplete);

        //StartCoroutine("LoadLoadingBar");
    }

    private float _coreLoadTotalSteps = 20.0f;
    private float _coreLoadCurrentStep = 0.0f;

    private float UpdateCoreSystemsProgress()
    {
        return _coreLoadCurrentStep / _coreLoadTotalSteps;
    }

    private float _modularLoadTotalSteps = 2000.0f;
    private float _modularLoadCurrentStep = 0.0f;

    private float UpdateModularSystemsProgress()
    {
        return _modularLoadCurrentStep / _modularLoadTotalSteps;
    }

    protected override void ProgressUpdated(float percentComplete)
    {
        base.ProgressUpdated(percentComplete);
        loadingScreen.UpdateLoadingBar(percentComplete);
        loadingScreen.UpdateLoadingStep("Progress: " + Mathf.Ceil(percentComplete * 100.0f) + "%");
    }

    private IEnumerator InitializeCoreSystems(Transform systemsParent)
    {
        Debug.Log("Loading core systems...");
        yield return null;
    }

    private IEnumerator InitializeModularSystems(Transform systemsParent)
    {
        Debug.Log("Loading modular systems...");

        foreach (Component c in GameModules)
        {
            if (c is IGameModule)
            {
                IGameModule module = (IGameModule)c;
                yield return module.LoadModule();
            }
        }
        yield return null;
    }

    private void OnComplete()
    {
        Debug.Log("GameLoader Completed");
        StartCoroutine(LoadInitialScene(_sceneIndex));
    }

    private IEnumerator LoadInitialScene(int index)
    {
        Debug.Log("GameLoader Starting Scene load");
        var loadOp = SceneManager.LoadSceneAsync(index);

        loadingScreen.UpdateLoadingStep("Loading Scene: " + index.ToString());

        while (!loadOp.isDone)
        {
            loadingScreen.UpdateLoadingBar(loadOp.progress);
            yield return loadOp;
        }
    }
}
