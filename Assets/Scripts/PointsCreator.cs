using UnityEngine;

public class PointsCreator : MonoBehaviour
{
    [SerializeField] private GameObject _pointPrefab;

    private void Start()
    {
        // Получаем все дочерние объекты, содержащие "aps*" в имени
        BoxCollider[] colliders = GetComponentsInChildren<BoxCollider>();
        foreach (var collider in colliders)
        {
            if (collider.gameObject.transform.parent.name.Contains("aps"))
                CreatePointsInsideCollider(collider);
        }
    }

    private Vector3 Inverse(Vector3 vector) // Возводим каждый элемент в -1 степень
    {
        return new Vector3(1f / vector.x, 1f / vector.y, 1f / vector.z);
    }

    private Vector3 Multiply(Vector3 v1, Vector3 v2) // Перемножаем каждый элемент с соответсвующим ему
    {
        float x = v1.x * v2.x;
        float y = v1.y * v2.y;
        float z = v1.z * v2.z;

        return new Vector3(x, y, z);
    }

    private float CalculateStep(float x) // Рассчитываем шаг
    {
        return (int)x == 1 ? 0 : x / (x - 1);
    }

    private void CreatePointsInsideCollider(BoxCollider collider)
    {
        Bounds bounds = collider.bounds;
        Vector3 center = bounds.center; // Центр коллайдера
        Vector3 size = bounds.size; // Размеры коллайдера
        size = Multiply(size, Inverse(collider.transform.localScale));
        Vector3 step = new Vector3(CalculateStep(size.x),
            CalculateStep(size.y),
            CalculateStep(size.z)); // шаг

        // Проходимся по всем точкам внутри коллайдера и создаем объекты в этих точках
        for (int x = 0; x < (int)size.x; x++)
        {
            for (int y = 0; y < (int)size.y; y++)
            {
                for (int z = 0; z < (int)size.z; z++)
                {
                    // Вычисляем позицию точки
                    Vector3 position = new Vector3(
                        center.x - size.x / 2f + x * step.x,
                        center.y - size.y / 2f + y * step.y,
                        center.z - size.z / 2f + z * step.z
                    );
                    // Создаем объект точки
                    GameObject point = Instantiate(_pointPrefab, collider.transform);
                    point.transform.localPosition = position;
                    point.name = "Point";
                }
            }
        }
    }
}