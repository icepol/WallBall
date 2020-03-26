using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Player))]
public class PlayerControls : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    [SerializeField] private Transform leftBoundary;
    [SerializeField] private Transform rightBoundary;
    
    [SerializeField] private KeyCode keyLeft;
    [SerializeField] private KeyCode keyRight;

    private Player _player;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleControls();
    }

    void HandleControls()
    {
        if (_player.IsKicking)
            return;
        
        Vector2 position = transform.position;
            
        if (Input.GetKey(keyLeft))
            position.x -= moveSpeed * Time.deltaTime;
        else if (Input.GetKey(keyRight))
            position.x += moveSpeed * Time.deltaTime;

        position.x = Mathf.Clamp(
            position.x,
            leftBoundary.transform.position.x,
            rightBoundary.transform.position.x
            );

        transform.position = position;
    }
}
