using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EndlessGames.Util;


namespace EndlessGames
{
    public class Minigames
    {
        public class Minigame
        {
            public string name;
            public string displayName;
            public string color;
            public GameObject holder;
            public bool threeD;
            public bool fxOnly;
            public List<GameAction> actions = new List<GameAction>();

            public Minigame(string name, string displayName, string color, bool threeD, bool fxOnly, List<GameAction> actions)
            {
                this.name = name;
                this.displayName = displayName;
                this.color = color;
                this.actions = actions;
                this.threeD = threeD;
                this.fxOnly = fxOnly;
            }
        }

        public class GameAction
        {
            public string actionName;
            public EventCallback function;
            public float defaultLength;
            public bool resizable;
            public List<Param> parameters;

            public GameAction(string actionName, EventCallback function, float defaultLength = 1, bool resizable = false, List<Param> parameters = null)
            {
                this.actionName = actionName;
                this.function = function;
                this.defaultLength = defaultLength;
                this.resizable = resizable;
                this.parameters = parameters;
            }
        }

        [System.Serializable]
        public class Param
        {
            public string propertyName;
            public object parameter;
            public string propertyCaption;

            public Param(string propertyName, object parameter, string propertyCaption)
            {
                this.propertyName = propertyName;
                this.parameter = parameter;
                this.propertyCaption = propertyCaption;
            }
        }

        public delegate void EventCallback();

        public static void Init(EventCaller eventCaller)
        {
            eventCaller.minigames = new List<Minigame>()
            {
                new Minigame("gameManager", "Game Manager", "", false, true, new List<GameAction>()
                {
                    
                }),
            };
        }
    }
}