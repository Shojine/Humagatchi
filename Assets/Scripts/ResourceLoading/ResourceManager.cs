using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    private ActionCollection actionOptions;
    public static ResourceManager Instance { get; private set; }
    private System.Random random = new();

    private List<int> actionsInUse = new List<int>() { };

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Found more than one Resource Manager in the scene.");
        }
        Instance = this;
        
        Load();
    }

    public void Load()
    {
        TextAsset actionsJsonFile = Resources.Load<TextAsset>("actions");

        try
        {
            actionOptions = JsonUtility.FromJson<ActionCollection>(actionsJsonFile.text);
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to load game resources" + "\n" + e.Message);
        }
    }

    public ActionOption GetAction()
    {
        // this is a horrible way to do this
        int index = random.Next(0, actionOptions.actions.Length - 1);
        while (actionsInUse.Contains(index))
        {
            index = random.Next(0, actionOptions.actions.Length - 1);
        }

        Debug.Log(index);
        actionsInUse.Add(index);
        return actionOptions.actions[index];
    }

    public void FreeAction(ActionOption action)
    {
        Debug.Log("freed " + actionOptions.FindIndex(action));
        actionsInUse.Remove(actionOptions.FindIndex(action));
    }
}
