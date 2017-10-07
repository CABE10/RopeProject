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
        protected virtual void InitializeData(int defaultLeftCount, int defaultRightCount)
        {

        }

        public virtual int CreaturesOnRopeCount
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public virtual void ProcessMoves()
        {
            throw new NotImplementedException();
        }

        public virtual void AddCreature(Orientation orientation)
        {
            throw new NotImplementedException();
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
