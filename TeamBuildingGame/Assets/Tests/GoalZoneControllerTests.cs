using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEditor;

namespace Tests
{

    public class GoalZoneControllerTests
    {
        private GameObject playingField;
        private GameObject goalZoneController;
        private GameObject blueGoal;
        private GameObject redGoal;

        [SetUp]
        public void SetUp()
        {
            SceneManager.LoadScene(0);
            playingField = GameObject.FindGameObjectWithTag("PlayingField");
            
        }

        [TearDown]
        public void TearDown()
        {
            Object.Destroy(goalZoneController.gameObject);
        }

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
            goalZoneController = GameObject.Find("GoalZoneController");
            Assert.AreEqual(goalZoneController.transform.childCount, 2);
            yield return null;

        }

        [UnityTest]
        public IEnumerator GoalZoneControllerHasTwoZonesFails()
        {
            goalZoneController = GameObject.Find("GoalZoneController");
            Assert.AreNotEqual(goalZoneController.transform.childCount, 0);
            yield return null;
        }
    }
}
