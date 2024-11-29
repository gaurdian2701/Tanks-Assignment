using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Actions
{
    public class TankAction_MoveForward : TankAction
    {
        #region Properties

        #endregion

        public TankAction_MoveForward(Tank tank) : base(tank)
        { 
        }

        public override IEnumerator PerformAction()
        {
            // TODO: move the tank smoothly to the next tile
            // (don't forget to update it's Position property)

            // TIP: you cannot drive in to a wall, check out Cave.Instance.HasWall()

            // TIP: you can get the forward vector from the Tank.Forward

            // TIP:
            // Vector2Int vTargetCoord = Tank.Position + Tank.Forward;
            // Vector3 vTargetTile = Tank.GetPositionForCoordinate(vTargetCoord);

            yield break;
        }
    }
}