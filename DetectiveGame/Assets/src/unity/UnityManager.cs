using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace unity
{
    public class UnityManager
    {
        public network.NetworkUnity net = null;

        public main.MainCamera mainCamera = null;
        public main.Noon noon = null;
        public main.Night night = null;
        public main.Midnight midnight = null;
        public title.MainCamera title = null;


        public void update()
        {
            if (mainCamera != null) mainCamera.updateDraw();
        }
        
        public void createDropdown(Dropdown drop,List<string> items,bool freset)
        {
            if (drop == null) return;
            drop.options.Clear();
            foreach (var item in items)
            {
                drop.options.Add(new Dropdown.OptionData { text = item });
            }
            if (freset)
            {
                drop.value = 1;
                drop.value = 0;
            }
        }

        public void createPlayerDropdown(Dropdown drop)
        {
            List<string> items = new List<string>();
            foreach (var p in game.GameFactory.getGame().players.players)
            {
                items.Add(p.name);
            }
            game.GameFactory.getUnityManager().createDropdown(drop, items, true);
        }
    }
}


