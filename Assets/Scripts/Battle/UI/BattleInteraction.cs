using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BattleInteraction : MonoBehaviour
{
    public bool IsActive { get; private set; }

    [SerializeField] private GameObject _actionButton;
    [SerializeField] private GameObject _actionEffect;

    private MoveData _move;
    private float _startInteractionTime;
    private float _endInteractionTime;
    private Tween _antecipationTween;

    public void Init(MoveData move, float resolveTime)
    {
        _move = move;

        _startInteractionTime = resolveTime + _move.InteractionWindowTime;
        _endInteractionTime = _startInteractionTime + _move.InteractionWindowDuration;

        gameObject.SetActive(true);
        IsActive = true;

        _antecipationTween = _actionEffect.transform.DOScale(3f, _move.InteractionWindowTime).SetEase(Ease.InSine).From();
    }

    public async void Hide(int delay = 0)
    {
        IsActive = false;
        _antecipationTween.Kill();
        _actionEffect.transform.localScale = Vector3.one;

        await Task.Delay(delay);
        gameObject.SetActive(false);
    }

    public void TryInteract(float time)
    {
        if (IsWithinInteractionWindow(time))
        {
            _move.HasInteracted = true;
            _actionButton.GetComponent<Image>().color = Color.green;
            Debug.Log("Interagiu no momento certo");
            Hide(500);
        }
        else
        {
            _actionButton.GetComponent<Image>().color = Color.red;
            Debug.Log("Interagiu no momento errado");
            Hide(500);
        }
    }

    public void UpdateState(float time)
    {
        if (IsWithinInteractionWindow(time))
        {
            _actionButton.GetComponent<Image>().color = Color.blue;
        }
        else
        {
            _actionButton.GetComponent<Image>().color = Color.white;
        }

        if (time > _endInteractionTime)
        {
            Hide();
        }
    }

    private bool IsWithinInteractionWindow(float time)
    {
        return time >= _startInteractionTime && time <= _endInteractionTime && !_move.HasInteracted;
    }
}