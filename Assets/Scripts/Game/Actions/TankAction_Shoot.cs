using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Actions
{
    public class TankAction_Shoot : TankAction
    {
        #region Properties

        #endregion

        public TankAction_Shoot(Tank tank) : base(tank)
        { 
        }

        public override IEnumerator PerformAction()
        {
            // TODO:
            // --------------------------------------------------------------------------------
            // 1. load the bullet prefab using Resource.Load<GameObject>("Prefabs/Bullet")
            // 2. Instantiate a new GameObject from the bullet prefab
            // 3. Place the new gameobject at the muzzle of the tank
            // 4. Move the bullet forward with a constant (but fast speed) until it hits a wall or another tank
            // 5. Cleanup, Destroy the bullet you've created
            //      A. If you hit a tank, destroy the tank gameObject as well 

            yield break;
        }
    }
}