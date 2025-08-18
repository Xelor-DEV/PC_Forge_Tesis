using UnityEngine;

public class CubeSpawnZone : MonoBehaviour
{
    [Header("Gizmo Settings")]
    public Color gizmoWireColor = Color.blue;
    [SerializeField] private Color gizmoFillColor = new Color(0, 0.5f, 1f, 0.3f);

    public void SpawnObject(GameObject prefab)
    {
        if (prefab != null)
        {
            // Generar posición aleatoria en espacio local
            Vector3 localPosition = new Vector3(
                Random.Range(-0.5f, 0.5f),
                Random.Range(-0.5f, 0.5f),
                Random.Range(-0.5f, 0.5f)
            );

            // Convertir a posición mundial considerando escala, rotación y posición
            Vector3 worldPosition = transform.TransformPoint(localPosition);

            Instantiate(prefab, worldPosition, prefab.transform.rotation);
        }
    }

    private void OnDrawGizmos()
    {
        Matrix4x4 originalMatrix = Gizmos.matrix;
        Color originalColor = Gizmos.color;

        // Configurar matriz para incluir posición, rotación y escala
        Gizmos.matrix = transform.localToWorldMatrix;

        // Dibujar cubo sólido translúcido
        Gizmos.color = gizmoFillColor;
        Gizmos.DrawCube(Vector3.zero, Vector3.one);

        // Dibujar wireframe
        Gizmos.color = gizmoWireColor;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);

        // Restaurar configuración original
        Gizmos.matrix = originalMatrix;
        Gizmos.color = originalColor;
    }
}