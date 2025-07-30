using UnityEngine;

public class ItemProxy : MonoBehaviour
{
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Transform proxy;
    public static ItemProxy instance;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void Update()
    {
        proxy.rotation = Quaternion.Euler(0, Time.time * 10, 0);
    }

    public void ChangeMesh(Mesh _mesh, Material _material)
    {
        meshFilter.mesh = _mesh;
        meshRenderer.material = _material;
    }
}
