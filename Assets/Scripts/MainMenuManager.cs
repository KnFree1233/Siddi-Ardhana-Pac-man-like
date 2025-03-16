using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] Transform pacmanRunAnimate;
    [SerializeField] float animateSpeed;
    [SerializeField] SpriteRenderer[] spriteRenderers;

    private float startXPos;
    private float endXPos;
    private bool isReverse;
    Vector3 moveToward;

    private void Start()
    {
        isReverse = false;
        moveToward = Vector3.right;
        startXPos = pacmanRunAnimate.position.x;
        endXPos = -pacmanRunAnimate.position.x;
    }

    private void Update()
    {
        MoveSpriteAnimate();
    }

    private void MoveSpriteAnimate()
    {
        if (pacmanRunAnimate.position.x > endXPos || pacmanRunAnimate.position.x < startXPos)
        {
            isReverse = !isReverse;
            moveToward = isReverse ? Vector3.left : Vector3.right;
            FlippingSprite();
        }
        pacmanRunAnimate.Translate(moveToward * Time.deltaTime * animateSpeed);
    }

    private void FlippingSprite()
    {
        foreach (SpriteRenderer sprite in spriteRenderers)
        {
            sprite.flipX = isReverse;
        }
    }

    public void Play()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
