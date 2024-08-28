using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

public class StatusUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private Image _lifebar;

    [Header("Colors")]
    [SerializeField] private Color _fullLifeColor;
    [SerializeField] private Color _midLifeColor;
    [SerializeField] private Color _lowLifeColor;
    [SerializeField] private List<Image> _energySlots;

    private Unit _unit;

    public void Init(Unit unit)
    {
        _unit = unit;

        _name.text = _unit.Name;
        _lifebar.fillAmount = _unit.GetHealthPercentage();
        _lifebar.color = GetCurrentLifebarColor(_unit.GetHealthPercentage());

        if (_energySlots.Count > 0)
        {
            UpdateEnergySlots(_unit.EnergyAmount);
            _unit.OnEnergyUpdated += UpdateEnergySlots;
        }

        _unit.OnHealthUpdated += UpdateUI;
    }

    private void OnDestroy()
    {
        if (_energySlots.Count > 0)
        {
            _unit.OnEnergyUpdated -= UpdateEnergySlots;
        }
        _unit.OnHealthUpdated -= UpdateUI;
    }

    private async void UpdateUI(float healthPercentage)
    {
        if (healthPercentage < _lifebar.fillAmount)
        {
            await _lifebar.DOColor(new Color(0f, 0f, 0f, 0f), .1f).SetLoops(6, LoopType.Yoyo).AsyncWaitForCompletion();
        }
        await _lifebar.DOFillAmount(healthPercentage, .8f).SetEase(Ease.OutSine).AsyncWaitForCompletion();

        _lifebar.color = GetCurrentLifebarColor(healthPercentage);
    }

    private Color GetCurrentLifebarColor(float healthPercentage)
    {
        if (healthPercentage <= .25f)
        {
            return _lowLifeColor;
        }
        else if (healthPercentage <= .5f)
        {
            return _midLifeColor;
        }
        else
        {
            return _fullLifeColor;
        }
    }

    private void UpdateEnergySlots(int energyAmount)
    {
        for (int i = 0; i < _energySlots.Count; i++)
        {
            _energySlots[i].gameObject.SetActive(i < energyAmount);
        }
    }
}
