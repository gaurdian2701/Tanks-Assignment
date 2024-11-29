using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Actions
{
    public abstract class TankAction 
    {
        private Tank        m_tank;

        #region Properties

        public Tank Tank => m_tank;

        #endregion

        public TankAction(Tank tank)
        { 
            m_tank = tank;
        }

        public abstract IEnumerator PerformAction();
    }
}