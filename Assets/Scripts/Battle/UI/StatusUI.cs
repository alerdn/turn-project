using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StatusUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private Image _lifebar;

    [Header("Colors")]
    [SerializeField] private Color _fullLifeColor;
    [SerializeField] private Color _midLifeColor;
    [SerializeField] private Color _lowLifeColor;

    public void Init(Unit unit)
    {
        _name.text = unit.Name;
        _lifebar.fillAmount = unit.GetHealthPercentage();

        unit.OnDamaged += UpdateUI;
    }

    private async void UpdateUI(float healthPercentage)
    {
        await _lifebar.DOColor(new Color(0f, 0f, 0f, 0f), .1f).SetLoops(6, LoopType.Yoyo).AsyncWaitForCompletion();
        await _lifebar.DOFillAmount(healthPercentage, 1f).SetEase(Ease.OutSine).AsyncWaitForCompletion();

        if (healthPercentage <= .25f)
        {
            _lifebar.color = _lowLifeColor;
        }
        else if (healthPercentage <= .5f)
        {
            _lifebar.color = _midLifeColor;
        }
        else
        {
            _lifebar.color = _fullLifeColor;
        }
    }
}
