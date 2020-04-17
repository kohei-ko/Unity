using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public Enemy[] m_enemyPrefabs;
    public float m_intervalFrom;
    public float m_intervalTo; 
    public float m_elapsedTimeMax;
    public float m_elapsedTime;
    private float m_timer;

    private void Update()
    {
        m_elapsedTime += Time.deltaTime;

        m_timer += Time.deltaTime;

        var t = m_elapsedTime / m_elapsedTimeMax;
        var interval = Mathf.Lerp( m_intervalFrom, m_intervalTo, t );

        if ( m_timer < interval ) return;

        m_timer = 0;
        var enemyIndex = Random.Range( 0, m_enemyPrefabs.Length );

        var enemyPrefab = m_enemyPrefabs[ enemyIndex ];

        var enemy = Instantiate( enemyPrefab );

        var respawnType = ( RESPAWN_TYPE )Random.Range( 
            0, ( int )RESPAWN_TYPE.SIZEOF );

        enemy.Init( respawnType );
    }
}