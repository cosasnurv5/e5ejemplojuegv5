using UnityEngine;
using UnityEngine.UI;
using Hunter;

namespace UI_Debug
{
    public class StateWorldUI : MonoBehaviour
    {
        [Header("Referencias.")]
        public HunterNPC hunter;
        public Text stateText; 
        public Vector3 offset = new Vector3(0, 2.5f, 0);

        private Camera mainCamera;

        private void Start()
        {
            mainCamera = Camera.main;
            if (hunter == null)
            {
                hunter = GetComponentInParent<HunterNPC>();
            }
        }
        private void LateUpdate()
        {
            if (hunter == null || stateText == null) return;

            
            transform.position = hunter.transform.position + offset;

         
            if (mainCamera != null)
            {
                transform.rotation = mainCamera.transform.rotation;
            }

            string currentStateName = "Sin Estado";
            if (hunter.FSM != null && hunter.FSM.CurrentState != null)
            {
                currentStateName = hunter.FSM.CurrentState.GetType().Name.Replace("Hunter", "").Replace("State", "");
            }

            string targetInfo = hunter.CurrentTarget != null ? hunter.CurrentTarget.name : "Ninguno";
            string tbaStatus = hunter.TBATimer <= 0 ? "LISTO" : $"{hunter.TBATimer:F1}s";

           
            stateText.text = $"Estado: {currentStateName}\n" +
                             $"Objetivo: {targetInfo}\n" +
                             $"TBA: {tbaStatus}";
        }
    }
}