using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EndlessGames
{
    public class MainMenuButton : MonoBehaviour
    {
        public delegate void EventCallback();
        private bool preSelectionActive = false;
        private bool cursorOver;

        public int index = 0;
        private void Start()
        {
            AddTrigger((data) => { MainMenu.instance.SelectionDown(this.gameObject); preSelectionActive = true; }, EventTriggerType.PointerDown);
            AddTrigger((data) => 
            {
                if (cursorOver)
                {
                    MainMenu.instance.SelectionUp(this.gameObject);
                    preSelectionActive = false; 
                }
            }, EventTriggerType.PointerUp);
            AddTrigger((data) => 
            {
                if (preSelectionActive)
                {
                    MainMenu.instance.SelectionEnter(this.gameObject); 
                }
                cursorOver = true;
                MainMenu.instance.SetSelectionPos(this.gameObject.transform.position);
            }, EventTriggerType.PointerEnter);
            AddTrigger((data) => 
            { 
                cursorOver = false;
                MainMenu.instance.SelectionExit(this.gameObject); 
            }, EventTriggerType.PointerExit);
        }

        private void Update()
        {
#if UNITY_ANDROID || UNITY_IPHONE
            if (Input.touchCount == 0)
                preSelectionActive = false;
#else
            if (Input.GetMouseButtonUp(0))
            {
                if (!cursorOver)
                    preSelectionActive = false;
            }
#endif
        }

        private void AddTrigger(UnityEngine.Events.UnityAction<BaseEventData> del, EventTriggerType type)
        {
            EventTrigger eventTrigger = GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = type;
            entry.callback.AddListener(del);
            eventTrigger.triggers.Add(entry);
        }
    }
}