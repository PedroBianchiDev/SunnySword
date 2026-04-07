using UnityEngine;

namespace SunnySword.Animation
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteAnimator : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;
        private Sprite[] currentAnimation;
        private int currentFrame;
        private float timer;
        private float frameRate = 0.12f;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public float PlayAnimation(Sprite[] animationFrames, bool flipX)
        {
            spriteRenderer.flipX = flipX;

            if (currentAnimation != animationFrames)
            {
                currentAnimation = animationFrames;
                currentFrame = 0;
                timer = 0;
                if (currentAnimation.Length > 0)
                    spriteRenderer.sprite = currentAnimation[0];
            }

            return animationFrames.Length * frameRate;
        }

        private void Update()
        {
            if (currentAnimation == null || currentAnimation.Length == 0) return;

            timer += Time.deltaTime;

            if (timer >= frameRate)
            {
                timer -= frameRate;
                currentFrame = (currentFrame + 1) % currentAnimation.Length;
                spriteRenderer.sprite = currentAnimation[currentFrame];
            }
        }
    }
}