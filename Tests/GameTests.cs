using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

            Assert.Fail("Placeholder");
        }
        /// <summary>
        /// This tests the number of steps it takes to cross the rope.
        /// </summary>
        [TestMethod]
        public void RopeNumberOfSteps()
        {
            //Each monkey requires 4 steps to cross the rope. ONLY THREE ARE VISIBLE.
            Assert.Fail("Placeholder");
        }
        /// <summary>
        /// This test determines whether or not a monkey is getting impatient.
        /// </summary>
        [TestMethod]
        public void Impatience()
        {
            //Make sure the monkey at the front of the line doesn't get impatient.
            Assert.Fail("Placeholder");
        }
        /// <summary>
        /// This test makes sure that the monkey at the other end of the chasm is halted when there are others crossing.
        /// </summary>
        [TestMethod]
        public void MonkeysHaltedOnOtherSide()
        {
            //Once a monkey starts across the rope, the monkeys on the other side of the rope are not allowed to begin crossing.
            Assert.Fail("Placeholder");
        }
        /// <summary>
        /// This tests the ability to add a monkey/creature.
        /// </summary>
        [TestMethod]
        public void AddMonkey()
        {
            Assert.Fail("Placeholder");
        }

        /// <summary>
        /// This is just an ioc test.
        /// </summary>
        [TestMethod]
        public void InjectableGameTest()
        {
            Assert.Fail("Placeholder");
        }

    }
}
