using Unity.Entities;
using UnityEngine;
namespace PONG {
    public class ScoreProxy : MonoBehaviour, IConvertGameObjectToEntity
    {
        [SerializeField]private ScoreBoardHUD HUD;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            HUD.Entity = entity;
            
            Score score = new Score
            {
                Player1 = 0,
                Player2 = 0
            };

            dstManager.AddComponentData(entity, score);
    }


    }
}
