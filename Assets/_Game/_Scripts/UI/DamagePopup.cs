using UnityEngine;
using TMPro;

namespace SunnySword.UI
{
    public class DamagePopup : MonoBehaviour
    {
        private TextMeshPro textMesh;
        private float disappearTimer;
        private Color textColor;
        private Vector3 moveVector;

        private void Awake()
        {
            textMesh = GetComponent<TextMeshPro>();

            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.sortingLayerName = "UI";
            meshRenderer.sortingOrder = 1;
        }

        public void Setup(int damageAmount, GameObject targetObj)
        {
            textMesh.SetText(damageAmount.ToString());
            textColor = textMesh.color;
            disappearTimer = 0.5f;

            Vector3 spawnPosition = targetObj.transform.position;
            float heightOffset = 0f;

            if (targetObj.TryGetComponent<Collider2D>(out Collider2D targetCollider))
            {
                float topOfCollider = targetCollider.bounds.max.y;

                heightOffset = topOfCollider - spawnPosition.y;
            }
            else if (targetObj.TryGetComponent<SpriteRenderer>(out SpriteRenderer targetSprite))
            {
                heightOffset = targetSprite.bounds.extents.y;
            }

            transform.position = spawnPosition + new Vector3(0f, heightOffset + 0.2f, 0f);

            moveVector = new Vector3(Random.Range(-0.8f, 0.8f), 1.5f, 0f);
        }

        private void Update()
        {
            transform.position += moveVector * Time.deltaTime;
            moveVector -= moveVector * 2f * Time.deltaTime;

            if (transform.localScale.x > 0.1f)
            {
                transform.localScale -= Vector3.one * 1.5f * Time.deltaTime;
            }

            disappearTimer -= Time.deltaTime;
            if (disappearTimer < 0)
            {
                textColor.a -= 3f * Time.deltaTime;
                textMesh.color = textColor;

                if (textColor.a <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}