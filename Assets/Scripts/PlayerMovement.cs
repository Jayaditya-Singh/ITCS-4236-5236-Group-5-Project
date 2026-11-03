using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float framesPerSecond = 6f;

    [Header("Walk frames (2 each)")]
    [SerializeField] private Sprite[] down = new Sprite[2];
    [SerializeField] private Sprite[] up = new Sprite[2];
    [SerializeField] private Sprite[] left = new Sprite[2];
    [SerializeField] private Sprite[] right = new Sprite[2];

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Vector2 input;
    private Sprite[] current;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        current = down;
    }

    void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        input = input.normalized;

        UpdateSprite();
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + input * moveSpeed * Time.fixedDeltaTime);
    }

    private void UpdateSprite()
    {
        if (input == Vector2.zero)
        {
            // Idle: first frame of the last direction faced
            sr.sprite = current[0];
            return;
        }

        // Dominant axis decides facing so diagonals don't flicker
        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            current = input.x > 0 ? right : left;
        else
            current = input.y > 0 ? up : down;

        int frame = (int)(Time.time * framesPerSecond) % 2;
        sr.sprite = current[frame];
    }
}