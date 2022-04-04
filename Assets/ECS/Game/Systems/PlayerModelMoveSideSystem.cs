using DataBase.Game;
using ECS.Core.Utils.ReactiveSystem;
using ECS.Game.Components;
using ECS.Game.Components.Flags;
using ECS.Game.Components.Input;
using ECS.Utils.Extensions;
using ECS.Views.Impls;
using Leopotam.Ecs;
using UnityEngine;

namespace ECS.Game.Systems
{
    public class PlayerModelMoveSideSystem : ReactiveSystem<PointerDragComponent>
    {
        private readonly EcsFilter<PlayerComponent, RemapPointComponent, LinkComponent> _player;
        private readonly EcsFilter<GameStageComponent> _gameStage;
        protected override EcsFilter<PointerDragComponent> ReactiveFilter { get; }
        protected override bool DeleteEvent => false;
        protected override void Execute(EcsEntity entity)
        {
            if (_gameStage.Get1(0).Value != EGameStage.Play) return;
            var inputPos = entity.Get<PointerDragComponent>().Position;
            var input = _player.Get2(0).Input.x;
            var cubeRemapValueStart = _player.Get2(0).ModelPos.x;
            var newX = inputPos.x.Remap(
                input - 20, 
                input + 20, 
                cubeRemapValueStart - 2, 
                cubeRemapValueStart + 2);

            newX = Mathf.Clamp(newX, -2, 2);
                
            var playerView = (CharacterView)_player.Get3(0).View;
            var modelTransform = playerView.ModelRootTransform;
            modelTransform.localPosition = new Vector3(newX, modelTransform.localPosition.y, modelTransform.localPosition.z);
        }
    }
}