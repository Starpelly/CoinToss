using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EndlessGames.Util;

namespace EndlessGames.Games.CoinToss
{
    public class Cointoss : PlayerActionObject
    {
        [Header("Components")] 
        public GameObject player;
        public Sprite[] seconds;
        public Sprite[] milliseconds;
        public Sprite[] scores;
        public SpriteRenderer second;
        public SpriteRenderer millisecond0, millisecond1;
        public SpriteRenderer scoreSprite, scoreSprite2;
        
        [Header("Properties")]
        private bool isTossing;
        private bool isTossingAnim;
        private bool isCounting;
        private int tossTimes = 0;
        public int score;
        
        private void Start()
        {
            Flicking.OnFlick += Toss;
        }
        private void Update()
        {
            if (PlayerInput.Tapped())
            {
                if (!isTossing) player.GetComponent<Animator>().Play("Prepare_Coin", 0,0 );
                else player.GetComponent<Animator>().Play("Prepare", 0,0 );
                isTossingAnim = false;
            }
            else if (PlayerInput.TappedRelease() && !isTossingAnim)
            {
                if (!isTossing) player.GetComponent<Animator>().Play("Idle_Coin", 0,0 );
                else player.GetComponent<Animator>().Play("Idle", 0,0 );
            }

            if (isCounting)
            {
                float normalizedBeat = Conductor.instance.GetPositionFromBeat(0, 6);
                StateCheck(normalizedBeat);
                if (state.perfect)
                {
                    if (PlayerInput.Tapped())
                    {
                        Catch();
                    }
                }
            }

            if (isTossing && Conductor.instance.songPosition < 10f)
            {
                second.sprite = seconds[(int)(Conductor.instance.songPosition)];
                string songPosStr = Conductor.instance.songPosition.ToString();
                if (songPosStr.Length >= 4)
                {
                    millisecond0.sprite = milliseconds[int.Parse(songPosStr[2].ToString())];
                    millisecond1.sprite = milliseconds[int.Parse(songPosStr[3].ToString())];
                }
            }
        }

        private void Catch()
        {
            Jukebox.PlayOneShotGame("coinToss/catch");
            player.GetComponent<Animator>().Play("Catch", 0, 0);
            
            isTossing = false;
            isTossingAnim = true;
            isCounting = false;

            score++;
            
            if (score > 9)
            {
                string scoreStr = score.ToString();
                scoreSprite.sprite = scores[int.Parse(scoreStr[0].ToString())];
                scoreSprite2.sprite = scores[int.Parse(scoreStr[1].ToString())];

                scoreSprite.transform.localPosition = new Vector3(-0.125f, scoreSprite.transform.localPosition.y);
                scoreSprite2.transform.localPosition = new Vector3(0.25f, scoreSprite2.transform.localPosition.y);
                
                scoreSprite2.gameObject.SetActive(true);
                scoreSprite.transform.parent.transform.localPosition = new Vector3(-0.047f, -0.03125f);
            }
            else
            {
                scoreSprite.sprite = scores[score];
            }
            
            ResetState();
        }

        private void Toss(FlickData obj)
        {
            if (!isTossing) player.GetComponent<Animator>().Play("Flick_Coin", 0,0 );
            else player.GetComponent<Animator>().Play("Flick", 0,0 );
            isTossingAnim = true;
            if (!isTossing)
            {
                isTossing = true;
                Jukebox.PlayOneShotGame("coinToss/flick");

                isCounting = true;

                if (tossTimes > 8)
                {
                    tossTimes = 0;
                    if (Conductor.instance.musicSource.pitch >= 0.19)
                    {
                        Conductor.instance.musicSource.pitch -= 0.09f;   
                    }
                }

                if (tossTimes > 0)
                {
                    Conductor.instance.musicSource.clip = Jukebox.LoadSong($"Cointoss_{tossTimes - 1}"); 
                }
                else
                {
                    Conductor.instance.musicSource.clip = null;
                    MultiSound.Play(new MultiSound.Sound[]
                    {
                        new MultiSound.Sound("coinToss/cowbell1", 0),
                        new MultiSound.Sound("coinToss/cowbell2", 1),
                        new MultiSound.Sound("coinToss/cowbell1", 2),
                        new MultiSound.Sound("coinToss/cowbell2", 3),
                        new MultiSound.Sound("coinToss/cowbell1", 4),
                        new MultiSound.Sound("coinToss/cowbell2", 5),
                        new MultiSound.Sound("coinToss/cowbell1", 6),
                    });
                }
                Conductor.instance.SetBeat(0);
                Conductor.instance.Play(0);
                tossTimes++;
            }
        }
    }
}