using System.Collections;
using UnityEngine;

public class Leg : MonoBehaviour
{
    [SerializeField] private float kickForce;
    
    private Player _player;

    private void Awake()
    {
        _player = GetComponentInParent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Ball ball = other.GetComponent<Ball>();
        if (ball && _player.IsPlaying && !_player.IsKicking && ball.transform.position.y > transform.position.y + 0.2f)
        {
            _player.IsKicking = true;

            StartCoroutine(Kick(ball));
        }
    }

    IEnumerator Kick(Ball ball)
    {
        ball.Kick(kickForce, transform.position);
        
        Vector2 position = transform.localPosition;

        position.y = 0.4f;
        transform.localPosition = position;

        yield return new WaitForSeconds(Constants.SleepAfterKick);
        
        position.y = 0;
        transform.localPosition = position;
    }
}
