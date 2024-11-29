using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Actions
{
    public class TankAction_Wait : TankAction
    {
        #region Properties

        #endregion

        public TankAction_Wait(Tank tank) : base(tank)
        { 
        }

        public override IEnumerator PerformAction()
        {
            // You get this action for free...
            // The tank does nothing this turn :)
            yield break;
        }
    }
}