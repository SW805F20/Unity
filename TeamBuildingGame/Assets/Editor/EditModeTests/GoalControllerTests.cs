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
        private FieldGenerator playingField;
        private GoalZoneRenderer renderer;

        [SetUp]
        public void Setup()
        {
            var controllerObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/GoalZoneController"));
            goalZoneController = controllerObject.GetComponent<GoalZoneController>();
            var playingFieldObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/PlayingField"));
            playingField = playingFieldObject.GetComponent<FieldGenerator>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(goalZoneController.gameObject);
            Object.DestroyImmediate(playingField.gameObject);
        }

        void CreateVerticalField()
        {
            Vector3 anchor1, anchor2, anchor3, anchor4;
            anchor1 = new Vector3(0, 0, 0);
            anchor2 = new Vector3(0, 20, 0);
            anchor3 = new Vector3(10, 0, 0);
            anchor4 = new Vector3(10, 20, 0);

            playingField.anchor1 = anchor1;
            playingField.anchor2 = anchor2;
            playingField.anchor3 = anchor3;
            playingField.anchor4 = anchor4;

            goalZoneController.goalZoneEdgeLength = 2f;
            goalZoneController.goalZoneMiddleOffset = 1f;
            goalZoneController.maxX = 10f;
            goalZoneController.maxY = 20f;
            goalZoneController.minX = 0f;
            goalZoneController.minY = 0f;
        }

        void CreateHorizontalField()
        {
            Vector3 anchor1, anchor2, anchor3, anchor4;
            anchor1 = new Vector3(0, 0, 0);
            anchor2 = new Vector3(0, 10, 0);
            anchor3 = new Vector3(20, 0, 0);
            anchor4 = new Vector3(20, 10, 0);

            playingField.anchor1 = anchor1;
            playingField.anchor2 = anchor2;
            playingField.anchor3 = anchor3;
            playingField.anchor4 = anchor4;

            goalZoneController.goalZoneEdgeLength = 2f;
            goalZoneController.goalZoneMiddleOffset = 1f;
            goalZoneController.maxX = 20f;
            goalZoneController.maxY = 10f;
            goalZoneController.minX = 0f;
            goalZoneController.minY = 0f;
            goalZoneController.xDifference = 20f;
            goalZoneController.yDifference = 10f;
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
