using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EndlessGames.Util
{
    public class MultiSound : MonoBehaviour
    {
        private float startBeat;
        private int index;
        private bool game;
        public List<Sound> sounds = new List<Sound>();

        public class Sound
        {
            public string name { get; set; }
            public float beat { get; set; }

            public Sound(string name, float beat)
            {
                this.name = name;
                this.beat = beat;
            }
        }


        public static void Play(Sound[] snds, bool game = true)
        {
            List<Sound> sounds = snds.ToList();
            GameObject gameObj = new GameObject();
            MultiSound ms = gameObj.AddComponent<MultiSound>();
            ms.sounds = sounds;
            ms.startBeat = sounds[0].beat;
            ms.game = game;
            gameObj.name = "MultiSound";
        }

        private void LateUpdate()
        {
            float songPositionInBeats = Conductor.instance.songPositionInBeats;

            for (int i = 0; i < sounds.Count; i++)
            {
                if (songPositionInBeats >= sounds[i].beat && index == i)
                {
                    if (game)
                        Jukebox.PlayOneShotGame(sounds[i].name);
                    else
                        Jukebox.PlayOneShot(sounds[i].name);

                    index++;
                }
            }

            if (songPositionInBeats >= (sounds[sounds.Count - 1].beat))
            {
                Destroy(this.gameObject);
            }
        }
    }
}