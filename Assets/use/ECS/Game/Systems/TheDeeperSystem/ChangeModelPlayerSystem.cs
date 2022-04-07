using ECS.Core.Utils.ReactiveSystem;
using ECS.Core.Utils.SystemInterfaces;
using ECS.Game.Components;
using ECS.Game.Components.Flags;
using ECS.Views.Impls;
using Leopotam.Ecs;
using UnityEngine;

namespace ECS.Game.Systems.TheDeeperSystem
{
    public class ChangeModelPlayerSystem : IEcsUpdateSystem
    {
        private EcsFilter<SpherePlayerComponent, LinkComponent> _player;

        private float MaxVelocity = 6.5f;
        private float MaxAngularVelocity = 6.5f;
        private float MaxImpact = 4f;

        private PlayerState playerState;
        public void Run()
        {
            foreach (var player in _player)
            {
                SpherePlayerView playerView = (SpherePlayerView) _player.GetEntity(player).Get<LinkComponent>().View;
                
                LogicUpdate(playerView);
                
                switch (playerState)
                {
                    case PlayerState.RotationState :
                        playerView.ScareEmoji.SetActive(false);
                        playerView.SimpleEmoji.SetActive(false);
                        playerView.CrushedEmoji.SetActive(false);
                        playerView.RotationEmoji.SetActive(true);
                        break;
                    
                    case PlayerState.ScareState :
                        playerView.ScareEmoji.SetActive(true);
                        playerView.SimpleEmoji.SetActive(false);
                        playerView.CrushedEmoji.SetActive(false);
                        playerView.RotationEmoji.SetActive(false);
                        break;
                    
                    case PlayerState.SimpleState :
                        playerView.ScareEmoji.SetActive(false);
                        playerView.SimpleEmoji.SetActive(true);
                        playerView.CrushedEmoji.SetActive(false);
                        playerView.RotationEmoji.SetActive(false);
                        break;
                    
                    case PlayerState.CrushedState :
                        playerView.ScareEmoji.SetActive(false);
                        playerView.SimpleEmoji.SetActive(false);
                        playerView.CrushedEmoji.SetActive(true);
                        playerView.RotationEmoji.SetActive(false);
                        break;
                }
            }
        }
        
        private void LogicUpdate(SpherePlayerView player)
        {
            if (player.Rigidbody.velocity.magnitude > MaxVelocity)
            {
                playerState = PlayerState.ScareState;
                return;
            }
            
            if (player.Rigidbody.angularVelocity.magnitude > MaxAngularVelocity)
            {
                playerState = PlayerState.RotationState;
                return;
            }
            
            if (player.CollisionForce.magnitude > MaxImpact)
            {
                playerState = PlayerState.CrushedState;
                return;
            }
            
            playerState = PlayerState.SimpleState;
            
        }
        
        public enum PlayerState
        {
            SimpleState,
            ScareState,
            CrushedState,
            RotationState
        }
    }
}