﻿using DataBase.Game;
using ECS.Core.Utils.ReactiveSystem;
using ECS.Game.Components;
using ECS.Game.Components.Events;
using ECS.Game.Components.Flags;
using ECS.Game.Components.TheDeeperComponent;
using ECS.Utils.Extensions;
using ECS.Views.Impls;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace ECS.Game.Systems
{
    public class StartGameSystem : ReactiveSystem<ChangeStageComponent>
    {
        private readonly EcsFilter<PlayerComponent, ImpactComponent, LinkComponent> _player;
        private readonly EcsFilter<CameraComponent, LinkComponent> _camera;
        
        protected override EcsFilter<ChangeStageComponent> ReactiveFilter { get; }
        protected override bool DeleteEvent => false;

        private EcsEntity _cameraEntity;
        private CameraView _cameraView;
        protected override void Execute(EcsEntity entity)
        {
            if(entity.Get<ChangeStageComponent>().Value != EGameStage.Play) return;
            foreach (var i in _player)
            {
                CharacterView playerView = _player.Get3(i).View as CharacterView;
                var playerImpact = _player.Get2(i).Value;
                // playerView.SetStage(playerImpact.ComparePlayerStage());
            }
        }
    }
}