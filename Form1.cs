using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace electron
{
    public partial class Form1 : Form
    {
        Timer moveTimer = new Timer();
        Timer spawTimer = new Timer();
        List<Electron> electrons = new List<Electron>();
        int batteryVolage = 24;
        int speedDivider = 4;
        int setVoltage = 12;

        int passed = 0;
        public Form1()
        {
            InitializeComponent();
            Start();
        }
        void Start()
        {
            StartTimers();
            AddEvents();
        }
        void StartTimers()
        {
            moveTimer.Start();
            spawTimer.Start();

            moveTimer.Interval = 16;
            spawTimer.Interval = 500;

            moveTimer.Tick += MoveEvent;
            spawTimer.Tick += SpawnEvent;
        }
        void AddEvents()
        {
            button1.Click += UpEvent;
            button2.Click += DownEvent;
        }
        void UpEvent(object s, EventArgs e)
        {
            if (setVoltage < 24)
            {
                UpdateVoltage(1);
            }
        }
        void DownEvent(object s, EventArgs e)
        {
            if ((setVoltage-1)/speedDivider > 0)
            {
                UpdateVoltage(-1);
            }
        }
        void MoveEvent(object s, EventArgs e)
        {
            List<Electron> deleteThese = new List<Electron>();
            foreach (Electron item in electrons)
            {
                //Ha az elektron az első veetéken van
                if (item.Top > wire2.Top && item.Bounds.IntersectsWith(wire1.Bounds))
                    item.Top -= batteryVolage / speedDivider;

                //Ha az elektron a második vezetéken van
                else if (item.Top < wire2.Top && item.Bounds.IntersectsWith(wire2.Bounds))
                    item.Left -= batteryVolage / speedDivider;
                //Amikor az elektron a feszültség szabályozó mögött van
                else if (item.Left < wire2.Left && item.Bounds.IntersectsWith(voltageRegulator.Bounds))
                    item.Left -= batteryVolage / speedDivider;
                //Amikor a 3. vezetéken megy
                else if (item.Bounds.IntersectsWith(wire3.Bounds))
                {
                    //beállítja a sebességet ha még nincs neki
                    if (item.speed < 0)
                        item.speed = int.Parse(voltage.Text.Trim('V'));

                    item.Left -= item.speed / speedDivider;
                }
                //Amikor már nem látszik az elektron és a lámpa mögött van
                else if (item.Bounds.IntersectsWith(lamp.Bounds))
                {
                    if (item.Left > wire4.Left)
                        item.Left -= item.speed / speedDivider;
                    else
                        item.Top += item.speed / speedDivider;
                }
                //amikor a 4. vezetéken megy
                else if (item.Bounds.IntersectsWith(wire4.Bounds) && !item.Bounds.IntersectsWith(wire5.Bounds))
                {
                    if (item.Top < wire5.Top)
                        item.Top += item.speed / speedDivider;
                }
                //amikor az 5. vezetéken megy
                else if (item.Bounds.IntersectsWith(wire5.Bounds) && item.Left < wire5.Right)
                {
                    item.Left += item.speed / speedDivider;
                }
                //amikor elérte a pozitív terminált
                else if (item.Bounds.IntersectsWith(positiveTerminal.Bounds))
                {
                    if (!item.Visible)
                    {
                        passed++;
                        deleteThese.Add(item);
                        UpdatePassed();
                    }
                    item.Hide();
                }
            }
            foreach (Electron item in deleteThese)
            {
                electrons.Remove(item);
            }
        }
        void SpawnEvent(object s, EventArgs e)
        {
            Electron oneElectron = new Electron();
            oneElectron.Top = wire1.Bottom - oneElectron.Height;
            oneElectron.Left = (wire1.Left + wire1.Right) / 2 - oneElectron.Width / 2;
            this.Controls.Add(oneElectron);
            electrons.Add(oneElectron);
        }
        void UpdateVoltage(int difference)
        {
            setVoltage += difference;
            voltage.Text = setVoltage + "V";
        }
        void UpdatePassed()
        {
            passedLabel.Text = $"Electrons passed: {passed}";
        }
    }
}
