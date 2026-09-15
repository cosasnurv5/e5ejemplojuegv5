using UnityEngine;
using FSM;
using Steering;
using Managers;

namespace Hunter.States
{
    public class HunterGatherState : IState
    {
        private readonly HunterNPC hunter;
        private float gatherTimer = 0f;
        private const float GATHER_DURATION = 2f; 

        public HunterGatherState(HunterNPC hunter)
        {
            this.hunter = hunter;
        }

        public void Enter()
        {
            gatherTimer = 0f;
        }

        public void Update()
        {
            var target = hunter.DestroyedTarget;

            if (target == null || !target.IsDestroyed)
            {
                hunter.DestroyedTarget = null;
                hunter.FSM.ChangeState(hunter.PatrolState);
                return;
            }

            float distance = Vector3.Distance(hunter.transform.position, target.transform.position);

            if (distance > 1.2f)
            {
                hunter.AddForce(SteeringBehaviors.Arrive(hunter, target.transform.position, 2f));
            }
            else
            {
                gatherTimer += Time.deltaTime;
                if (gatherTimer >= GATHER_DURATION)
                {                 
                    GameManager.Instance.CollectAndRespawnBoid(target);
                    hunter.DestroyedTarget = null;
                    hunter.FSM.ChangeState(hunter.PatrolState);
                }
            }
        }

        public void Exit() { }
    }
}