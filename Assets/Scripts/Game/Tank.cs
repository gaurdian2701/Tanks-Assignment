         using DSA;
using Game.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public abstract class Tank : MonoBehaviour
    {
        public static List<Tank>    AllTanks = new List<Tank>();

        private Vector2Int          m_vPosition;
        private int                 m_iRotation = 0;

        #region Properties c      

        public Vector2Int Position
        {
            get => m_vPosition;
            set
            {
                if (m_vPosition != value)
                {
                    m_vPosition = value;
                    transform.position = GetPositionForCoordinate(m_vPosition);
                }
            }
        }

        public int Rotation
        {
            get => m_iRotation;
            set
            {
                if (m_iRotation != value)
                {
                    m_iRotation = value;
                    if (m_iRotation < 0)
                    {
                        m_iRotation += 4;
                    }
                    m_iRotation %= 4;
                    transform.rotation = GetRotation(m_iRotation);
                }
            }
        }

        public Vector2Int Forward => new Vector2Int(Mathf.RoundToInt(transform.up.x), Mathf.RoundToInt(transform.up.y));

        public bool IsPerformingActions { get; private set; }

        #endregion

        protected virtual void OnEnable()
        {
            AllTanks.Add(this);
        }

        protected virtual void OnDisable()
        {
            AllTanks.Remove(this);
        }

        protected virtual void Start()
        {
            Position = Cave.Instance.GetRandomPosition();
            Rotation = Random.Range(0, 3);
        }

        public abstract void OnNewTurn(int iActionCount);

        public virtual void OnEndOfTurn()
        {
        }

        public abstract Queue<TankAction> GetActionQueue(int iActionCount);

        public IEnumerator PerformAction(TankAction action)
        {
            IsPerformingActions = true;
            if (action != null)
            {
                yield return StartCoroutine(action.PerformAction());
            }
            IsPerformingActions = false;
        }

        public static Vector3 GetPositionForCoordinate(Vector2Int vCoord)
        {
            return new Vector3(vCoord.x + 0.5f, vCoord.y + 0.5f, -0.05f);
        }

        public static Quaternion GetRotation(int iRotation)
        {
            return Quaternion.Euler(0.0f, 0.0f, iRotation * 90.0f);
        }
    }
}