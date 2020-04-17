using UnityEngine;

public class Shot : MonoBehaviour
{
    private Vector3 m_velocity;


    private void Update()
    {
        transform.localPosition += m_velocity;
    }

    public void Init( float angle, float speed )
    {
        var direction = Utils.GetDirection( angle );

        m_velocity = direction * speed;

        var angles = transform.localEulerAngles;
        angles.z = angle - 90;
        transform.localEulerAngles = angles;
        Destroy( gameObject, 2 );
    }
}