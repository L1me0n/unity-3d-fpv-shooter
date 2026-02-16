using UnityEngine;

public class ShopCloseButton : MonoBehaviour
{
    public void Close()
    {
        if (ShopManager.Instance != null)
            ShopManager.Instance.SetOpen(false);
    }
}
