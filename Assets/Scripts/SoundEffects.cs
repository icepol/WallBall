using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    [SerializeField] private AudioClip[] ball;
    [SerializeField] private AudioClip[] kick;
    [SerializeField] private AudioClip[] scream;

    private Transform _cameraTransform;

    private void Start()
    {
        _cameraTransform = Camera.main.transform;
    }

    public void PlayOnBall()
    {
        if (ball.Length > 0)
            AudioSource.PlayClipAtPoint(ball[Random.Range(0, ball.Length)], _cameraTransform.position);
    }

    public void PlayOnKick()
    {
        if (kick.Length > 0)
            AudioSource.PlayClipAtPoint(kick[Random.Range(0, kick.Length)], _cameraTransform.position);
    }

    public void PlayOnScream()
    {
        if (scream.Length > 0)
            AudioSource.PlayClipAtPoint(scream[Random.Range(0, scream.Length)], _cameraTransform.position);
    }
}
