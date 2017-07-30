using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        public bool DemoMode;
        public Texture2D Crosshair;

        public int Level;

        public bool QuitYesNoActive;

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

        void Update()
        {
            if (Input.GetButton("Quit"))
            {
                if(Level == 0)
                    Application.Quit();

                if (Level == -1)
                {
                    LoadMenu();
                    return;
                }

                QuitYesNoActive = true;
            }

            if (QuitYesNoActive && Input.GetButton("QuitYes"))
            {
                Application.Quit();
            }

            if (QuitYesNoActive && Input.GetButton("QuitNo"))
            {
                QuitYesNoActive = false;
            }
            
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
                case 2:
                    SceneManager.LoadScene("L2Bridges");
                    break;
                case 3:
                //    SceneManager.LoadScene("L3TheIsland");
                //    break;
                //case 4:
                    SceneManager.LoadScene("L4LotsOfBridges");
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

        public void EnableTargetingView()
        {
            Cursor.SetCursor(Crosshair, new Vector2(0.32f, 0.32f), CursorMode.Auto);
        }

        public void DisableTargetingView()
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

        }
    }
}
