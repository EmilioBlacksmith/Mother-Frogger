using Audio;
using Character_System.Physics;
using Game_Manager;
using Game_Manager.Timer_System;
using Particles;
using UnityEngine;

namespace Character_System.HP_System
{
    public class HealthSystem : MonoBehaviour
    {
        [SerializeField] private int currentHealthPoints;
        [SerializeField] private int startingHealthPoints = 3;
        [SerializeField] private Transform startingPosition;
        [SerializeField] private GameObject player;

        public static HealthSystem Instance { get; private set; }
        public bool IsGameOver { get; private set; } = false;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this);
            else
                Instance = this;
        }

        public void Start()
        {
            currentHealthPoints = startingHealthPoints;
            player.transform.position = startingPosition.position;
            IsGameOver = false;
            HUDSystem.Instance?.AllocateHealthPoints(currentHealthPoints);
        } 
        
        public int CurrentHealthPoints => currentHealthPoints;

        public void SubtractHealthPoint()
        {
            AudioSystem.Instance.PlaySoundEffect(AudioSystem.SoundEffect.Error);
            currentHealthPoints--;
            if (currentHealthPoints <= 0 && !IsGameOver)
            {
                IsGameOver = true;
                GameManager.Instance.GameOver();
            }else
            { 
                player.transform.position = startingPosition.position;
                CrashController.Instance.RestartCrashPoints();
                GameManager.Instance.TimerManager.RestartTimer();
                HUDSystem.Instance.UpdateHealthPoints(currentHealthPoints);
                ParticleSpawningSystem.Instance.SpawnSpawningParticle(player.transform);
            }
        }

        public void AddHealthPoint()
        {
            if(currentHealthPoints < startingHealthPoints)
            {
                currentHealthPoints++;
                HUDSystem.Instance.UpdateHealthPoints(currentHealthPoints);
            }
        }

        public void NextLevel()
        {
            player.transform.position = startingPosition.position;
            //AddHealthPoint();
            CrashController.Instance.RestartCrashPoints();
            GameManager.Instance.TimerManager.RestartTimer();
            ParticleSpawningSystem.Instance.SpawnSpawningParticle(player.transform);
        }
    }
}
