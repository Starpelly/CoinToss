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
        [SerializeField] private SpriteRenderer dialogue;
        [SerializeField] private Sprite[] dialogueSpr;
        public GameObject ScoreInputField;
        public TMPro.TMP_Text pitchTEst;
        
        [Header("Properties")]
        private bool isTossing;
        private bool isTossingAnim;
        private bool isCounting;
        private int tossTimes = 0;
        private bool panelCounting = false;
        public int score;
        private bool isFailing = false;
        private bool hasDoneTutorial = false;
        private int totalTossTimes = 0;
        private bool inActive = true;
        public float pitch;

        private float spamTimer = 0;

        public TMPro.TMP_Text tooltipText;

        public static Cointoss instance { get; set; }
        
        private void Start()
        {
            instance = this;
            Flicking.OnFlick += Toss;
        }
        private void Update()
        {
            spamTimer += Time.deltaTime;
            bool canCatch = false;

            if (isCounting)
            {
                float normalizedBeat = Conductor.instance.GetPositionFromBeat(0, 6);
                StateCheck(normalizedBeat);
            }

            if (!isFailing)
            {
                if (PlayerInput.Tapped())
                {
                    if (spamTimer >= 0.35f)
                    {
                        if (state.perfect) canCatch = true;
                        spamTimer = 0;
                    }

                    if (!isTossing) player.GetComponent<Animator>().Play("Prepare_Coin", 0,0 );
                    else player.GetComponent<Animator>().Play("Prepare", 0,0 );
                    isTossingAnim = false;
                }
                else if (PlayerInput.TappedRelease() && !isTossingAnim)
                {
                    if (!isTossing) player.GetComponent<Animator>().Play("Idle_Coin", 0,0 );
                    else player.GetComponent<Animator>().Play("Idle", 0,0 );
                }
            }

            if (isCounting)
            {
                if (canCatch)
                {
                    if (PlayerInput.Tapped())
                    {
                        Catch();
                    }
                }
                if (state.late)
                {
                    Miss();
                }
            }

            if (!hasDoneTutorial && Conductor.instance.songPositionInBeats >= 4f)
            {
                dialogue.sprite = null;
            }
        }

        private void LateUpdate()
        {
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

            hasDoneTutorial = true;
        }

        private void Miss()
        {
            Instantiate(coinMissPrefab, this.transform).SetActive(true);
            
            Jukebox.PlayOneShotGame("coinToss/miss");
            tossTimes = 0;
            isTossing = false;
            isTossingAnim = true;
            isCounting = false;
            isFailing = true;

            ResetState();
        }

        public void GrabCoinFromMiss()
        {
            player.GetComponent<Animator>().Play("Miss", 0, 0);
        }

        public void ResetGame()
        {
            if (score >= 1)
            {
                ScoreInputField.SetActive(true);
                LeaderboardController.instance.currentScore = score;
            }

            timePanel.SetActive(true);
            score = 0;
            panelCounting = false;
            pitch = 1;
            Conductor.instance.musicSource.time = 0;
            Conductor.instance.musicSource.clip = null;
            Conductor.instance.Stop(0);
            SetScoreText();
            SetPanelText(0, true);
            isFailing = false;
        }

        private void SetScoreText()
        {
            if (score <= 99)
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
                    scoreSprite2.sprite = null;

                    scoreSprite.transform.localPosition = new Vector3(0.03125f, scoreSprite.transform.localPosition.y);

                    scoreSprite2.gameObject.SetActive(false);
                }
            }
        }

        private void SetPanelText(float songPos, bool reset = false)
        {
            second.sprite = seconds[(int)(songPos)];
            string songPosStr = songPos.ToString();
            if (songPosStr.Length >= 4 || reset)
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
            if (!hasDoneTutorial)
            {
                dialogue.sprite = dialogueSpr[1];
            }

            if (isFailing) return;

            if (!isTossing) player.GetComponent<Animator>().Play("Flick_Coin", 0,0 );
            else player.GetComponent<Animator>().Play("Flick", 0,0 );

            if (ScoreInputField.gameObject.activeInHierarchy)
                ScoreInputField.SetActive(false);

            isTossingAnim = true;
            if (!isTossing)
            {
                isTossing = true;
                totalTossTimes++;
                Jukebox.PlayOneShotGame("coinToss/flick");

                isCounting = true;
                panelCounting = true;


                if (tossTimes > 8)
                {
                    tossTimes = 0;

                    pitch -= 0.09f;

                    pitch = Mathf.Clamp(pitch, 0.19f, 3f);
                    Conductor.instance.musicSource.pitch = this.pitch;
                }

                Conductor.instance.SetBeat(0);
                Conductor.instance.Play(0);

                if (tossTimes > 0)
                {
                    timePanel.SetActive(false);
                    if (score > 27)
                    {
                        int[] first = new int[] { 0, 2, 4, 6, 8, 10 };
                        int[] second = new int[] { 1, 3, 5, 7, 9, 11 };
                        if (score % 2 == 0)
                            Conductor.instance.musicSource.clip = Jukebox.LoadSong($"CoinToss/Cointoss_{first[Random.Range(0, first.Length)]}");
                        else
                            Conductor.instance.musicSource.clip = Jukebox.LoadSong($"CoinToss/Cointoss_{second[Random.Range(0, second.Length)]}");
                    }
                    else
                    {
                        Conductor.instance.musicSource.clip = Jukebox.LoadSong($"CoinToss/Cointoss_{tossTimes - 1}");
                    }
                    Conductor.instance.musicSource.Play();
                    Conductor.instance.musicSource.pitch = this.pitch;
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
                    Conductor.instance.musicSource.pitch = this.pitch;
                }


                tossTimes++;
            }
        }
    }
}