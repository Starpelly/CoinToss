using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EndlessGames.Games.CoinToss
{
    public class CointossPlayer : MonoBehaviour
    {
        public void ResetState()
        {
            Cointoss.instance.ResetGame();
        }
    }

}
