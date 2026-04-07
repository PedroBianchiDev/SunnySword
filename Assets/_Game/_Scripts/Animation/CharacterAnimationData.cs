using UnityEngine;

namespace SunnySword.Animation
{

    [CreateAssetMenu(fileName = "New Character Anim", menuName = "SunnySword/Animation/CharacterData")]
    public class CharacterAnimationData : ScriptableObject
    {
        [Header("Idle")]
        public Sprite[] idleSprites;

        [Header("Walk")]
        public Sprite[] walkSprites;

        [Header("Attack")]
        public Sprite[] firstAttackSprite;
        public Sprite[] secondAttackSprite;

        [Header("Defense")]
        public Sprite[] defenseSprite;
    }
}