using UnityEngine;


public class Player : MonoBehaviour
{
    public float m_speed;
    public Shot m_shotPrefab;
    public float m_shotSpeed;
    public float m_shotAngleRange;
    public float m_shotTimer;
    public int m_shotCount;
    public float m_shotInterval;
    public int m_hpMax;
    public int m_hp;
    public static Player m_instance;
    public float m_magnetDistance;
    public int m_nextExpBase;
    public int m_nextExpInterval;
    public int m_level;
    public int m_exp;
    public int m_prevNeedExp; 
    public int m_needExp;
    public AudioClip m_levelUpClip;
    public AudioClip m_damageClip;
    public int m_levelMax; 
    public int m_shotCountFrom; 
    public int m_shotCountTo; 
    public float m_shotIntervalFrom; 
    public float m_shotIntervalTo;
    public float m_magnetDistanceFrom;
    public float m_magnetDistanceTo;

    public int wazacount;

    // public int m_waza;
    // public int m_needWaza;

    //新武器

    //新武器
    
    




    private void Awake()
    {
        m_instance = this;
        m_hp = m_hpMax;
        m_level = 1;
        m_needExp = GetNeedExp( 1 );
        m_shotCount = m_shotCountFrom;
        m_shotInterval = m_shotIntervalFrom;
        m_magnetDistance = m_magnetDistanceFrom;

        // m_needWaza = 200;
    }
    private void Update()
    {

        var h = Input.GetAxis( "Horizontal" );
        var v = Input.GetAxis( "Vertical" );

        var velocity = new Vector3( h, v ) * m_speed;
        transform.localPosition += velocity;

        transform.localPosition = Utils.ClampPosition( transform.localPosition );

        var screenPos = Camera.main.WorldToScreenPoint( transform.position );

        var direction = Input.mousePosition - screenPos;

    
        var angle = Utils.GetAngle( Vector3.zero, direction );

    
        var angles = transform.localEulerAngles;
        angles.z = angle - 90;
        transform.localEulerAngles = angles;

        m_shotTimer += Time.deltaTime;

        if ( m_shotTimer < m_shotInterval ) return;

        m_shotTimer = 0;

        ShootNWay( angle, m_shotAngleRange, m_shotSpeed, m_shotCount );

        //技一覧！！

        if(wazacount == 1　&& Input.GetKey(KeyCode.Space)){
            wazacount=0;
            m_exp=0;

            if(m_shotAngleRange<60){
                m_shotAngleRange+=10;
            }

            m_level++;

            m_prevNeedExp = m_needExp;

            m_needExp = GetNeedExp( m_level );


                var angleBase = 0;
            var angleRange = 360;
            var count = 28;
            ShootNWay( angleBase, angleRange, 0.15f, count );
            ShootNWay( angleBase, angleRange, 0.2f, count );
            ShootNWay( angleBase, angleRange, 0.25f, count );
            ShootNWay( angleBase, angleRange, 0.35f, count );
            ShootNWay( angleBase, angleRange, 0.45f, count );
            ShootNWay( angleBase, angleRange, 0.55f, count );
            ShootNWay( angleBase, angleRange, 0.65f, count );
            ShootNWay( angleBase, angleRange, 0.75f, count );
            ShootNWay( angleBase, angleRange, 0.85f, count );
            ShootNWay( angleBase, angleRange, 0.95f, count );
            ShootNWay( angleBase, angleRange, 1.05f, count );


            var audioSource = FindObjectOfType<AudioSource>();
            audioSource.PlayOneShot( m_levelUpClip );

            var t = ( float )( m_level - 1 ) / ( m_levelMax - 1 );
            m_shotCount = Mathf.RoundToInt( 
                Mathf.Lerp( m_shotCountFrom, m_shotCountTo, t ) );
            m_shotInterval = Mathf.Lerp( 
                m_shotIntervalFrom, m_shotIntervalTo, t );
            m_magnetDistance = Mathf.Lerp( 
                m_magnetDistanceFrom, m_magnetDistanceTo, t );
        }
        if(wazacount == 1　&& Input.GetKey(KeyCode.Q)){
            wazacount=0;
            m_exp=0;

            if(m_shotAngleRange<60){
                m_shotAngleRange+=10;
            }

            m_level++;

            m_prevNeedExp = m_needExp;

            m_needExp = GetNeedExp( m_level );


            //var angleBase = 0;
            var angleRange = 20;
            var count = 5;

                for(float i=0.05f; i<2.0f; i+=0.20f){
                    ShootNWay( 0, angleRange, i, count );
                    ShootNWay( 90, angleRange, i, count );
                    ShootNWay( 180, angleRange, i, count );
                    ShootNWay( 270, angleRange, i, count );
                }

            

                var audioSource = FindObjectOfType<AudioSource>();
                audioSource.PlayOneShot( m_levelUpClip );

                var t = ( float )( m_level - 1 ) / ( m_levelMax - 1 );
                m_shotCount = Mathf.RoundToInt( 
                    Mathf.Lerp( m_shotCountFrom, m_shotCountTo, t ) );
                m_shotInterval = Mathf.Lerp( 
                    m_shotIntervalFrom, m_shotIntervalTo, t );
                m_magnetDistance = Mathf.Lerp( 
                    m_magnetDistanceFrom, m_magnetDistanceTo, t );
            
        }


    }
    private void ShootNWay( float angleBase, float angleRange, float speed, int count )
    {
        var pos = transform.localPosition;
        var rot = transform.localRotation;

        if ( 1 < count )
        {
            for ( int i = 0; i < count; ++i )
            {
                var angle = angleBase + 
                    angleRange * ( ( float )i / ( count - 1 ) - 0.5f );

                var shot = Instantiate( m_shotPrefab, pos, rot );

                shot.Init( angle, speed );
            }
        }
        else if ( count == 1 )
        {
            var shot = Instantiate( m_shotPrefab, pos, rot );

            shot.Init( angleBase, speed );
        }
    }
    public void Damage( int damage )
    {
        var audioSource = FindObjectOfType<AudioSource>();
        audioSource.PlayOneShot( m_damageClip );
        
        m_hp -= damage;

        if ( 0 < m_hp ) return;

        gameObject.SetActive( false );
    }
    public void AddExp( int exp )
    {
        
        m_exp += exp;
        // m_waza += exp;

        // if(m_waza > m_needWaza){
        //     ShootNWay( angleBase, angleRange, 0.15f, count );
        //     ShootNWay( angleBase, angleRange, 0.2f, count );
        //     ShootNWay( angleBase, angleRange, 0.25f, count );
        //     ShootNWay( angleBase, angleRange, 0.35f, count );
        //     ShootNWay( angleBase, angleRange, 0.45f, count );
        //     ShootNWay( angleBase, angleRange, 0.55f, count );
        //     ShootNWay( angleBase, angleRange, 0.65f, count );
        //     ShootNWay( angleBase, angleRange, 0.75f, count );
        //     ShootNWay( angleBase, angleRange, 0.85f, count );
        //     ShootNWay( angleBase, angleRange, 0.95f, count );
        //     ShootNWay( angleBase, angleRange, 1.05f, count );

        //     m_waza = 0;
        // }

        if ( m_exp < m_needExp ) return;

        wazacount = 1;

        // if(Input.GetKey(KeyCode.Space)){

        // if(m_shotAngleRange<60){
        //     m_shotAngleRange+=10;
        // }

        // m_level++;

        // m_prevNeedExp = m_needExp;

        // m_needExp = GetNeedExp( m_level );
        

        // var angleBase = 0;
        // var angleRange = 360;
        // var count = 28;
        // ShootNWay( angleBase, angleRange, 0.15f, count );
        // ShootNWay( angleBase, angleRange, 0.2f, count );
        // ShootNWay( angleBase, angleRange, 0.25f, count );
        // ShootNWay( angleBase, angleRange, 0.35f, count );
        // ShootNWay( angleBase, angleRange, 0.45f, count );
        // ShootNWay( angleBase, angleRange, 0.55f, count );
        // ShootNWay( angleBase, angleRange, 0.65f, count );
        // ShootNWay( angleBase, angleRange, 0.75f, count );
        // ShootNWay( angleBase, angleRange, 0.85f, count );
        // ShootNWay( angleBase, angleRange, 0.95f, count );
        // ShootNWay( angleBase, angleRange, 1.05f, count );


        // var audioSource = FindObjectOfType<AudioSource>();
        // audioSource.PlayOneShot( m_levelUpClip );

        // var t = ( float )( m_level - 1 ) / ( m_levelMax - 1 );
        // m_shotCount = Mathf.RoundToInt( 
        //     Mathf.Lerp( m_shotCountFrom, m_shotCountTo, t ) );
        // m_shotInterval = Mathf.Lerp( 
        //     m_shotIntervalFrom, m_shotIntervalTo, t );
        // m_magnetDistance = Mathf.Lerp( 
        //     m_magnetDistanceFrom, m_magnetDistanceTo, t );

    }
    private int GetNeedExp( int level )
    {
        return m_nextExpBase + 
            m_nextExpInterval * ( ( level - 1 ) * ( level - 1 ) );
    }
}