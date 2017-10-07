using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RopeAssignment;
using System.Linq;
namespace Tests
{
    [TestClass]
    public class GameTests
    {
        /// <summary>
        /// This tests the maximum number of monkeys on the rope at a time.
        /// </summary>
        [TestMethod]
        public void RopeThreshhold()
        {
            //The rope can hold a maximum of 3 monkeys at a time.

            IGame game = new Game(4, 0);
            game.ProcessMoves();
            Assert.IsTrue(game.CreaturesOnRopeCount == 1);
            game.ProcessMoves();
            Assert.IsTrue(game.CreaturesOnRopeCount == 2);
            game.ProcessMoves();
            Assert.IsTrue(game.CreaturesOnRopeCount == 3);
            game.ProcessMoves();
            Assert.IsTrue(game.CreaturesOnRopeCount == 3);
            game.ProcessMoves();
            Assert.IsTrue(game.CreaturesOnRopeCount == 2);




        }
        /// <summary>
        /// This tests the number of steps it takes to cross the rope.
        /// </summary>
        [TestMethod]
        public void RopeNumberOfSteps()
        {
            //Each monkey requires 4 steps to cross the rope. ONLY THREE ARE VISIBLE.
            IGame game = new Game(1, 0);
            game.ProcessMoves();
            Assert.IsTrue(game.LeftCreatureCount == 1);
            game.ProcessMoves();
            Assert.IsTrue(game.LeftCreatureCount == 1);
            game.ProcessMoves();
            Assert.IsTrue(game.LeftCreatureCount == 1);
            game.ProcessMoves();
            Assert.IsTrue(game.LeftCreatureCount == 0);
        }
        /// <summary>
        /// This test determines whether or not a monkey is getting impatient.
        /// </summary>
        [TestMethod]
        public void Impatience()
        {
            //Make sure the monkey at the front of the line doesn't get impatient.
            IGame game = new Game(4, 2);
            for (int x = 0; x < 3; x++)
            {
                game.ProcessMoves();
            }
            Assert.IsTrue(game.CreaturesOnRopeCount == 3);
            Assert.IsTrue(game.LeftCreatures.Count(l => l.Position == Position.LeftStartPoint) == 1);
            Assert.IsTrue(game.RightCreatures.All(r => r.Position == Position.RightStartPoint));
            game.ProcessMoves();
            Assert.IsTrue(game.LeftCreatures.Count(l => l.Position == Position.LeftStartPoint) == 1);
            Assert.IsTrue(game.LeftCreatures.Count(l => l.Position == Position.AlmostLeft) == 0);
            game.ProcessMoves();
            Assert.IsTrue(game.LeftCreatures.Count(l => l.Position == Position.Middle) == 0);
            game.ProcessMoves();
            Assert.IsTrue(game.LeftCreatures.Count(l => l.Position == Position.AlmostRight) == 0);
            Assert.IsTrue(game.RightCreatures.Count(r => r.Position == Position.AlmostRight) == 1);

        }
        /// <summary>
        /// This test makes sure that the monkey at the other end of the chasm is halted when there are others crossing.
        /// </summary>
        [TestMethod]
        public void MonkeysHaltedOnOtherSide()
        {
            //Once a monkey starts across the rope, the monkeys on the other side of the rope are not allowed to begin crossing.
            IGame game = new Game(4, 2);
            game.ProcessMoves();
            Assert.IsTrue(game.RightCreatures.All(r => r.Position == Position.RightStartPoint));
            game.ProcessMoves();
            Assert.IsTrue(game.RightCreatures.All(r => r.Position == Position.RightStartPoint));
            game.ProcessMoves();
            Assert.IsTrue(game.RightCreatures.All(r => r.Position == Position.RightStartPoint));

        }
        /// <summary>
        /// This tests the ability to add a monkey/creature.
        /// </summary>
        [TestMethod]
        public void AddMonkey()
        {
            IGame game = new Game(0, 0);
            Assert.IsTrue(game.LeftCreatureCount == 0 && (game.LeftCreatures == null || game.LeftCreatures.Count() == 0));
            game.AddCreature(Orientation.LeftGoingRight);
            Assert.IsTrue(game.LeftCreatureCount == 1 && (game.LeftCreatures == null || game.LeftCreatures.Count() == 1));

        }

        /// <summary>
        /// This is just an ioc test.
        /// </summary>
        [TestMethod]
        public void InjectableGameTest()
        {
            //ill get to this one soon.
            Assert.Fail("Placeholder");
        }

    }
}
