using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RopeAssignment
{
    public interface IGame
    {
        void AddCreature(Orientation orientation);
        List<ICreature> LeftCreatures { get; set; }
        List<ICreature> RightCreatures { get; set; }
        //System.Timers.Timer MainTimer { get; set; } //I think I'm going to put the timer on the main class.
        
        int LeftCreatureCount { get; set; }
        int RightCreatureCount { get; set; }
        int CreaturesOnRopeCount { get; }
        void ProcessMoves(); //main section.
        bool TrafficIsGoingRight { get; set; }
        bool TrafficIsGoingLeft { get; set; }
        //Action Callback { get; set; } //if the timer isn't on the class implementing this, I suppose no callback is needed.


    }
}
