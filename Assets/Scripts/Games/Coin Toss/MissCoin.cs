using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EndlessGames.Games.CoinToss
{
    public class MissCoin : MonoBehaviour
    {
        public void GrabCoin()
        {
            Cointoss.instance.GrabCoinFromMiss();
        }
    }
}