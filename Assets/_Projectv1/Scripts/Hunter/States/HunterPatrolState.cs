using UnityEngine;
using FSM;
using Steering;

namespace Hunter.States
{
    public class HunterPatrolState : IState
    {
        private readonly HunterNPC hunter;
        private float spawnTimer = 0f;
        private const float SPAWN_INTERVAL = 5f;

        public HunterPatrolState(HunterNPC hunter)
        {
            this.hunter = hunter;
        }

        public void Enter() { }

        public void Update()
        {        
            var destroyedBoid = hunter.FindDestroyedBoidInVision();
            if (destroyedBoid != null)
            {
                hunter.DestroyedTarget = destroyedBoid;
                hunter.FSM.ChangeState(hunter.GatherState);
                return;
            }
        
            if (hunter.TBATimer <= 0f)
            {
                var validBoid = hunter.FindValidBoidInVision();
                if (validBoid != null)
                {
                    hunter.CurrentTarget = validBoid;
                    hunter.FSM.ChangeState(hunter.AttackState);
                    return;
                }
            }
         
            if (hunter.path != null)
            {
                Vector3 targetWaypoint = hunter.path.GetCurrentWaypoint();
                hunter.AddForce(SteeringBehaviors.Seek(hunter, targetWaypoint));

                if (Vector3.Distance(hunter.transform.position, targetWaypoint) < 1f)
                {
                    hunter.path.AdvanceToNext();
                }
            }
         
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= SPAWN_INTERVAL)
            {
                spawnTimer = 0f;
                if (hunter.spawner != null && hunter.spawner.CanSpawn())
                {
                    hunter.spawner.SpawnInterestObject(hunter.transform.position + hunter.transform.forward * 2f);
                }
            }
        }

        public void Exit() { }
    }
}