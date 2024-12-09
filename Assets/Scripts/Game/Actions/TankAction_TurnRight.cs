using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Actions
{
    public class TankAction_TurnRight : TankAction
    {
        private const float _tankRotationSpeed = 1.5f;
        
        #region Properties

        #endregion
        public TankAction_TurnRight(Tank tank) : base(tank)
        { 
        }

        public override IEnumerator PerformAction()
        {
            int startingAngle, finalAngle, angleOnRotating;
            float rotationPercentage = 0f;
            Quaternion startingRotation, finalRotation;
            
            startingAngle = m_tank.Rotation;
            angleOnRotating = m_tank.Rotation - 1;
            finalAngle = angleOnRotating;

            if (angleOnRotating < 0)
                angleOnRotating += 4;
            angleOnRotating %= 4;
                
            startingRotation = Tank.GetRotation(startingAngle);
            finalRotation = Tank.GetRotation(angleOnRotating);
            
            while (rotationPercentage < 1f)
            {
                rotationPercentage += Time.deltaTime * _tankRotationSpeed;
                m_tank.transform.rotation = Quaternion.Lerp(startingRotation, finalRotation, rotationPercentage);
                yield return null;
            }

            m_tank.Rotation = finalAngle;
        }
    }
}