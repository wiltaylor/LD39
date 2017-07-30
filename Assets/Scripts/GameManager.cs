using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        public bool DemoMode;

        public int Level;

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
        }

        public void NextLevel()
        {
            LoadLevel(Level + 1);
        }

        public void ReloadLevel()
        {
            LoadLevel(Level);
        }

        public void LoadLevel(int level)
        {
            Level = level;
            switch (level)
            {
                case 1:
                    SceneManager.LoadScene("L1KillPit");
                    break;
                default:
                    LoadCredits();
                    break;
            }
        }

        public void LoadCredits()
        {
            Level = -1;
            SceneManager.LoadScene("Credits");
        }

        public void LoadMenu()
        {
            Level = 0;
            SceneManager.LoadScene("Menu");
        }
    }
}
