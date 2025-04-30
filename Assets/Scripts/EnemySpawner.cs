using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Enemy[] Enemies;
    public float[] InitialSpawnDelayInSeconds;

    float timer;
    bool _enemy_spawned = true;
    int enemyIndex;
    bool _should_timer_run = false;
    void Start()
    {
        timer = 0f;
        enemyIndex = 0;
    }



    // Update is called once per frame
    void Update()
    {

        if (enemyIndex >= Enemies.Length)
        {
            return;
        }

        if (_enemy_spawned)
        {
            _should_timer_run = true;
            _enemy_spawned = false;
        }

        if (!_enemy_spawned && _should_timer_run)
        {
            if (timer < InitialSpawnDelayInSeconds[enemyIndex])
            {
                timer += Time.deltaTime;
            }

            if (timer >= InitialSpawnDelayInSeconds[enemyIndex])
            {
                timer = 0;
                _should_timer_run = false;
                SpawnEnemy();
            }
        }


    }

    public void SpawnEnemy()
    {
        Enemies[enemyIndex].MoveToRandomLocationOutsidePlayerView();
        _enemy_spawned = true;
        enemyIndex++;
        
    }

    public void SharkEvent()
    {

        foreach (Enemy enemy in Enemies) {
            enemy.ShouldPermaSink=true;
            enemy.Sink(Vector3.one*3000f);
        }
    }

}