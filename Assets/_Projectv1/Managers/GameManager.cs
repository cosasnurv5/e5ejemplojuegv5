using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Boids;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Configuración de Agentes (Flocking).")]
        public GameObject boidPrefab;
        [Tooltip("Cantidad mínima de Boids requeridos por la consigna.")]
        public int initialBoidCount = 10;
        public float respawnDelay = 3f;
        public Vector3 spawnAreaSize = new Vector3(20f, 0f, 20f);

        private readonly List<BoidAgent> boids = new List<BoidAgent>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            SpawnInitialBoids();
        }

        private void SpawnInitialBoids()
        {
            for (int i = 0; i < initialBoidCount; i++)
            {
                Vector3 randomPos = GetRandomSpawnPosition();
                GameObject obj = Instantiate(boidPrefab, randomPos, Quaternion.identity);
                if (obj.TryGetComponent<BoidAgent>(out var boid))
                {
                    boids.Add(boid);
                }
            }
        }

        public void CollectAndRespawnBoid(BoidAgent boid)
        {
            boid.gameObject.SetActive(false);
            StartCoroutine(RespawnRoutine(boid));
        }

        private IEnumerator RespawnRoutine(BoidAgent boid)
        {
            yield return new WaitForSeconds(respawnDelay);

            boid.transform.position = GetRandomSpawnPosition();
            boid.ResetAgent();
        }
         
        public Vector3 GetRandomSpawnPosition()
        {
            return transform.position + new Vector3(
                Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f),
                0f,
                Random.Range(-spawnAreaSize.z / 2f, spawnAreaSize.z / 2f)
            );
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position, spawnAreaSize);
        }
    }
}