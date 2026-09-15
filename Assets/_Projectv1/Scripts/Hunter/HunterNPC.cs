using UnityEngine;
using FSM;
using Steering;
using Boids;
using Hunter.States;
using Environment;

namespace Hunter
{
    public class HunterNPC : SteeringAgent
    {
        [Header("Requerimientos de Consigna.")]
        [Tooltip("Tiempo mínimo entre ataques.")]
        public float TBA = 3f;
        [Tooltip("Distancia máxima para ataques a distancia.")]
        public float RangeAttackRadius = 8f;
        [Tooltip("Distancia máxima para ataques cuerpo a cuerpo.")]
        public float MeleeAttackRadius = 2f;

        [Header("Detección y Configuración.")]
        public float visionRadius = 10f;
        public LayerMask boidLayer;
        public WaypointPath path;
        public ObjectSpawner spawner;
    
        public FiniteStateMachine FSM { get; private set; }
        public HunterPatrolState PatrolState { get; private set; }
        public HunterAttackState AttackState { get; private set; }
        public HunterGatherState GatherState { get; private set; }

        public float TBATimer { get; set; }
        public BoidAgent CurrentTarget { get; set; }
        public BoidAgent DestroyedTarget { get; set; }

        protected void Awake()
        {
            FSM = new FiniteStateMachine();

            
            PatrolState = new HunterPatrolState(this);
            AttackState = new HunterAttackState(this);
            GatherState = new HunterGatherState(this);
        }

        private void Start()
        {
            TBATimer = 0f; 
            FSM.ChangeState(PatrolState);
        }

        protected override void Update()
        {
           
            if (TBATimer > 0)
            {
                TBATimer -= Time.deltaTime;
            }

            FSM.Update();
            base.Update();
        }
      
        public BoidAgent FindValidBoidInVision()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, visionRadius, boidLayer);
            foreach (var hit in hits)
            {
                if (hit.TryGetComponent<BoidAgent>(out var boid) && !boid.IsDestroyed)
                {
                    return boid;
                }
            }
            return null;
        }
    
        public BoidAgent FindDestroyedBoidInVision()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, visionRadius, boidLayer);
            foreach (var hit in hits)
            {
                if (hit.TryGetComponent<BoidAgent>(out var boid) && boid.IsDestroyed)
                {
                    return boid;
                }
            }
            return null;
        }
    }
}