using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

using game.story.game2;
using game.db;


namespace unity.main
{
    public class DebugRoomState : MonoBehaviour
    {
        public Player player = null;

        public void onClickState()
        {
            var drop = this.GetComponent<Dropdown>();

            if ((PLAYER_STATE)drop.value == player.state)
            {
                return;
            }
            player.state = (PLAYER_STATE)drop.value;
            Logger.info("[DEBUG]change state");

            GameFactory.getUnityManager().updateDraw(true);

        }

    }
}

