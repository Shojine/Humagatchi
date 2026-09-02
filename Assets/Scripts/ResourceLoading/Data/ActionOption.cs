using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActionOption
{
    public string name = "";
    public string type = "";
    public int cost = 0;
    public HumanStateChange changes;
}

[System.Serializable]
public class ActionCollection
{
    public ActionOption[] actions;

    public int FindIndex(ActionOption action)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            if (actions[i].name == action.name) return i;
        }
        return -1;
    }
}