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

        [UnityTest]
        public IEnumerator GoalZoneControllerDefineAnchorsReturnsFourAnchors()
        {
            var anchors = goalZoneController.DefineAnchorsForGoal(new Vector2(2f, 4f));
            Assert.True(anchors.Length == 4);

            yield return null;
        }

        [UnityTest]
        public IEnumerator GoalZoneControllerDefineAnchorsPasses()
        {
            goalZoneController.goalZoneMiddleOffset = 1; 
            var anchors = goalZoneController.DefineAnchorsForGoal(new Vector2(2f, 4f));
            Vector3 expectedAnchor1 = new Vector3(1f, 3f, 1f);
            Vector3 expectedAnchor2 = new Vector3(1f, 5f, 1f);
            Vector3 expectedAnchor3 = new Vector3(3f, 3f, 1f);
            Vector3 expectedAnchor4 = new Vector3(3f, 5f, 1f);

            Assert.AreEqual(expectedAnchor1, anchors[0]);
            Assert.AreEqual(expectedAnchor2, anchors[1]);
            Assert.AreEqual(expectedAnchor3, anchors[2]);
            Assert.AreEqual(expectedAnchor4, anchors[3]);

            yield return null;
        }

        [UnityTest]
        public IEnumerator GoalZoneControllerDefineAnchorsFails()
        {
            goalZoneController.goalZoneMiddleOffset = 1;
            var anchors = goalZoneController.DefineAnchorsForGoal(new Vector2(2f, 4f));
            Vector3 NotExpectedAnchor1 = new Vector3(5f, 3f, 1f);
            Vector3 NotExpectedAnchor2 = new Vector3(5f, 5f, 1f);
            Vector3 NotExpectedAnchor3 = new Vector3(5f, 3f, 1f);
            Vector3 NotExpectedAnchor4 = new Vector3(5f, 5f, 1f);

            Assert.AreNotEqual(NotExpectedAnchor1, anchors[0]);
            Assert.AreNotEqual(NotExpectedAnchor2, anchors[1]);
            Assert.AreNotEqual(NotExpectedAnchor3, anchors[2]);
            Assert.AreNotEqual(NotExpectedAnchor4, anchors[3]);

            yield return null;
        }


        [UnityTest]
        public IEnumerator GoalZoneControllerDefineVerticalBlueGoalPasses()
        {
            CreateVerticalField();

            Vector3 expectedAnchor1 = new Vector3(4f, 18f, 1f);
            Vector3 expectedAnchor2 = new Vector3(4f, 20f, 1f);
            Vector3 expectedAnchor3 = new Vector3(6f, 18f, 1f);
            Vector3 expectedAnchor4 = new Vector3(6f, 20f, 1f);
            var anchors = goalZoneController.DefineVerticalBlueGoal();
            Assert.AreEqual(expectedAnchor1, anchors[0]);
            Assert.AreEqual(expectedAnchor2, anchors[1]);
            Assert.AreEqual(expectedAnchor3, anchors[2]);
            Assert.AreEqual(expectedAnchor4, anchors[3]);

            yield return null;
        }

        [UnityTest]
        public IEnumerator GoalZoneControllerDefineVerticalBlueGoalFails()
        {
            CreateVerticalField();

            Vector3 expectedAnchor1 = new Vector3(1f, 18f, 1f);
            Vector3 expectedAnchor2 = new Vector3(1f, 20f, 1f);
            Vector3 expectedAnchor3 = new Vector3(2f, 18f, 1f);
            Vector3 expectedAnchor4 = new Vector3(2f, 20f, 1f);
            var anchors = goalZoneController.DefineVerticalBlueGoal();
            Assert.AreNotEqual(expectedAnchor1, anchors[0]);
            Assert.AreNotEqual(expectedAnchor2, anchors[1]);
            Assert.AreNotEqual(expectedAnchor3, anchors[2]);
            Assert.AreNotEqual(expectedAnchor4, anchors[3]);

            yield return null;
        }

        [UnityTest]
        public IEnumerator GoalZoneControllerDefineVerticalRedGoalPasses()
        {
            CreateVerticalField();

            Vector3 expectedAnchor1 = new Vector3(4f, 0f, 1f);
            Vector3 expectedAnchor2 = new Vector3(4f, 2f, 1f);
            Vector3 expectedAnchor3 = new Vector3(6f, 0f, 1f);
            Vector3 expectedAnchor4 = new Vector3(6f, 2f, 1f);
            var anchors = goalZoneController.DefineVerticalRedGoal();
            Assert.AreEqual(expectedAnchor1, anchors[0]);
            Assert.AreEqual(expectedAnchor2, anchors[1]);
            Assert.AreEqual(expectedAnchor3, anchors[2]);
            Assert.AreEqual(expectedAnchor4, anchors[3]);

            yield return null;
        }

        [UnityTest]
        public IEnumerator GoalZoneControllerDefineVerticalRedGoalFails()
        {
            CreateVerticalField();

            Vector3 expectedAnchor1 = new Vector3(4f, 12f, 1f);
            Vector3 expectedAnchor2 = new Vector3(4f, 12f, 1f);
            Vector3 expectedAnchor3 = new Vector3(6f, 12f, 1f);
            Vector3 expectedAnchor4 = new Vector3(6f, 12f, 1f);
            var anchors = goalZoneController.DefineVerticalRedGoal();
            Assert.AreNotEqual(expectedAnchor1, anchors[0]);
            Assert.AreNotEqual(expectedAnchor2, anchors[1]);
            Assert.AreNotEqual(expectedAnchor3, anchors[2]);
            Assert.AreNotEqual(expectedAnchor4, anchors[3]);

            yield return null;
        }

        [UnityTest]
        public IEnumerator GoalZoneControllerDefineHorizontalBlueGoalPasses()
        {
            CreateHorizontalField();

            Vector3 expectedAnchor1 = new Vector3(0f, 4f, 1f);
            Vector3 expectedAnchor2 = new Vector3(0f, 6f, 1f);
            Vector3 expectedAnchor3 = new Vector3(2f, 4f, 1f);
            Vector3 expectedAnchor4 = new Vector3(2f, 6f, 1f);
            var anchors = goalZoneController.DefineHorizontalBlueGoal();
            Assert.AreEqual(expectedAnchor1, anchors[0]);
            Assert.AreEqual(expectedAnchor2, anchors[1]);
            Assert.AreEqual(expectedAnchor3, anchors[2]);
            Assert.AreEqual(expectedAnchor4, anchors[3]);

            yield return null;
        }

        [UnityTest]
        public IEnumerator GoalZoneControllerDefineHorizontalBlueGoalFails()
        {
            CreateHorizontalField();

            Vector3 expectedAnchor1 = new Vector3(4f, 4f, 1f);
            Vector3 expectedAnchor2 = new Vector3(7f, 6f, 1f);
            Vector3 expectedAnchor3 = new Vector3(12f, 4f, 1f);
            Vector3 expectedAnchor4 = new Vector3(14f, 6f, 1f);
            var anchors = goalZoneController.DefineHorizontalBlueGoal();
            Assert.AreNotEqual(expectedAnchor1, anchors[0]);
            Assert.AreNotEqual(expectedAnchor2, anchors[1]);
            Assert.AreNotEqual(expectedAnchor3, anchors[2]);
            Assert.AreNotEqual(expectedAnchor4, anchors[3]);

            yield return null;
        }

        [UnityTest]
        public IEnumerator GoalZoneControllerDefineHorizontalRedGoalPasses()
        {
            CreateHorizontalField();

            Vector3 expectedAnchor1 = new Vector3(18f, 4f, 1f);
            Vector3 expectedAnchor2 = new Vector3(18f, 6f, 1f);
            Vector3 expectedAnchor3 = new Vector3(20f, 4f, 1f);
            Vector3 expectedAnchor4 = new Vector3(20f, 6f, 1f);
            var anchors = goalZoneController.DefineHorizontalRedGoal();
            Assert.AreEqual(expectedAnchor1, anchors[0]);
            Assert.AreEqual(expectedAnchor2, anchors[1]);
            Assert.AreEqual(expectedAnchor3, anchors[2]);
            Assert.AreEqual(expectedAnchor4, anchors[3]);

            yield return null;
        }

        [UnityTest]
        public IEnumerator GoalZoneControllerDefineHorizontalRedGoalFails()
        {
            CreateHorizontalField();

            Vector3 expectedAnchor1 = new Vector3(12f, 4f, 1f);
            Vector3 expectedAnchor2 = new Vector3(14f, 6f, 1f);
            Vector3 expectedAnchor3 = new Vector3(1f, 4f, 1f);
            Vector3 expectedAnchor4 = new Vector3(2f, 6f, 1f);
            var anchors = goalZoneController.DefineHorizontalRedGoal();
            Assert.AreNotEqual(expectedAnchor1, anchors[0]);
            Assert.AreNotEqual(expectedAnchor2, anchors[1]);
            Assert.AreNotEqual(expectedAnchor3, anchors[2]);
            Assert.AreNotEqual(expectedAnchor4, anchors[3]);

            yield return null;
        }
    }
}
