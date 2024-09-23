using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "HealItem", menuName = "Items/HealItem")]
public class HealItemData : ItemData
{
    public int HealAmount;

    public override async Task<string> Execute(Unit unitExecutor)
    {
        unitExecutor.Heal(HealAmount);
        await Task.Delay(Mathf.RoundToInt(ActionDuration * 1000f));

        return $"{unitExecutor.Name} foi curado em {HealAmount} pontos de vida.";
    }
}