using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RopeAssignment
{
    public class Game : IGame
    {
        public Game(int defaultLeftCount, int defaultRightCount)
        {
            this.InitializeData(defaultLeftCount, defaultRightCount);
        }
        /// <summary>
        /// Default creature counts are set.
        /// </summary>
        protected virtual void InitializeData(int defaultLeftCount, int defaultRightCount)
        {
            this.RopeThreshhold = 3; //Monkeys on the rope at once.
            this.LeftCreatureCount = defaultLeftCount;
            this.RightCreatureCount = defaultRightCount;
            this.LeftCreatures = new List<ICreature>();
            this.RightCreatures = new List<ICreature>();
            //Adding Monkeys.
            for (int i = 0; i < this.LeftCreatureCount; i++)
            {
                this.LeftCreatures.Add(new Creature() { Position = Position.LeftStartPoint });
            }
            for (int i = 0; i < this.RightCreatureCount; i++)
            {
                this.RightCreatures.Add(new Creature() { Position = Position.RightStartPoint });
            }
        }
        /// <summary>
        /// Determines whether the creature at the front of the left line is impatient.
        /// </summary>
        protected virtual bool ImpatientLeftCreature
        {
            /*
             So this property and the next are arguably the strangest of the bunch.
             The assignment said to 'make sure the monkey at the front doesn't get impatient'.
             Well, unfortunately, making him NOT impatient is going to cost us effeciency.
             The most effecient manner is to have the rope full at all possible times,
             which we cannot do - due to the three limit threshold and this rule about not allowing impatience.
             This combined with halting monkeys on the other side is an efficiency nightmare.
            */
            get
            {
                if ((rightCounterOfCrossed >= (this.RopeThreshhold * 2)) && (this.TrafficIsGoingLeft && this.LeftCreatureCount > 0))
                {
                    //too many folks from the other side have crossed...grrr!
                    return true;
                }
                return false;
            }
        }
        private int rightCounterOfCrossed = 0;
        private int leftCounterOfCrossed = 0;
        /// <summary>
        /// Determines whether the creature at the front of the right line is impatient.
        /// </summary>
        protected virtual bool ImpatientRightCreature
        {
            get
            {

                if ((leftCounterOfCrossed >= (this.RopeThreshhold * 2)) && (this.TrafficIsGoingRight && this.RightCreatureCount > 0))
                {

                    return true;
                }
                return false;
            }
        }


        /// <summary>
        /// Adds the number of creatures on the rope.
        /// </summary>
        public virtual int CreaturesOnRopeCount
        {
            get
            {
                int result = 0;
                if (this.LeftCreatures != null)
                {
                    foreach (var monkey in this.LeftCreatures)
                    {
                        if (monkey.Position != Position.LeftStartPoint && monkey.Position != Position.RightStartPoint)
                        {
                            result++;
                        }
                    }
                }
                if (this.RightCreatures != null)
                {
                    foreach (var monkey in this.RightCreatures)
                    {
                        if (monkey.Position != Position.LeftStartPoint && monkey.Position != Position.RightStartPoint)
                        {
                            result++;
                        }
                    }
                }

                return result;
            }
        }
        /// <summary>
        /// The bulk of the logic, this is where the traffic begins to move in a direction.
        /// </summary>
        public virtual void ProcessMoves()
        {
            try
            {

                //Main Logic...
                //Do we have any monkeys, period?
                if ((LeftCreatures == null || LeftCreatures.Count() == 0) && (RightCreatures == null || RightCreatures.Count() == 0))
                {
                    this.TrafficIsGoingLeft = false;
                    this.TrafficIsGoingRight = false;
                    //No more monkeys jumping on the bed.
                    return;
                }


                if (LeftCreatures != null && LeftCreatures.Count() > 0)
                {
                    if (!this.IsCreatureCrossingFromRight) //According to the rules, we can't cross if they are crossing from the other side.
                    {
                        //Do we have a left monkey in the 3 position? Going home!
                        if (LeftCreatures.Any(l => l.Position == Position.AlmostRight))
                        {
                            LeftCreatures.RemoveAll(l => l.Position == Position.AlmostRight);
                            this.TrafficIsGoingRight = true;
                            this.TrafficIsGoingLeft = false;
                        }
                        //Do we have a left monkey in the 2 position (middle)?
                        if (LeftCreatures.Any(l => l.Position == Position.Middle))
                        {
                            //Do we have any monkeys in the next spot? It sounds like we'll never have that, but 
                            //I guess I'll check anyway.... overkill.
                            if (!LeftCreatures.Any(l => l.Position == Position.AlmostRight) &&
                                (RightCreatures == null || (RightCreatures != null && !RightCreatures.Any(r => r.Position == Position.AlmostRight))))
                            {
                                //we are clear to go.
                                //Why am I doing a foreach when I know darn well there can only be one monkey in the spot?
                                //1) Someone may override my method / use dependency injection.
                                //2) Slight laziness, and I'm not really adding that much real estate.
                                foreach (var leftMonkey in LeftCreatures.Where(l => l.Position == Position.Middle))
                                {
                                    leftMonkey.Position = Position.AlmostRight;
                                    this.TrafficIsGoingRight = true;
                                    this.TrafficIsGoingLeft = false;
                                    this.leftCounterOfCrossed++;
                                    this.rightCounterOfCrossed = 0;
                                    break;
                                }
                            }
                        }
                        //Do we have a left monkey in the 1 position?
                        if (LeftCreatures.Any(l => l.Position == Position.AlmostLeft))
                        {
                            if (!LeftCreatures.Any(l => l.Position == Position.Middle) &&
                                (RightCreatures == null || (RightCreatures != null && !RightCreatures.Any(r => r.Position == Position.Middle))))
                            {
                                foreach (var leftMonkey in LeftCreatures.Where(l => l.Position == Position.AlmostLeft))
                                {
                                    leftMonkey.Position = Position.Middle;
                                    this.TrafficIsGoingRight = true;
                                    this.TrafficIsGoingLeft = false;
                                    this.leftCounterOfCrossed++;
                                    this.rightCounterOfCrossed = 0;
                                    break;
                                }
                            }
                        }
                        //Do we have a left monkey in the 0 position?
                        if (LeftCreatures.Any(l => l.Position == Position.LeftStartPoint))
                        {
                            if (CreaturesOnRopeCount < this.RopeThreshhold)
                            {
                                if (!LeftCreatures.Any(l => l.Position == Position.AlmostLeft) &&
                                    (RightCreatures == null || (RightCreatures != null && !RightCreatures.Any(r => r.Position == Position.AlmostLeft))))
                                {
                                    if (!ImpatientRightCreature)
                                    {
                                        foreach (var leftMonkey in LeftCreatures.Where(l => l.Position == Position.LeftStartPoint))
                                        {
                                            this.IsCreatureCrossingFromLeft = true;
                                            leftMonkey.Position = Position.AlmostLeft;
                                            this.TrafficIsGoingRight = true;
                                            this.TrafficIsGoingLeft = false;
                                            this.leftCounterOfCrossed++;
                                            this.rightCounterOfCrossed = 0;

                                            break;
                                        }
                                    }

                                }
                            }
                        }
                        //unnecessary null check city, but better safe than sorry in the case of overrides/injection.
                        if (this.LeftCreatures == null || (this.LeftCreatures != null && !this.LeftCreatures.Any(l => l.Position == Position.AlmostLeft || l.Position == Position.Middle || l.Position == Position.AlmostRight)))
                        {
                            this.IsCreatureCrossingFromLeft = false;
                            this.TrafficIsGoingRight = false;
                        }
                    }
                }

                if (this.RightCreatures != null && this.RightCreatures.Count() > 0)
                {
                    if (!this.IsCreatureCrossingFromLeft) //According to the rules, we can't cross if they are crossing from the other side.
                    {
                        if (this.RightCreatures.Any(l => l.Position == Position.AlmostLeft))
                        {
                            this.RightCreatures.RemoveAll(l => l.Position == Position.AlmostLeft);
                            this.TrafficIsGoingRight = false;
                            this.TrafficIsGoingLeft = true;
                        }
                        if (this.RightCreatures.Any(l => l.Position == Position.Middle))
                        {
                            if (!RightCreatures.Any(l => l.Position == Position.AlmostLeft) &&
                                (LeftCreatures == null || (LeftCreatures != null && !LeftCreatures.Any(r => r.Position == Position.AlmostLeft))))
                            {
                                foreach (var rightMonkey in this.RightCreatures.Where(l => l.Position == Position.Middle))
                                {
                                    rightMonkey.Position = Position.AlmostLeft;
                                    this.TrafficIsGoingRight = false;
                                    this.TrafficIsGoingLeft = true;
                                    this.leftCounterOfCrossed = 0;
                                    this.rightCounterOfCrossed++;
                                    break;
                                }
                            }
                        }
                        if (this.RightCreatures.Any(l => l.Position == Position.AlmostRight))
                        {
                            if (!RightCreatures.Any(l => l.Position == Position.Middle) &&
                                (LeftCreatures == null || (LeftCreatures != null && !LeftCreatures.Any(r => r.Position == Position.Middle))))
                            {
                                foreach (var rightMonkey in this.RightCreatures.Where(l => l.Position == Position.AlmostRight))
                                {
                                    rightMonkey.Position = Position.Middle;
                                    this.TrafficIsGoingRight = false;
                                    this.TrafficIsGoingLeft = true;
                                    this.leftCounterOfCrossed = 0;
                                    this.rightCounterOfCrossed++;
                                    break;
                                }
                            }
                        }
                        if (this.RightCreatures.Any(l => l.Position == Position.RightStartPoint))
                        {
                            if (CreaturesOnRopeCount < this.RopeThreshhold)
                            {
                                if (!RightCreatures.Any(l => l.Position == Position.AlmostRight) &&
                                (LeftCreatures == null || (LeftCreatures != null && !LeftCreatures.Any(r => r.Position == Position.AlmostRight))))
                                {
                                    if (!ImpatientLeftCreature)
                                    {
                                        foreach (var rightMonkey in this.RightCreatures.Where(l => l.Position == Position.RightStartPoint))
                                        {

                                            this.IsCreatureCrossingFromRight = true;
                                            rightMonkey.Position = Position.AlmostRight;
                                            this.TrafficIsGoingRight = false;
                                            this.TrafficIsGoingLeft = true;
                                            this.leftCounterOfCrossed = 0;
                                            this.rightCounterOfCrossed++;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        if (this.RightCreatures == null || (this.RightCreatures != null && !this.RightCreatures.Any(l => l.Position == Position.AlmostLeft || l.Position == Position.Middle || l.Position == Position.AlmostRight)))
                        {
                            this.IsCreatureCrossingFromRight = false;
                            this.TrafficIsGoingLeft = false;
                        }
                    }
                }
                //We might have an unutilized left going-right monkey.
                if (this.LeftCreatures != null && this.LeftCreatures.Count() > 0 && this.CreaturesOnRopeCount == 0 && this.RopeThreshhold != 0 &&
                    LeftCreatures.Any(l => l.Position == Position.LeftStartPoint))
                {
                    //Bug fix. effeciency issue.
                    foreach (var leftMonkey in LeftCreatures.Where(l => l.Position == Position.LeftStartPoint))
                    {

                        this.IsCreatureCrossingFromLeft = true;
                        leftMonkey.Position = Position.AlmostLeft;
                        this.TrafficIsGoingRight = true;
                        this.TrafficIsGoingLeft = false;
                        this.leftCounterOfCrossed++;
                        this.rightCounterOfCrossed = 0;

                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                //Traditionally, I would put some kind of logging in here (log4net, etc).
                //Bad practice to not do anything with your catches or even do non-specific catches, but alas...

            }
            finally
            {
             
                this.LeftCreatureCount = this.LeftCreatures != null ? this.LeftCreatures.Count() : 0;
                this.RightCreatureCount = this.RightCreatures != null ? this.RightCreatures.Count() : 0;

            }
        }
        /// <summary>
        /// Adds a creature to the left or to the right.
        /// </summary>
        /// <param name="orientation">Direction</param>
        public virtual void AddCreature(Orientation orientation)
        {
            switch (orientation)
            {
                case Orientation.LeftGoingRight:
                    this.LeftCreatures.Add(new Creature() { Position = Position.LeftStartPoint });
                    this.LeftCreatureCount = this.LeftCreatures.Count();
                    break;
                case Orientation.RightGoingLeft:
                    this.RightCreatures.Add(new Creature() { Position = Position.RightStartPoint });
                    this.RightCreatureCount = this.RightCreatures.Count();
                    break;
            }
        }
        public virtual List<ICreature> LeftCreatures { get; set; }
        public virtual List<ICreature> RightCreatures { get; set; }
 
        public virtual int RopeThreshhold { get; set; }
        public virtual int LeftCreatureCount { get; set; }
        public virtual int RightCreatureCount { get; set; }
        protected virtual bool IsCreatureCrossingFromLeft { get; set; }
        protected virtual bool IsCreatureCrossingFromRight { get; set; }
        public virtual bool TrafficIsGoingRight { get; set; }
        public virtual bool TrafficIsGoingLeft { get; set; }


    }
}
