using NUnit.Framework;
using Tests.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Tests.EditMode
{
    public class MissionsTests
    {
        void TestForMission(MissionBase mission)
        {
            var go = new GameObject(nameof(MissionEntry));
            var missionEntry = go.AddComponent<MissionEntry>();
            Assert.NotNull(missionEntry);
            missionEntry.descText = new GameObject("descText").AddComponent<Text>();
            missionEntry.rewardText = new GameObject("rewardText").AddComponent<Text>();
            missionEntry.progressText = new GameObject("progressText").AddComponent<Text>();
            missionEntry.claimButton = go.AddComponent<Button>();
            missionEntry.background = go.AddComponent<Image>();
            var mockMissionUi = TestableMonobehaviourFactory.Create<MissionUI>();
            missionEntry.FillWithMission(mission, mockMissionUi);
        }

        [Test]
        public void MissionSingleRunMission()
        {
            var mission = new SingleRunMission {
                reward = 0,
                max = 1
            };
            Assert.NotNull(mission);
            TestForMission(mission);
        }

        [Test]
        public void MissionPickupMission()
        {
            var mission = new PickupMission {
                reward = 0,
                max = 1
            };
            Assert.NotNull(mission);
            TestForMission(mission);
        }

        [Test]
        public void MissionBarrierJumpMission()
        {
            var mission = new BarrierJumpMission {
                reward = 0,
                max = 1
            };
            Assert.NotNull(mission);
            TestForMission(mission);
        }

        [Test]
        public void MissionSlidingMission()
        {
            var mission = new SlidingMission {
                reward = 0,
                max = 1
            };
            Assert.NotNull(mission);
            TestForMission(mission);
        }

        [Test]
        public void MissionMultiplierMission()
        {
            var mission = new MultiplierMission {
                reward = 0,
                max = 1
            };
            Assert.NotNull(mission);
            TestForMission(mission);
        }

    }

}