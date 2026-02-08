using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Health playerHealth;
    [SerializeField] private MonoBehaviour[] disableOnDeath; // PlayerController, Gun, etc.
    [SerializeField] private GameOverUI gameOverUI;
    [SerializeField] private WaveManager waveManager;

    private bool isGameOver;


    private void Awake()
    {
        if (playerHealth == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) playerHealth = player.GetComponent<Health>();
        }
    }

    private void Start()
    {
        if (playerHealth != null)
            playerHealth.onDied.AddListener(HandlePlayerDied);

        if (gameOverUI != null)
            gameOverUI.Hide();

        LockCursor(true);
    }

    private void Update()
    {
        if (!isGameOver) return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void HandlePlayerDied()
    {
        isGameOver = true;

        // Disable gameplay scripts
        foreach (var b in disableOnDeath)
            if (b != null) b.enabled = false;
        
        waveManager.enabled = false;

        // Show UI
        if (gameOverUI != null)
            gameOverUI.Show();

        // Unlock cursor so player can breathe
        LockCursor(false);
    }

    private void LockCursor(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }
}

