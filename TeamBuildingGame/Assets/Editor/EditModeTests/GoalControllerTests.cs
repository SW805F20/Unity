using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class GoalControllerTests
    {
        private GoalZoneController goalZoneController;

        [SetUp]
        public void Setup()
        {
            var controllerObject = MonoBehaviour.Instantiate(Resources.Load<GoalZoneController>("Prefabs/GoalZoneController"));
            goalZoneController = controllerObject.GetComponent<GoalZoneController>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(goalZoneController.gameObject);
        }

        [UnityTest]
        public IEnumerator GoalZoneControllerHasTwoZonesPasses()
        {
            Assert.AreEqual(goalZoneController.transform.childCount, 2);
            yield return null;

        }

        [UnityTest]
        public IEnumerator GoalZoneControllerHasTwoZonesFails()
        {
            Assert.AreNotEqual(goalZoneController.transform.childCount, 0);
            yield return null;
        }
    }
}
