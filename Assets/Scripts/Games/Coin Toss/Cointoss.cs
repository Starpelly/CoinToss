using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EndlessGames.Util;
using NaughtyBezierCurves;

namespace EndlessGames.Games.CoinToss
{
    public class Cointoss : PlayerActionObject
    {
        [Header("Components")] 
        public GameObject player;
        public GameObject coinMissPrefab;
        public GameObject timePanel;
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
        private bool panelCounting = false;
        public int score;
        
        public static Cointoss instance { get; set; }
        
        private void Start()
        {
            instance = this;
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
                else if (state.late)
                {
                    Miss();
                }
            }

            if (panelCounting && Conductor.instance.songPosition < 10f)
            {
                SetPanelText(Conductor.instance.songPosition);
            }
        }

        private void Catch()
        {
            Jukebox.PlayOneShotGame("coinToss/catch");
            player.GetComponent<Animator>().Play("Catch", 0, 0);
            
            isTossing = false;
            isTossingAnim = true;
            isCounting = false;
            panelCounting = false;

            score++;
            timePanel.SetActive(true);
            
            SetScoreText();

            ResetState();
        }

        private void Miss()
        {
            Instantiate(coinMissPrefab, this.transform).SetActive(true);
            
            Jukebox.PlayOneShotGame("coinToss/miss");
            tossTimes = 0;
            isTossing = false;
            isTossingAnim = true;
            isCounting = false;

            ResetState();
        }

        public void GrabCoinFromMiss()
        {
            player.GetComponent<Animator>().Play("Miss", 0, 0);
        }

        public void ResetGame()
        {
            Debug.Log(Time.frameCount + "LOLs");
            score = 0;
            panelCounting = false;
            Conductor.instance.musicSource.pitch = 1;
            SetScoreText();
            SetPanelText(0, true);
            timePanel.SetActive(true);
        }

        private void SetScoreText()
        {
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
        }

        private void SetPanelText(float songPos, bool reset = false)
        {
            second.sprite = seconds[(int)(songPos)];
            string songPosStr = songPos.ToString();
            if (songPosStr.Length >= 4)
            {
                if (reset)
                {
                    millisecond0.sprite = milliseconds[0];
                    millisecond1.sprite = milliseconds[0];   
                }
                else
                {
                    millisecond0.sprite = milliseconds[int.Parse(songPosStr[2].ToString())];
                    millisecond1.sprite = milliseconds[int.Parse(songPosStr[3].ToString())];   
                }
            }
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
                panelCounting = true;

                if (tossTimes > 8)
                {
                    tossTimes = 0;
                    
                    Conductor.instance.musicSource.pitch -= 0.09f;

                    Conductor.instance.musicSource.pitch = Mathf.Clamp(Conductor.instance.musicSource.pitch, 0.19f, 3f);
                }

                if (tossTimes > 0)
                {
                    timePanel.SetActive(false);
                    Conductor.instance.musicSource.clip = Jukebox.LoadSong($"Cointoss_{tossTimes - 1}"); 
                }
                else
                {
                    timePanel.SetActive(true);
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