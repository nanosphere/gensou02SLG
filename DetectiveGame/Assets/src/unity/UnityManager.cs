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
        public main.MainCamera mainCamera = null;
        public main.Noon noon = null;
        public main.Midnight midnight = null;

        public void update()
        {
            if (mainCamera != null) mainCamera.updateMessage();
        }
        
        public void createDropdown(Dropdown drop,List<string> items,bool freset)
        {
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
    }
    
}
