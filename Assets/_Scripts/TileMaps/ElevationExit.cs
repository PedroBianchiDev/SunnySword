using UnityEngine;

namespace SunnySword.TileMap
{
    public class ElevationExit : MonoBehaviour
    {
        public Collider2D[] mountainColliders;
        public Collider2D[] boundaryColliders;


        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                foreach (Collider2D mountain in mountainColliders)
                {
                    mountain.enabled = true;
                }

                collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 14;

                foreach (Collider2D boundary in boundaryColliders)
                {
                    boundary.enabled = false;
                }

                collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 10;
            }
        }
    }
}