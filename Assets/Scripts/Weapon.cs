using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ProjectBonsai
{
    public class Weapon : Item
    {
        Animator anim;
        Dictionary<WeaponAnimState, string> animStateDict;
        void Start()
        {
            player_go = GameObject.FindGameObjectWithTag("Player");
            player = player_go.GetComponent<Player>();
            anim = gameObject.GetComponent<Animator>();
            animStateDict = new Dictionary<WeaponAnimState, string>()
            {
                {WeaponAnimState.Idling, "idling" },
                {WeaponAnimState.Walking, "walking" },
                {WeaponAnimState.Sprinting, "sprinting" },
                {WeaponAnimState.Crouching, "crouching" },
                {WeaponAnimState.CrouchWalking, "crouchWalking" },
                {WeaponAnimState.Jumping, "jumping" },
                {WeaponAnimState.Attacking, "attacking" },
            };
        }

        // Update is called once per frame
        void Update()
        {
            Animate();
        }

        protected override void Animate()
        {
            // Comnbat states
            if (player.GetComponent<Player>().CurrentPlayerCombatState == Player.PlayerCombatState.Attacking)
            {
                SetAnimState(WeaponAnimState.Attacking);
                anim.speed = 1;
            }
            else if (player.GetComponent<Player>().CurrentPlayerState == Player.PlayerState.Idling)
            {
                SetAnimState(WeaponAnimState.Idling);
                anim.speed = 1;
            }
            else if (player.GetComponent<Player>().CurrentPlayerState == Player.PlayerState.Walking)
            {
                SetAnimState(WeaponAnimState.Walking);
                anim.speed = 1;
            }
            else if (player.GetComponent<Player>().CurrentPlayerState == Player.PlayerState.Sprinting)
            {
                SetAnimState(WeaponAnimState.Sprinting);
                anim.speed = player.GetComponent<Player>().fpController.sprintSpeed / player.GetComponent<Player>().fpController.walkSpeed;
            }
            else if (player.GetComponent<Player>().CurrentPlayerState == Player.PlayerState.Jumping)
            {
                SetAnimState(WeaponAnimState.Jumping);
                anim.speed = 1;
            }
            else if (player.GetComponent<Player>().CurrentPlayerState == Player.PlayerState.Crouching)
            {
                SetAnimState(WeaponAnimState.Crouching);
                anim.speed = 1f - 0.75f*(player.GetComponent<Player>().fpController.speedReduction / player.GetComponent<Player>().fpController.walkSpeed);
            }
            else if (player.GetComponent<Player>().CurrentPlayerState == Player.PlayerState.CrouchWalking)
            {
                SetAnimState(WeaponAnimState.CrouchWalking);
                anim.speed = 1f - 0.75f * (player.GetComponent<Player>().fpController.speedReduction / player.GetComponent<Player>().fpController.walkSpeed);
            }
        }

        private void SetAnimState(WeaponAnimState weaponAnimState)
        {

            foreach (KeyValuePair<WeaponAnimState, string> keyValuePair in animStateDict)
            {
                if (keyValuePair.Key == weaponAnimState)
                {
                    anim.SetBool(keyValuePair.Value, true);
                }
                else
                {
                    anim.SetBool(keyValuePair.Value, false);
                }
            }
        }

        public enum WeaponAnimState
        {
            Idling,
            Walking,
            Sprinting,
            Crouching,
            CrouchWalking,
            Jumping,
            Attacking
        }

        public void AttackAnimationFinished()
        {
            player.GetComponent<Player>().CurrentPlayerCombatState = Player.PlayerCombatState.Idling;
        }

        public void AxeHit()
        {
            player.OnAttackHitFrame();
        }
    }
}
