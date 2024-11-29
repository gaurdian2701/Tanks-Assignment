using DSA;
using Game.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class PlayerTank : Tank
    {
        #region Properties

        #endregion

        public override void OnNewTurn(int iActionCount)
        {
            // TODO: do your turn initialization here
        }

        public override Queue<TankAction> GetActionQueue(int iActionCount)
        {
            // TODO: 1. store a queue in the player tank
            //       2. create an interface that let's the player input UI enqueue actions
            //       3. return that queue here
            
            return null;
        }
    }
}