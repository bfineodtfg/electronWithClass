using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace electron
{
    public class Electron:UserControl
    {
        public int speed { get; set; }
        public Electron(int speed = -1) {
            InitializeComponent();
            this.speed = speed;
        }
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Electron
            // 
            this.Name = "Electron";
            this.Size = new System.Drawing.Size(25, 25);
            this.ResumeLayout(false);
            this.BackColor = System.Drawing.Color.Blue;
        }
    }
}
