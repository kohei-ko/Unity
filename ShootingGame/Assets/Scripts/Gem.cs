using UnityEngine;

public class Gem : MonoBehaviour
{
    public int m_exp; 
    public float m_brake = 0.9f; 

    private Vector3 m_direction; 
    private float m_speed; 
    public float m_followAccel = 0.01f;

    private bool m_isFollow;
    private float m_followSpeed;
    public AudioClip m_goldClip;

    
    private void Update()
    {
        var playerPos = Player.m_instance.transform.localPosition;

        var distance = Vector3.Distance( playerPos, transform.localPosition );

        if ( distance < Player.m_instance.m_magnetDistance )
        {
            m_isFollow = true;
        }

        if ( m_isFollow && Player.m_instance.gameObject.activeSelf )
        {
            var direction = playerPos - transform.localPosition;
            direction.Normalize();

            transform.localPosition += direction * m_followSpeed;

            m_followSpeed += m_followAccel;
            return;
        }
        var velocity = m_direction * m_speed;

        transform.localPosition += velocity;

        m_speed *= m_brake;

        transform.localPosition = 
            Utils.ClampPosition( transform.localPosition );
    }

    public void Init( int score, float speedMin, float speedMax )
    {
        var angle = Random.Range( 0, 360 );

        var f = angle * Mathf.Deg2Rad;

        m_direction = new Vector3( Mathf.Cos( f ), Mathf.Sin( f ), 0 );

        m_speed = Mathf.Lerp( speedMin, speedMax, Random.value );

        Destroy( gameObject, 8 );
    }

    private void OnTriggerEnter2D( Collider2D collision )
    {
        if ( !collision.name.Contains( "Player" ) ) return;

        Destroy( gameObject );
        
        var player = collision.GetComponent<Player>();
        player.AddExp( m_exp );

        var audioSource = FindObjectOfType<AudioSource>();
        audioSource.PlayOneShot( m_goldClip );
    }
}