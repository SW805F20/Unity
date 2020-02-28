using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests
{
    public class GoalZoneControllerTests
    {
        // A Test behaves as an ordinary method
        [Test]
        public void GoalZoneControllerTestsSimplePasses()
        {
            // Use the Assert class to test conditions
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator GoalZoneControllerHasTwoZonesPasses()
        {
            SceneManager.LoadScene(0);
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;

            var goalZoneController = GameObject.Find("GoalZoneController");

            Assert.AreEqual(goalZoneController.transform.childCount, 2);
        }

        [UnityTest]
        public IEnumerator GoalZoneControllerHasTwoZonesFails()
        {
            SceneManager.LoadScene(0);
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;

            var goalZoneController = GameObject.Find("GoalZoneController");

            Assert.AreNotEqual(goalZoneController.transform.childCount, 0);
        }
    }
}
