using UnityEngine;

public class ResultSpawner : MonoBehaviour
{
    [SerializeField] private GameObject resultPrefab;
    [SerializeField] private Transform spawnRoot;
    [SerializeField] private float radius = 3.0f;

    void Start()
    {
        int count = ClearConfirmation.clearedCount;

        Debug.Log($"ResultScene 受け取ったクリア人数: {count}");

        if (count <= 0) return;

        float angleStep = 360f / count;

        for (int i = 0; i < count; i++)
        {
            float angle = angleStep * i * Mathf.Deg2Rad;

            Vector3 offset = new Vector3(
                Mathf.Cos(angle) * radius,
                0f,
                Mathf.Sin(angle) * radius
            );

            Vector3 pos = spawnRoot.position + offset;

            GameObject obj = Instantiate(
                resultPrefab,
                pos,
                Quaternion.identity,
                spawnRoot
            );

            // 中心（spawnRoot）を見るように回転させたい場合
            obj.transform.LookAt(spawnRoot);
        }
    }
}
