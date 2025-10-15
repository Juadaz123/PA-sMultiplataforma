    using System;
    using UnityEngine;
    
    public class SheepTriggerUI : MonoBehaviour
    {
        [SerializeField] private ScoreManager scoreManager;
        [SerializeField] private string objectTag = "Sheep";
        
        [SerializeField] private GameObject WinPanel;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(objectTag))
            {
                scoreManager.AddScore(1);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(objectTag))
            {
                scoreManager.SubtractScore(1);
            }
        }

        private void Start()
        {
            if (scoreManager == null)
            {
                Debug.LogError("Error: No se ha asignado una referencia al ScoreManager en el script ScoreManager.");
            }
        }

        private void Update()
        {
            if (scoreManager.GetScore() == 5)
            {
                
                WinPanel.SetActive(true);
            }
        }
    }
