using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

using game.story.game2;
using game.db;


namespace unity.main
{
    public class DebugRoomItem : MonoBehaviour
    {
        public Player player = null;
        public int item_index=-1;

        public void onClickItem()
        {
            var drop = this.GetComponent<Dropdown>();

            if ((ITEM)drop.value == player.getItem(item_index))
            {
                return;
            }
            player.setItem(item_index, (ITEM)drop.value);
            Logger.info("[DEBUG]change item");

            GameFactory.getUnityManager().updateDraw(true);
        }

    }
}

