using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _SmileyBread
{
    public enum PlayerAction
    {
        MoveUp,
        MoveDown,
        MoveRight,
        MoveLeft,
        Boost,
        Explode,
        Escape,
    }

    public static class InputManager
    {
        public static Dictionary<PlayerAction, KeyCode[]> boundKeys;

        static InputManager()
        {
            boundKeys = new Dictionary<PlayerAction, KeyCode[]>()
            {
                { PlayerAction.MoveUp, new KeyCode[]{ KeyCode.W, KeyCode.UpArrow } },
                { PlayerAction.MoveDown, new KeyCode[]{ KeyCode.S, KeyCode.DownArrow } },
                { PlayerAction.Boost, new KeyCode[]{ KeyCode.Space } },
                { PlayerAction.Explode, new KeyCode[]{ KeyCode.E } },
                { PlayerAction.Escape, new KeyCode[]{ KeyCode.Escape } },
            };
        }

        public static bool GetKeyDown(PlayerAction action)
        {
            foreach (KeyCode key in boundKeys[action])
            {
                if (Input.GetKeyDown(key))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool GetKeyUp(PlayerAction action)
        {
            foreach (KeyCode key in boundKeys[action])
            {
                if (Input.GetKeyUp(key))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool GetKey(PlayerAction action)
        {
            foreach (KeyCode key in boundKeys[action])
            {
                if (Input.GetKey(key))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
