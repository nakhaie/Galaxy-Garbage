using UnityEngine;

namespace Controllers
{
    public class AudioController : MonoBehaviour, IAudioController
    {
        [Header("Source")]
        [SerializeField] private AudioSource effectSource;
        [SerializeField] private AudioSource attackSource;

        [Header("Clip")]
        [SerializeField] private AudioClip[] enemyDiedClips;
        [SerializeField] private AudioClip[] attackClips;

        [Header("Pitch")]
        [SerializeField] private Vector2 destroyPitch;
        [SerializeField] private Vector2 attackPitch;

        private int _lastIndex;

    #region Interface Methods

        public void EnemyDied()
        {
            int index = Random.Range(0, enemyDiedClips.Length);

            if (index == _lastIndex)
            {
                index++;

                if (index >= enemyDiedClips.Length)
                {
                    index = 0;
                }
            }

            _lastIndex = index;

            effectSource.clip = enemyDiedClips[index];

            effectSource.pitch = Random.Range(destroyPitch.x, destroyPitch.y);

            effectSource.Play();
        }

        public void Attack()
        {
            int index = Random.Range(0, attackClips.Length);

            attackSource.clip = attackClips[index];

            attackSource.pitch = Random.Range(attackPitch.x, attackPitch.y);

            attackSource.Play();
        }

    #endregion
    }

    public interface IAudioController
    {
        void EnemyDied();
        void Attack();
    }
}