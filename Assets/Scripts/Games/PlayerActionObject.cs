using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EndlessGames.Games
{
    public class PlayerActionObject : MonoBehaviour
    {
        public bool inList = false;
        public int lastState;
        public Minigame.Eligible state = new Minigame.Eligible();
        public bool isEligible;

        public List<Minigame.Eligible> eligibleHitsList = new List<Minigame.Eligible>();

        public int aceTimes;

        private bool autoPlayEnabledOnStart;

        public void PlayerActionInit(GameObject g, float createBeat)
        {
            state.gameObject = g;
            state.createBeat = createBeat;

            autoPlayEnabledOnStart = GameManager.instance.autoplay;
        }

        private void CheckForAce(float normalizedBeat, bool autoPlay = false)
        {
            if (aceTimes == 0)
            {
                if (GameManager.instance.autoplay && normalizedBeat > 0.99f || autoPlay && normalizedBeat > 0.99f)
                {
                    OnAce();
                    // aceTimes++;
                }
            }
        }

        public void ResetAce()
        {
            aceTimes = 0;
        }

        public void ResetState()
        {
            lastState = 0;
            ResetAce();
        }

        // could possibly add support for custom early, perfect, and end times if needed.
        public void StateCheck(float normalizedBeat, bool autoPlay = false)
        {
            CheckForAce(normalizedBeat, autoPlay);
            if (normalizedBeat > Minigame.EarlyTime() && normalizedBeat < Minigame.PerfectTime() && lastState == 0)
            {
                MakeEligible(true, false, false);
                lastState++;
            }
            // Perfect State
            else if (normalizedBeat > Minigame.PerfectTime() && normalizedBeat < Minigame.LateTime() && lastState == 1)
            {
                MakeEligible(false, true, false);
                lastState++;
            }
            // Late State
            else if (normalizedBeat > Minigame.LateTime() && normalizedBeat < Minigame.EndTime() && lastState == 2)
            {
                MakeEligible(false, false, true);
                lastState++;
            }
            else if (normalizedBeat < Minigame.EarlyTime() || normalizedBeat > Minigame.EndTime())
            {
                MakeInEligible();
            }
        }

        public void MakeEligible(bool early, bool perfect, bool late)
        {
            if (!inList)
            {
                state.early = early;
                state.perfect = perfect;
                state.late = late;

                eligibleHitsList.Add(state);
                inList = true;
            }
            else
            {
                Minigame.Eligible es = eligibleHitsList[eligibleHitsList.IndexOf(state)];
                es.early = early;
                es.perfect = perfect;
                es.late = late;
            }
        }

        public void MakeInEligible()
        {
            state.early = false;
            state.perfect = false;
            state.late = false;
            if (!inList) return;

            eligibleHitsList.Remove(state);
            inList = false;
        }

        public void RemoveObject(int currentHitInList, bool destroyObject = false)
        {
            if (currentHitInList < eligibleHitsList.Count)
            {
                eligibleHitsList.Remove(eligibleHitsList[currentHitInList]);
                currentHitInList++;
                if (destroyObject) Destroy(this.gameObject);
            }
        }
        
        // No list
        public void StateCheckNoList(float normalizedBeat)
        {
            CheckForAce(normalizedBeat);
            if (normalizedBeat > Minigame.EarlyTime() && normalizedBeat < Minigame.PerfectTime() && lastState == 0)
            {
                ModifyState(true, false, false);
                lastState++;
            }
            // Perfect State
            else if (normalizedBeat > Minigame.PerfectTime() && normalizedBeat < Minigame.LateTime() && lastState == 1)
            {
                ModifyState(false, true, false);
                lastState++;
            }
            // Late State
            else if (normalizedBeat > Minigame.LateTime() && normalizedBeat < Minigame.EndTime() && lastState == 2)
            {
                ModifyState(false, false, true);
                lastState++;
            }
            else if (normalizedBeat < Minigame.EarlyTime() || normalizedBeat > Minigame.EndTime())
            {
                // ineligible
            }
        }

        public virtual void OnAce()
        {
        }

        private void ModifyState(bool early, bool perfect, bool late)
        {
            state.early = early;
            state.perfect = perfect;
            state.late = late;
        }

        private void OnDestroy()
        {
            MakeInEligible();
        }
    }
}