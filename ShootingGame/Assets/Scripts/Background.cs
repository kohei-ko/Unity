using UnityEngine;

public class Background : MonoBehaviour
{
    public Transform m_player;
    public Vector2 m_limit;

    private void Update()
    {
        var pos = m_player.localPosition;

        var limit = Utils.m_moveLimit;


        var tx = 1 - Mathf.InverseLerp( -limit.x, limit.x, pos.x );
        var ty = 1 - Mathf.InverseLerp( -limit.y, limit.y, pos.y );

        var x = Mathf.Lerp( -m_limit.x, m_limit.x, tx );
        var y = Mathf.Lerp( -m_limit.y, m_limit.y, ty );

        transform.localPosition = new Vector3( x, y, 0 );
    }
}