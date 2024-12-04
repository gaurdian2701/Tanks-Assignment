using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Game.Actions
{
    public class TankAction_MoveForward : TankAction
    {
        #region Properties
        
        #endregion
        
        private Vector3 _velocity = Vector3.zero;
        private const float reachingThreshold = 0.01f;
        private const float reachTime = 0.3f;
        public TankAction_MoveForward(Tank tank) : base(tank)
        { 
        }

        public override IEnumerator PerformAction()
        {
            Vector2Int vTargetCoord = m_tank.Position + m_tank.Forward;
            Vector3 vTargetTile = Tank.GetPositionForCoordinate(vTargetCoord);
            float zOffset = m_tank.transform.position.z;
            
            while(Vector3.Distance(m_tank.transform.position, vTargetTile) > reachingThreshold)
            {
                m_tank.transform.position = Vector3.SmoothDamp(m_tank.transform.position, vTargetTile, ref _velocity, reachTime);
                Vector3 tankPos = m_tank.transform.position;
                tankPos.z = zOffset;
                m_tank.transform.position = tankPos;
                yield return null;
            }
            m_tank.Position = vTargetCoord;
        }
    }
}