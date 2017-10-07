using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RopeAssignment
{
    public partial class Main : Form
    {
        protected virtual IGame GameEngine { get; set; }
        protected virtual System.Timers.Timer MainTimer { get; set; }
        public Main()
        {
            InitializeComponent();
            Initialize();
        }
        /// <summary>
        /// This is where the game engine is initialized and timer set.
        /// </summary>
        protected virtual void Initialize()
        {
            int defaultInterval;
            string configInterval = System.Configuration.ConfigurationManager.AppSettings["Interval"];
            if (String.IsNullOrEmpty(configInterval) || !Int32.TryParse(configInterval, out defaultInterval))
            {
                defaultInterval = 750;//ms
            }
            this.MainTimer = new System.Timers.Timer();
            this.GameEngine = new Game(10, 10);
            this.MainTimer.Interval = defaultInterval;
            this.MainTimer.Elapsed += MainTimer_Elapsed;
            this.MainTimer.Start();
        }

        /// <summary>
        /// This is where the timer ticks.
        /// </summary>
        protected virtual void MainTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.GameEngine.ProcessMoves();
            RefreshScreen();
        }

        /// <summary>
        /// The visual aspects (images) are properly notified of their assignments.
        /// </summary>
        protected virtual void RefreshScreen()
        {
            try
            {

                //thread that owns the control.
                this.Invoke(new MethodInvoker(delegate ()
                {

                    this.lblLeftCount.Text = this.GameEngine.LeftCreatureCount.ToString();
                    this.lblRightCount.Text = this.GameEngine.RightCreatureCount.ToString();

                    if (this.GameEngine.TrafficIsGoingLeft)
                    {
                        this.imgArrowLeft.Visible = true;
                    }
                    else
                    {
                        this.imgArrowLeft.Visible = false;
                    }
                    if (this.GameEngine.TrafficIsGoingRight)
                    {
                        this.imgArrowRight.Visible = true;
                    }
                    else
                    {
                        this.imgArrowRight.Visible = false;
                    }

                    //all that we care about are three visible spaces.
                    if ((this.GameEngine.LeftCreatures != null && this.GameEngine.LeftCreatures.Any(l => l.Position == Position.AlmostLeft)) ||
                        (this.GameEngine.RightCreatures != null && this.GameEngine.RightCreatures.Any(r => r.Position == Position.AlmostLeft)))
                    {
                        this.imgPos1.Visible = true;
                    }
                    else
                    {
                        this.imgPos1.Visible = false;
                    }

                    if ((this.GameEngine.LeftCreatures != null && this.GameEngine.LeftCreatures.Any(l => l.Position == Position.Middle)) ||
        (this.GameEngine.RightCreatures != null && this.GameEngine.RightCreatures.Any(r => r.Position == Position.Middle)))
                    {
                        this.imgPos2.Visible = true;
                    }
                    else
                    {
                        this.imgPos2.Visible = false;
                    }

                    if ((this.GameEngine.LeftCreatures != null && this.GameEngine.LeftCreatures.Any(l => l.Position == Position.AlmostRight)) ||
    (this.GameEngine.RightCreatures != null && this.GameEngine.RightCreatures.Any(r => r.Position == Position.AlmostRight)))
                    {
                        this.imgPos3.Visible = true;
                    }
                    else
                    {
                        this.imgPos3.Visible = false;
                    }

                }));
            }
            catch (ObjectDisposedException ex)
            {
                //Known exception.
                //I would normally log4net this kind of thing.
            }



        }
        /// <summary>
        /// Adds a Left Creature.
        /// </summary>
        protected virtual void btnAddLeftMonkey_Click(object sender, EventArgs e)
        {
            this.GameEngine.AddCreature(Orientation.LeftGoingRight);
        }
        /// <summary>
        /// Adds a Right Creature.
        /// </summary>
        protected virtual void btnAddRightMonkey_Click(object sender, EventArgs e)
        {
            this.GameEngine.AddCreature(Orientation.RightGoingLeft);
        }
    }
}
