using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class InteractionResolver
{
    public InteractionData InteractionData;
    public float StartAntecipationTime;
    public bool IsAntecipating;
    public float StartInteractionTime;
    public float EndInteractionTime;
}

[Serializable]
public class ActionUI
{
    public GameObject Frame;
    public GameObject Button;
    public GameObject Effect;
    public Tween AntecipationTween;
    public int ButtonIndex;
}

public class BattleInteraction : MonoBehaviour
{
    public bool IsInteractable
    {
        get
        {
            EInteractableBy interactableBy = _unit.Type switch
            {
                UnitType.Player => EInteractableBy.Player,
                UnitType.Enemy => EInteractableBy.Enemy,
                _ => EInteractableBy.None,
            };

            return _interactionResolvers.Count > 0 && _move.InteractableBy.HasFlag(interactableBy);
        }
    }

    [SerializeField] private List<ActionUI> _actions;
    [SerializeField] private Ease _antecipationEase;

    private Unit _unit;
    private MoveData _move;
    private List<InteractionResolver> _interactionResolvers;
    private int _currentInteractionIndex;

    public void Init(Unit unit, MoveData move, float resolveTime)
    {
        _unit = unit;
        _move = move;
        _interactionResolvers = new();

        foreach (var interaction in _move.InteractionsData)
        {
            float startAntecipationTime = resolveTime + interaction.AntecipationTime;
            float startInteractionTime = resolveTime + interaction.InteractionWindowTime;
            float endInteractionTime = startInteractionTime + interaction.InteractionWindowDuration;

            _interactionResolvers.Add(new()
            {
                InteractionData = interaction,
                StartAntecipationTime = startAntecipationTime,
                IsAntecipating = false,
                StartInteractionTime = startInteractionTime,
                EndInteractionTime = endInteractionTime,
            });
        }

        _currentInteractionIndex = 0;
        _actions.Shuffle();
    }

    public void UpdateState(float time)
    {
        for (int i = 0; i < _interactionResolvers.Count; i++)
        {
            InteractionResolver resolver = _interactionResolvers[i];
            ActionUI action = _actions[i];

            if (time >= resolver.StartAntecipationTime && !resolver.IsAntecipating)
            {
                resolver.IsAntecipating = true;
                Show(action);

                // Aumentamos o tempo da animação para auxiliar o jogador, dando a falsa impressão que ele acertou o timing perfeitamente
                action.AntecipationTween = action.Effect.transform
                    .DOScale(1f, resolver.InteractionData.InteractionWindowTime + .3f - resolver.InteractionData.AntecipationTime)
                    .SetEase(_antecipationEase)
                    .From(3f);
            }

            if (time >= resolver.EndInteractionTime && !resolver.InteractionData.HasInteracted)
            {
                if (!VerifyNextInteraction())
                {
                    Hide(action);
                }
            }
        }
    }

    public void TryInteract(float time, int buttonIndex)
    {
        InteractionResolver resolver = _interactionResolvers[_currentInteractionIndex];
        ActionUI action = _actions[_currentInteractionIndex];

        if (IsWithinInteractionWindow(time, resolver, action, buttonIndex))
        {
            resolver.InteractionData.HasInteracted = true;
            action.Button.GetComponent<Image>().color = Color.green;

            VerifyNextInteraction();
            Hide(action, 100);
        }
        else
        {
            action.Button.GetComponent<Image>().color = Color.red;

            VerifyNextInteraction();
            Hide(action, 100);
        }
    }

    public void Show(ActionUI action)
    {
        action.Button.GetComponent<Image>().color = Color.white;
        action.Frame.SetActive(true);
    }

    public void Hide(ActionUI action, int delay = 0)
    {
        HideAction(action, delay);
    }

    public void Hide()
    {
        foreach (var action in _actions.FindAll(action => action.Frame.activeInHierarchy))
        {
            HideAction(action);
        }
    }

    private bool VerifyNextInteraction()
    {
        if (_currentInteractionIndex + 1 < _interactionResolvers.Count)
        {
            _currentInteractionIndex++;
            return true;
        }

        return false;
    }

    private async void HideAction(ActionUI action, int delay = 0)
    {
        action.AntecipationTween.Kill();
        action.Effect.transform.localScale = Vector3.one;

        await Task.Delay(delay);
        action.Frame.SetActive(false);
    }

    private bool IsWithinInteractionWindow(float time, InteractionResolver resolver, ActionUI action, int buttonIndex)
    {
        return time >= resolver.StartInteractionTime
            && time <= resolver.EndInteractionTime
            && !resolver.InteractionData.HasInteracted
            && action.ButtonIndex == buttonIndex;
    }
}