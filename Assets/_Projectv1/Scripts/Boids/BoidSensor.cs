using System.Collections.Generic;
using UnityEngine;

namespace Boids
{
    public class BoidSensor : MonoBehaviour
    {
        [Header("Capas de Detección.")]
        public LayerMask boidLayer;
        public LayerMask hunterLayer;
        public LayerMask interestObjectLayer;

        
        public List<BoidAgent> GetNeighbors(float radius)
        {
            List<BoidAgent> neighbors = new List<BoidAgent>();
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius, boidLayer);

            foreach (var col in colliders)
            {
                if (col.gameObject != gameObject && col.TryGetComponent<BoidAgent>(out var agent))
                {
                    if (!agent.IsDestroyed)
                    {
                        neighbors.Add(agent);
                    }
                }
            }
            return neighbors;
        }

        
        public Transform GetNearestHunter(float radius)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius, hunterLayer);
            if (colliders.Length > 0)
            {
                return colliders[0].transform;
            }
            return null;
        }

        
        public Transform GetNearestInterestObject(float radius)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius, interestObjectLayer);
            Transform nearest = null;
            float minDistance = float.MaxValue;

            foreach (var col in colliders)
            {
                float dist = Vector3.Distance(transform.position, col.transform.position);
                if (dist < minDistance)
                {
                    minDistance = dist;
                    nearest = col.transform;
                }
            }
            return nearest;
        }
    }
}