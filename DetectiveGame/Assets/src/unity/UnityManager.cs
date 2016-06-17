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

        public void update()
        {
            if (mainCamera != null) mainCamera.updateMessage();
        }
        

    }
}
