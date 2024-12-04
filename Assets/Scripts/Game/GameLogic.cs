using Game.Actions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class GameLogic : MonoBehaviour
    {
        [SerializeField]
        public List<Sprite>         m_actionIcons = new List<Sprite>();

        private int                 m_iNumActions = 1;
        private GameObject          m_inputMenu;
        private CanvasGroup         m_actionGroup;
        private GameObject          m_slotTemplate;
        private Queue<Image>        m_slotIconQueue;

        #region Properties

        #endregion

        private void Start()
        {
            StartCoroutine(Logic());

            // create action inputs
            m_inputMenu = transform.Find("PlayerInput/Canvas/InputMenu").gameObject;
            m_actionGroup = m_inputMenu.transform.Find("ActionMenu").GetComponent<CanvasGroup>();
            Transform actionTemplate = m_actionGroup.transform.Find("ActionList/ActionTemplate");
            foreach (System.Type type in typeof(Tank).Assembly.GetTypes())
            {
                if (!type.IsAbstract && typeof(TankAction).IsAssignableFrom(type))
                {
                    GameObject go = Instantiate(actionTemplate.gameObject, actionTemplate.parent);
                    go.name = type.Name;
                    go.SetActive(true);
                    Image icon = go.transform.Find("Icon").GetComponent<Image>();
                    icon.sprite = m_actionIcons.Find(s => s.name == type.Name);
                    go.GetComponent<Button>().onClick.AddListener(() => { QueuePlayerAction(type); });
                }
            }

            // grab slot template
            m_slotTemplate = m_inputMenu.transform.Find("ActionQueue/ActionContainer/SlotTemplate").gameObject;
            m_inputMenu.SetActive(false);
            m_actionGroup.interactable = false;
        }

        IEnumerator Logic()
        {
            while (true)            // TODO: Define game over conditions!
            {
                // increate num actions?
                m_iNumActions = Mathf.Min(m_iNumActions + 1, 6);

                // initialize tanks for new turn
                foreach (Tank tank in Tank.AllTanks)
                {
                    tank.OnNewTurn(m_iNumActions);
                }

                // get player input
                yield return new WaitForSeconds(0.5f);
                yield return PlayerInput();

                // perform action queues
                yield return new WaitForSeconds(0.5f);
                yield return PerformTankQueues();

                // notify tanks of 'end of turn'
                foreach (Tank tank in Tank.AllTanks)
                {
                    tank.OnEndOfTurn();
                }
            }
        }

        IEnumerator PlayerInput()
        {
            // got player tank still alive?
            PlayerTank playerTank = GetComponentInChildren<PlayerTank>();
            if (playerTank != null)
            {
                // show player input menu
                m_inputMenu.SetActive(true);

                // create new slots
                List<GameObject>  createdSlots = new List<GameObject>();
                m_slotIconQueue = new Queue<Image>();
                while (createdSlots.Count < m_iNumActions)
                {
                    // create a new action slot
                    GameObject go = Instantiate(m_slotTemplate, m_slotTemplate.transform.parent);
                    go.name = "Slot #" + (createdSlots.Count + 1);
                    go.SetActive(true);
                    createdSlots.Add(go);
                    m_slotIconQueue.Enqueue(go.transform.Find("Icon").GetComponent<Image>());
                }

                // wait for inputs
                m_actionGroup.interactable = true;
                while (m_slotIconQueue.Count > 0)
                {
                    yield return null;
                }

                // wait for a little while before executing plan
                m_actionGroup.interactable = false;
                yield return new WaitForSeconds(0.5f);

                // clean up
                foreach (GameObject go in createdSlots)
                {
                    Destroy(go);
                }
                createdSlots.Clear();
                m_slotIconQueue = null;

                // hide player input menu
                m_inputMenu.SetActive(false);
            }
        }

        IEnumerator PerformTankQueues()
        {
            // get tanks and their queues
            List<Tank> tanks = new List<Tank>(Tank.AllTanks);
            List<Queue<TankAction>> tankActions = tanks.ConvertAll(t => t.GetActionQueue(m_iNumActions));
                
            for (int i = 0; i < m_iNumActions; ++i)
            {
                for (int j = 0; j < tanks.Count; ++j)
                {
                    // TODO: check if the tank is still alive

                    // let tank perform action?
                    TankAction action;
                    if (tankActions[j] != null && 
                        tankActions[j].TryDequeue(out action) && 
                        action != null)
                    {
                        tanks[j].StartCoroutine(tanks[j].PerformAction(action));
                    }
                }

                // wait for all tanks to finish their actions
                while (Tank.AllTanks.FindIndex(t => t.IsPerformingActions) >= 0)
                {
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }

        protected void QueuePlayerAction(System.Type type)
        {
            if (m_slotIconQueue != null && m_slotIconQueue.Count > 0)
            {
                Debug.Log("Queue Player Action: " + type.Name);
                Image nextIcon = m_slotIconQueue.Dequeue();
                nextIcon.sprite = m_actionIcons.Find(s => s.name == type.Name);

                // create a new tank action of type
                PlayerTank playerTank = GetComponentInChildren<PlayerTank>();
                TankAction newTankAction = System.Activator.CreateInstance(type, new object[] { playerTank }) as TankAction;

                if (newTankAction != null)
                {
                    playerTank.AddPlayerActionToQueue(newTankAction);
                    // TODO: somehow enqueue the new tank action in the player tank's queue here
                }
            }
        }
    }
}