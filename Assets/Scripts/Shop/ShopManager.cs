using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private GameObject shopPanel; // the whole panel root

    [Header("Controls")]
    [SerializeField] private KeyCode toggleKey = KeyCode.E;

    public bool IsOpen { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Start()
    {
        SetOpen(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            SetOpen(!IsOpen);
        }
    }

    public void SetOpen(bool open)
    {
        IsOpen = open;

        if (shopPanel != null)
            shopPanel.SetActive(open);

        // Pause/unpause game
        Time.timeScale = open ? 0f : 1f;

        // Cursor
        Cursor.visible = open;
        Cursor.lockState = open ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
