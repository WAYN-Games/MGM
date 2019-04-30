using Unity.Entities;

namespace PONG
{
    public struct Score : IComponentData
    {
        public int Player1;
        public int Player2;
    }
}
