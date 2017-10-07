using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RopeAssignment
{
    public class Creature : ICreature //Normally this would be in a Models or Entity Folder & Namespace.
    {
        public virtual Position Position { get; set; }

    }
}
