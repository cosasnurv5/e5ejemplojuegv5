using UnityEngine;

namespace Steering
{
    public class SteeringAgent : MonoBehaviour
    {
        [Header("Configuración de Movimiento")]
        public float maxSpeed = 5f;
        public float maxForce = 10f;
        public float mass = 1f;

        public Vector3 Velocity { get; protected set; }
        public Vector3 SteeringForce { get; private set; }

        protected virtual void Update()
        {
          
            SteeringForce = Vector3.ClampMagnitude(SteeringForce, maxForce);
          
            Vector3 acceleration = SteeringForce / mass;
     
            Velocity = Vector3.ClampMagnitude(Velocity + acceleration * Time.deltaTime, maxSpeed);
            transform.position += Velocity * Time.deltaTime;
         
            if (Velocity.sqrMagnitude > 0.001f)
            {
                transform.forward = Velocity.normalized;
            }
          
            SteeringForce = Vector3.zero;
        }

        public void AddForce(Vector3 force)
        {
            SteeringForce += force;
        }
    }
}