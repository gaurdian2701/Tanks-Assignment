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

        private Queue<TankAction> _playerActionQueue = new Queue<TankAction>();
        public override void OnNewTurn(int iActionCount)
        {
            _playerActionQueue.Clear();
        }

        public override Queue<TankAction> GetActionQueue(int iActionCount)
        {
            return _playerActionQueue;
        }
        
        public void AddPlayerActionToQueue(TankAction action) => _playerActionQueue.Enqueue(action);
    }
}