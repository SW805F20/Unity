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
        private GoalZoneController testo;

        [SetUp]
        public void Setup()
        {
            var controllerObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/GoalZoneController"));
            goalZoneController = controllerObject.GetComponent<GoalZoneController>();
            var playingFieldObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/PlayingField"));
            playingField = playingFieldObject.GetComponent<FieldGenerator>();

            var newObject = new GameObject();
            testo = newObject.AddComponent<GoalZoneController>();
            
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(goalZoneController.gameObject);
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
        public IEnumerator GoalZoneControllerSpawnsBlueGoalVertical()
        {
            CreateVerticalField();


            goalZoneController.SpawnVerticalGoals();
            Vector2 properPosition = new Vector2(5, 19);
            //Assert.AreNotEqual(properPosition, goalZoneController.centerOfBlueGoal);
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
        public IEnumerator GoalZoneControllerSpawnMirroringZonesVerticalPasses()
        {
            CreateVerticalField();
            goalZoneController.SpawnMirroringGoals(2f, 4f);
            Assert.True(goalZoneController.centerOfRedGoal == new Vector2(8f, 16f)); 

            
            yield return null;
        }



    }
}
