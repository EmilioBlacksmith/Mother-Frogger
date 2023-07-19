using Character_System.HP_System;
using Character_System.Physics;
using Game_Manager.Goal_Spots_System;
using Game_Manager.Score_System;
using Game_Manager.Score_System.Leaderboard;
using Game_Manager.Timer_System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Game_Manager
{
    [RequireComponent(typeof(TimerManager), typeof(GoalSpotsManager), typeof(ScoreSystem))]
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        private int _difficultyLever = 1;
        
        [SerializeField] private int currentLevelIndex;

        [Header("Player Parenting")] 
        [SerializeField] private Transform playerParent;
        [SerializeField] private Transform playerHip;

        [Header("Player Score Submit")] 
        [SerializeField] private TMP_InputField playerName;
        [SerializeField] private TextMeshProUGUI playerScoreUI;
        
        public int DifficultyLevel() => _difficultyLever;
        
        // Manager Subsystems
        public ScoreSystem ScoreSystem { get; private set; }
        public TimerManager TimerManager { get; private set; }
        public GoalSpotsManager GoalSpotsManager { get; private set; }
        public LeaderBoard Leaderboard { get; private set; }

        private void Start()
        {
            TimerManager = GetComponent<TimerManager>();
            ScoreSystem = GetComponent<ScoreSystem>();
            GoalSpotsManager = GetComponent<GoalSpotsManager>();
            Leaderboard = GetComponent<LeaderBoard>();
        }

        public void RestartParenting() => playerHip.SetParent(playerParent, true);

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        public void GoalSpotCrossed()
        {
            ScoreSystem.FrogCrossed();
            HealthSystem.Instance.NextLevel();
            RestartParenting();
        }

        public void NextDifficulty()
        {
            HealthSystem.Instance.NextLevel();
            _difficultyLever++;
            _difficultyLever = Mathf.Clamp(_difficultyLever, 1, 5);
            ScoreSystem.AllGoalSpotsCrossed();
            RestartParenting();
        }

        public void GameOver()
        {
            CrashController.Instance.CrashedJoints();
            CrashController.Instance.hasCrash = true;
            playerScoreUI.text = ScoreSystem.Score().ToString();
            ScoreSystem.ShowGameOverUI();
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }

        public void SubmitPlayerScore()
        {
            Leaderboard.SetLeaderboardEntry(playerName.text, ScoreSystem.Score());
        }

        public void Restart()
        {
            SceneManager.LoadSceneAsync(currentLevelIndex,LoadSceneMode.Single);
        }
    }
}
