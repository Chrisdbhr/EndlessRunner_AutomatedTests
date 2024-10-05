using System;
using System.Collections;
using Tests.Utils;
using UnityEngine;

namespace Tests.PlayMode.Mocks
{
    public class MockObstacle : AllLaneObstacle
    {
        void Awake()
        {
            gameObject.AddComponent<BoxCollider>().isTrigger = true;
            gameObject.layer = (int)typeof(CharacterCollider).ReflectionGetConstFieldValue("k_ObstacleLayerIndex");
            CreateRedCube();
        }

        public override void Impacted()
        {
            Debug.Log(this.name + " impacted");
        }

        void CreateRedCube()
        {
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.parent = this.transform;
            cube.transform.position = new Vector3(0, 0, 0);
            cube.GetComponent<Renderer>().material = TestUtils.GetNewTestMaterial();
        }

        public override IEnumerator Spawn(TrackSegment segment,float t)
        {
            Vector3 position;
            Quaternion rotation;
            segment.GetPointAt(t, out position, out rotation);
            transform.SetPositionAndRotation(position, rotation);
            transform.SetParent(segment.objectRoot, true);
            yield break;
        }
    }
}