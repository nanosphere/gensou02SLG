using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


namespace unity
{
    public class MyDropdownUnity : MyDropdown
    {
        public Dropdown dropdown = null;
        public MyDropdownUnity(Dropdown drop)
        {
            dropdown = drop;
        }

        public override int getSelect()
        {
            if (0 <= dropdown.value && dropdown.value < option.Count)
            {
                return option[dropdown.value];
            }
            return 0;
        }
        //GameObject.Find("item").GetComponent<Dropdown>().value
        //GameObject.Find("chara").GetComponent<Dropdown>().captionText.text;
        public override void select(int id)
        {
            for (int i = 0; i < option.Count; i++)
            {
                if (id == option[i])
                {
                    dropdown.value = i;
                    return;
                }
            }
        }
        public override void updateDraw(bool update)
        {
            if (dropdown == null) return;
            dropdown.options.Clear();
            if (items.Count == 0)
            {
                add("選択肢なし", -1);
            }
            foreach (var item in items)
            {
                dropdown.options.Add(new Dropdown.OptionData { text = item });
            }

            if (update)
            {
                dropdown.RefreshShownValue();
            }
        }
    }

    public class MyDropdownDebug : MyDropdown
    {
        int nselect = 0;
        public override int getSelect()
        {
            if (option.Count == 0) return 0;
            return option[nselect];
        }
        public override void select(int id)
        {
            for (int i = 0; i < option.Count; i++)
            {
                if (id == option[i])
                {
                    nselect = i;
                    return;
                }
            }
        }
        public override void updateDraw(bool update)
        {
        }

    }
    public abstract class MyDropdown
    {
        public List<string> items = new List<string>();
        public List<int> option = new List<int>();
        
        public void clear()
        {
            items.Clear();
            option.Clear();
        }

        public void add(string item, int id)
        {
            items.Add(item);
            option.Add(id);
        }
        public abstract int getSelect();
        public abstract void select(int id);
        public abstract void updateDraw(bool update);

    }
}


