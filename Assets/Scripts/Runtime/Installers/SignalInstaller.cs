using ECS.Game.Components.Hair_Remove_Component;
using Signals;
using Zenject;

namespace Installers
{
    public class SignalInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.DeclareSignal<SignalGameInit>();
            Container.DeclareSignal<SignalScoreOpen>();
            Container.DeclareSignal<SignalMakeHudButtonsVisible>();
            Container.DeclareSignal<SignalBlackScreen>();
            Container.DeclareSignal<SignalQuestionChoice>();
            Container.DeclareSignal<SignalJoystickUpdate>();
            Container.DeclareSignal<SignalUpdateImpact>();
            Container.DeclareSignal<SignalGameEnd>();
        }
    }
}