/*using System;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;

using EndlessGames.Util;

namespace EndlessGames
{
    [Serializable]
    public class Beatmap
    {
        public float bpm;
        public List<Entity> entities = new List<Entity>();
        public List<TempoChange> tempoChanges = new List<TempoChange>();

        [Serializable]
        public class Entity : ICloneable
        {
            public float beat;
            public int track;
            [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)] public float length;
            [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)] public float valA;
            [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)] public float valB;
            [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)] public float valC;
            [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)] public int type;
            [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)] public Color colorA;
            [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)] public Color colorB;
            [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)] public Color colorC;
            public string datamodel;

            public object Clone()
            {
                return this.MemberwiseClone();
            }

            public object this[string propertyName]
            {
                get
                {
                    return typeof(Entity).GetField(propertyName).GetValue(this);
                }
                set
                {
                    try
                    {
                        typeof(Entity).GetField(propertyName).SetValue(this, value);
                    }
                    catch (Exception ex)
                    {
                        UnityEngine.Debug.LogError($"You probably misspelled a paramater, or defined the object type wrong. Exception log: {ex}");
                    }
                }
            }
        }

        [Serializable]
        public class TempoChange : ICloneable
        {
            public float beat;
            public float length;
            public float tempo;

            public object Clone()
            {
                return this.MemberwiseClone();
            }
        }
    }
}
*/