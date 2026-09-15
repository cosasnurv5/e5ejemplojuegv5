using UnityEngine;
using FSM;
using Steering;

namespace Hunter.States
{
    public class HunterAttackState : IState
    {
        private readonly HunterNPC hunter;

        public HunterAttackState(HunterNPC hunter)
        {
            this.hunter = hunter;
        }

        public void Enter() { }

        public void Update()
        {
            var target = hunter.CurrentTarget;

                     
            if (target == null || target.IsDestroyed || Vector3.Distance(hunter.transform.position, target.transform.position) > hunter.visionRadius)
            {
                hunter.CurrentTarget = null;
                hunter.FSM.ChangeState(hunter.PatrolState);
                return;
            }

            float distance = Vector3.Distance(hunter.transform.position, target.transform.position);

            if (distance <= hunter.MeleeAttackRadius)
            {
               
                ExeAttack(target, 100f); 
            }
            else if (distance <= hunter.RangeAttackRadius)
            {
                
                ExeAttack(target, 50f);
            }
            else
            {             
                hunter.AddForce(SteeringBehaviors.Seek(hunter, target.transform.position));
            }
        }

        private void ExeAttack(Boids.BoidAgent target, float damage)
        {
            target.TakeDamage(damage);
         
            hunter.TBATimer = hunter.TBA;
            hunter.CurrentTarget = null;
           
            hunter.FSM.ChangeState(hunter.PatrolState);
        }

        public void Exit() { }
    }
}