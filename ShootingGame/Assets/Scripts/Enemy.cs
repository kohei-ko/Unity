using UnityEngine;
using System.Linq;

public enum RESPAWN_TYPE
{
    UP, 
    RIGHT, 
    DOWN, 
    LEFT,
    SIZEOF,
}

public class Enemy : MonoBehaviour
{
    public Vector2 m_respawnPosInside;
    public Vector2 m_respawnPosOutside; 
    public float m_speed; 
    public int m_hpMax; 
    public int m_exp; 
    public int m_damage; 

    private int m_hp;
    private Vector3 m_direction;
    public Explosion m_explosionPrefab;
    public bool m_isFollow;
    public Gem[] m_gemPrefabs;
    public float m_gemSpeedMin; 
    public float m_gemSpeedMax;
    public AudioClip m_deathClip;

    private void Start()
    {
 
        m_hp = m_hpMax;
    }

 
    private void Update()
    {
        if ( m_isFollow )
        {
            var angle = Utils.GetAngle( 
                transform.localPosition, 
                Player.m_instance.transform.localPosition );
            var direction = Utils.GetDirection( angle );

            transform.localPosition += direction * m_speed;

            var angles = transform.localEulerAngles;
            angles.z = angle - 90;
            transform.localEulerAngles = angles;
            return;
        }
        transform.localPosition += m_direction * m_speed;
    }

    
    public void Init( RESPAWN_TYPE respawnType )
    {
        var pos = Vector3.zero;

        switch ( respawnType )
        {

            case RESPAWN_TYPE.UP:
                pos.x = Random.Range( 
                    -m_respawnPosInside.x, m_respawnPosInside.x );
                pos.y = m_respawnPosOutside.y;
                m_direction = Vector2.down;
                break;

            case RESPAWN_TYPE.RIGHT:
                pos.x = m_respawnPosOutside.x;
                pos.y = Random.Range( 
                    -m_respawnPosInside.y, m_respawnPosInside.y );
                m_direction = Vector2.left;
                break;

            case RESPAWN_TYPE.DOWN:
                pos.x = Random.Range( 
                    -m_respawnPosInside.x, m_respawnPosInside.x );
                pos.y = -m_respawnPosOutside.y;
                m_direction = Vector2.up;
                break;

            case RESPAWN_TYPE.LEFT:
                pos.x = -m_respawnPosOutside.x;
                pos.y = Random.Range( 
                    -m_respawnPosInside.y, m_respawnPosInside.y );
                m_direction = Vector2.right;
                break;
        }

        transform.localPosition = pos;
    }

    private void OnTriggerEnter2D( Collider2D collision )
    {
        if ( collision.name.Contains( "Shot" ) )
        {
            Instantiate( 
                m_explosionPrefab, 
                collision.transform.localPosition, 
                Quaternion.identity );
        }       


        if ( collision.name.Contains( "Player" ) )
        {
            var player = collision.GetComponent<Player>();
            player.Damage( m_damage );
            return;
        }

        if ( collision.name.Contains( "Shot" ) )
        {
            Destroy( collision.gameObject );

            m_hp--;

            if ( 0 < m_hp ) return;
            var audioSource = FindObjectOfType<AudioSource>();
            audioSource.PlayOneShot( m_deathClip );

            Destroy( gameObject );

            var exp = m_exp;
            while ( 0 < exp )
            {
                var gemPrefabs = m_gemPrefabs.Where( c => c.m_exp <= exp ).ToArray();

                var gemPrefab = gemPrefabs[ Random.Range( 0, gemPrefabs.Length ) ];

                var gem = Instantiate( 
                    gemPrefab, transform.localPosition, Quaternion.identity );

                gem.Init( m_exp, m_gemSpeedMin, m_gemSpeedMax );

                exp -= gem.m_exp;
            }
        }
    }
}