using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBonsai
{
    public class Weapon : Item
    {
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
        
        }

        protected override void Animate()
        {
            if (player.GetComponent<Player>().CurrentPlayerState == Player.PlayerState.Idle)
            {

            }
            else if (player.GetComponent<Player>().CurrentPlayerState == Player.PlayerState.Walking)
            {

            }
            else if (player.GetComponent<Player>().CurrentPlayerState == Player.PlayerState.Sprinting)
            {

            }
        }
    }
}
