using UnityEngine;
using Hunter;
using Boids;

namespace UI_Debug
{
    public class DebugVisualizer : MonoBehaviour
    {
        [Header("Referencias.")]
        public HunterNPC hunter;
        public BoidAgent[] boids;

        [Header("Opciones de Gizmos.")]
        public bool showHunterRanges = true;
        public bool showBoidRanges = true;
        public bool showTargetLines = true;

        private void OnDrawGizmos()
        {
           
            if (showHunterRanges && hunter != null)
            {
                
                Gizmos.color = new Color(1f, 1f, 0f, 0.3f);
                Gizmos.DrawWireSphere(hunter.transform.position, hunter.visionRadius);

              
                Gizmos.color = new Color(1f, 0.5f, 0f, 0.5f);
                Gizmos.DrawWireSphere(hunter.transform.position, hunter.RangeAttackRadius);

         
                Gizmos.color = new Color(1f, 0f, 0f, 0.8f);
                Gizmos.DrawWireSphere(hunter.transform.position, hunter.MeleeAttackRadius);

             
                if (showTargetLines)
                {
                    if (hunter.CurrentTarget != null)
                    {
                        Gizmos.color = Color.red;
                        Gizmos.DrawLine(hunter.transform.position, hunter.CurrentTarget.transform.position);
                    }
                    else if (hunter.DestroyedTarget != null)
                    {
                        Gizmos.color = Color.magenta;
                        Gizmos.DrawLine(hunter.transform.position, hunter.DestroyedTarget.transform.position);
                    }
                }
            }

           
            if (showBoidRanges && boids != null)
            {
                foreach (var boid in boids)
                {
                    if (boid == null || boid.IsDestroyed) continue;
                  
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawWireSphere(boid.transform.position, boid.separationRadius);
               
                    Gizmos.color = Color.blue;
                    Gizmos.DrawWireSphere(boid.transform.position, boid.neighborRadius);
                }
            }
        }
    }
}