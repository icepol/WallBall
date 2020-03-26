using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        EventManager.AddListener(Events.BallBounce, OnBallBounce);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(Events.BallBounce, OnBallBounce);
    }

    void OnBallBounce()
    {
        _animator.SetTrigger("Shake");
    }
}
