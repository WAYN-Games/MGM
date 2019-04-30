using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace PONG
{
    public class ScoreBoardHUD : MonoBehaviour
    {
        [SerializeField] private Text ScoreP1;
        [SerializeField] private Text ScoreP2;
        public Entity Entity;
        private EntityManager em;

        private void Awake()
        {
            em = World.Active.EntityManager;
        }

        private void Update()
        {
            Score score = em.GetComponentData<Score>(Entity);
            ScoreP1.text = score.Player1.ToString();
            ScoreP2.text = score.Player2.ToString();
        }
    }
}
