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
    [SerializeField] private Transform _energySlotsParent;

    [Header("Colors")]
    [SerializeField] private Color _fullLifeColor;
    [SerializeField] private Color _midLifeColor;
    [SerializeField] private Color _lowLifeColor;

    [SerializeField] private List<Image> _energySlots;

    public void Init(Unit unit)
    {
        _name.text = unit.Name;
        _lifebar.fillAmount = unit.GetHealthPercentage();
        _lifebar.color = GetCurrentLifebarColor(unit.GetHealthPercentage());

        if (_energySlotsParent)
        {
            _energySlots = _energySlotsParent.GetComponentsInChildren<Image>().ToList();
            unit.OnEnergyUpdated += UpdateEnergySlots;
        }

        unit.OnHealthUpdated += UpdateUI;
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
