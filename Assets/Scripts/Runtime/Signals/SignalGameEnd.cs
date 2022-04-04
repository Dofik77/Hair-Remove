using DataBase.Game;

namespace Signals
{
    public struct SignalGameEnd
    {
        public EGameResult Result;

        public SignalGameEnd(EGameResult result)
        {
            Result = result;
        }
    }
}