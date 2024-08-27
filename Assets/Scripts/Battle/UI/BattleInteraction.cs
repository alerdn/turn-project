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

public class BattleInteraction : MonoBehaviour
{
    public bool IsInteractable => _interactionResolvers.Count > 0;

    [SerializeField] private GameObject _frame;
    [SerializeField] private GameObject _actionButton;
    [SerializeField] private GameObject _actionEffect;
    [SerializeField] private Ease _antecipationEase;

    private MoveData _move;
    private List<InteractionResolver> _interactionResolvers;
    private int _currentInteractionIndex;
    private Tween _antecipationTween;

    public void Init(MoveData move, float resolveTime)
    {
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
    }

    public void UpdateState(float time)
    {
        InteractionResolver resolver = _interactionResolvers[_currentInteractionIndex];

        if (time >= resolver.StartAntecipationTime && !resolver.IsAntecipating)
        {
            resolver.IsAntecipating = true;
            Show();

            // Aumentamos o tempo da animação para auxiliar o jogador, dando a falsa impressão que ele acertou o timing perfeitamente
            _antecipationTween = _actionEffect.transform
                .DOScale(1f, resolver.InteractionData.InteractionWindowTime + .2f - resolver.InteractionData.AntecipationTime)
                .SetEase(_antecipationEase)
                .From(3f);
        }

        if (IsWithinInteractionWindow(time))
        {
            _actionButton.GetComponent<Image>().color = Color.blue;
        }
        else
        {
            _actionButton.GetComponent<Image>().color = Color.white;
        }

        if (time >= resolver.EndInteractionTime)
        {
            if (_currentInteractionIndex + 1 < _interactionResolvers.Count)
            {
                _currentInteractionIndex++;
            }
            else
            {
                Hide();
            }
        }
    }

    public void TryInteract(float time)
    {
        if (IsWithinInteractionWindow(time))
        {
            InteractionResolver resolver = _interactionResolvers[_currentInteractionIndex];
            resolver.InteractionData.HasInteracted = true;
            _actionButton.GetComponent<Image>().color = Color.green;
            Debug.Log("Interagiu no momento certo");
            Hide(250);
        }
        else
        {
            _actionButton.GetComponent<Image>().color = Color.red;
            Debug.Log("Interagiu no momento errado");
            Hide(250);
        }
    }

    public void Show()
    {
        _frame.SetActive(true);
    }

    public async void Hide(int delay = 0)
    {
        _antecipationTween.Kill();
        _actionEffect.transform.localScale = Vector3.one;

        await Task.Delay(delay);
        _frame.SetActive(false);
    }

    private bool IsWithinInteractionWindow(float time)
    {
        InteractionResolver resolver = _interactionResolvers[_currentInteractionIndex];
        return time >= resolver.StartInteractionTime && time <= resolver.EndInteractionTime && !resolver.InteractionData.HasInteracted;
    }
}