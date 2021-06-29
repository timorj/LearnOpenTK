﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK;

namespace GammaCorrection
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
               
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (Window game = new Window(800, 600, "Gamma Correction"))
            {
                game.Run(60);
            }
        }
    }
}
