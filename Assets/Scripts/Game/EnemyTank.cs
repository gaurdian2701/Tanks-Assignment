using DSA;
using Game.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class EnemyTank : Tank
    {
        #region Properties

        #endregion

        public override void OnNewTurn(int iActionCount)
        {
            // TODO: do your turn initialization here
        }

        public override Queue<TankAction> GetActionQueue(int iActionCount)
        {
            // TODO: Create an enemy AI logic that creates a queue of actions and returns here!
            return null;
        }
    }
}