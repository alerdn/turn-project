using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockMoveData", menuName = "Moves/Status/Block")]
public class BlockMoveData : StatusMoveData
{
    public override Task Execute(Unit unitExecutor)
    {
        target = unitExecutor;
        return base.Execute(unitExecutor);
    }
}