using UnityEngine;
using System;
using Random = System.Random;

[Serializable]
[CreateAssetMenu(fileName = "Enemy Spawn Rule", menuName = "Custom/Procedural Generation/Enemy Spawn Rule")]
public class EnemySpawnDecoratorRule : BaseDecoratorRule
{
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] int minEnemies = 1;
    [SerializeField] int maxEnemies = 3;

    internal override bool CanBeApplied(TileType[,] levelDecorated, Room room)
    {
        return room.Area.width > 3 && room.Area.height > 3 && enemyPrefabs.Length > 0;
    }

    internal override void Apply(TileType[,] levelDecorated, Room room, Transform parent)
    {
        Random random = SharedLevelData.Instance.Rand;
        int count = random.Next(minEnemies, maxEnemies + 1);

        Transform enemyContainer = parent.Find("Enemies");
        if (enemyContainer == null)
        {
            GameObject go = new GameObject("Enemies");
            enemyContainer = go.transform;
            enemyContainer.SetParent(parent, false);
        }

        for (int i = 0; i < count; i++)
        {
            int x = random.Next(room.Area.xMin + 1, room.Area.xMax - 1);
            int y = random.Next(room.Area.yMin + 1, room.Area.yMax - 1);

            if (levelDecorated[x, y] != TileType.Floor)
                continue;

            int scale = SharedLevelData.Instance.Scale;
            Vector3 worldPos = new Vector3(x, 0, y) * scale;

            GameObject prefab = enemyPrefabs[random.Next(enemyPrefabs.Length)];

            GameObject enemy = Instantiate(prefab, worldPos, Quaternion.identity, enemyContainer);
            enemy.name = $"Enemy_{prefab.name}_{x}_{y}";

        }
    }
}
