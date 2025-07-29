using UnityEngine;

public class UniqueId : MonoBehaviour
{
    [SerializeField] private string UUID;

    public static string GenerateUniqueID(int min = 11, int max = 17)
    {
        int length = Random.Range(min, max);
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        string unique_id = "";
        for (int i = 0; i < length; i++)
        {
            unique_id += chars[Random.Range(0, chars.Length - 1)];
        }
        return unique_id;
    }

    public void GenerateUIDEditor()
    {
        UUID = GenerateUniqueID();
    }

    protected virtual void Start()
    {
        if (GameManager.instance.collectedItems.Contains(UUID)) Destroy(gameObject);
    }
}
