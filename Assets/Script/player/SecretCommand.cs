using System.Collections.Generic;
using UnityEngine;

public class SecretCommand
{
    private string[] command = { "U", "U", "D", "D", "L", "R", "L", "R", "B", "A" };
    private List<string> inputkey = new List<string>();

    public void Clear()
    {
        inputkey.Clear();
    }

    public void Input(ControllerInput input)
    {
        if (input.GetKeyDown(ControllerButtonType.Up))
            CheckKey("U");
        else if (input.GetKeyDown(ControllerButtonType.Down))
            CheckKey("D");
        else if (input.GetKeyDown(ControllerButtonType.Left))
            CheckKey("L");
        else if (input.GetKeyDown(ControllerButtonType.Right))
            CheckKey("R");
        else if (input.GetKeyDown(ControllerButtonType.A))
            CheckKey("A");
        else if (input.GetKeyDown(ControllerButtonType.B))
            CheckKey("B");
    }

    private void CheckKey(string key)
    {
        inputkey.Add(key);
        DumpInputKey();

        if (CheckCommand())
        {
            if (OnSuccess != null) OnSuccess();
            inputkey.Clear();
        }
    }

    private bool CheckCommand()
    {
        if(command.Length != inputkey.Count)
            return false;

        for (int i=0; i < command.Length; i++)
        {
            if (command[i] != inputkey[i]) return false;
        }
        return true;
    }

    private void DumpInputKey()
    {
        string str = "";
        for (int i = 0; i < inputkey.Count; i++)
        {
            str = str + inputkey[i] + ",";
        }
        Debug.Log(str);
    }

    public delegate void SuccessDelegate();
    public SuccessDelegate OnSuccess;
}
