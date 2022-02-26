using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EndlessGames
{
    public class CommonAnimationEvents : MonoBehaviour
    {
        public void Destroy()
        {
            Destroy(this.gameObject);
        }
    }
}
