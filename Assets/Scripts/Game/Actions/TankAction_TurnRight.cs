using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Actions
{
    public class TankAction_TurnRight : TankAction
    {
        // TODO: Turn Left & Turn Right are very similar? 
        //       how to share logic between them in a good way to avoid
        //       duplicating code?

        #region Properties

        #endregion

        public TankAction_TurnRight(Tank tank) : base(tank)
        { 
        }

        public override IEnumerator PerformAction()
        {
            // TODO: implement a smooth turn of the tank
            yield break;
        }
    }
}