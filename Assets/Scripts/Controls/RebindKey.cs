using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class RebindKey : MonoBehaviour
{
    [SerializeField] InputActionReference Action;
    [SerializeField] TextMeshProUGUI ActionName;
    [SerializeField] TextMeshProUGUI KeyName;
    [SerializeField] GameObject Button;
    [SerializeField] GameObject RebindText;
    bool isInGame;
    PlayerInput playerInput;
    InputActionRebindingExtensions.RebindingOperation RebindOperation;

    private void Awake()
    {
        ActionName.text = Action.action.name;
        KeyName.text = Action.action.GetBindingDisplayString();
    }

    public void Rebind()
    {
        isInGame = GameObject.FindGameObjectWithTag("Player");
        if (isInGame)
        {
            playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
            playerInput.SwitchCurrentActionMap("Menu");
        }
        Button.SetActive(false);
        RebindText.SetActive(true);
        RebindOperation = Action.action.PerformInteractiveRebinding()
            .WithControlsExcluding("<keyboard>/escape").WithControlsExcluding("<keyboard>/anykey")
            .WithControlsExcluding("<keyboard>/a").WithControlsExcluding("<keyboard>/d")
            .OnMatchWaitForAnother(0.1f).OnComplete(arg => Complited()).Start();
    }
    private void Complited()
    {
        if (CheckDuplicationOfBinding())
        {
            Action.action.RemoveBindingOverride(Action.action.GetBindingIndex());
            RebindOperation.Dispose();
            RebindOperation = null;
            Rebind();
            return;
        }
        RebindOperation.Dispose();
        Button.SetActive(true);
        RebindText.SetActive(false);
        KeyName.text = Action.action.GetBindingDisplayString();
        if (isInGame)
        {
            playerInput.SwitchCurrentActionMap("Player");
            SaveLoad.SaveControls(playerInput);
        } 
        else 
        {
            SaveLoad.SaveControls(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PlayerInput>());
        }   
    }
    private bool CheckDuplicationOfBinding()
    {
        InputBinding newB = Action.action.bindings[Action.action.GetBindingIndex()];
        foreach (var bindings in Action.action.actionMap.bindings)
        {
            if (bindings.action == newB.action)
                continue;
            if (bindings.effectivePath == newB.effectivePath)
                return true;
        }
        return false;
    }
}
