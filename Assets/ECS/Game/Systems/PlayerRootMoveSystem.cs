using DataBase.Game;
using ECS.Core.Utils.SystemInterfaces;
using ECS.Game.Components;
using ECS.Game.Components.Events;
using ECS.Game.Components.Flags;
using ECS.Utils.Impls;
using Leopotam.Ecs;
using UnityEngine;

namespace ECS.Game.Systems
{
    public class PlayerRootMoveSystem : IEcsUpdateSystem
    {
        private readonly EcsFilter<PlayerComponent, PositionComponent, RotationComponent> _player;
        private readonly EcsFilter<GameStageComponent> _gameStage;
        private readonly EcsFilter<PathMovePoint, PositionComponent, RotationComponent> _pathPoints;
        public void Run() 
        {
            foreach (var g in _gameStage)
            {
                if(_gameStage.Get1(g).Value != EGameStage.Play) return;
               
                var entity = _player.GetEntity(0);
                if (entity.Has<TargetPositionComponent>()) continue;
                ref var playerPos = ref _player.Get2(0).Value;
                 
                var closestPoint = GetClosestPoint(playerPos);
                if (closestPoint == Vector3.zero)
                {
                    _player.GetEntity(0).Get<GameResultComponent>().Value = EGameResult.Win;
                    _gameStage.GetEntity(0).Get<ChangeStageComponent>().Value = EGameStage.GameEnd;
                    return;
                }
                
                ref var targetPos = ref entity.Get<TargetPositionComponent>();
                targetPos.Value = closestPoint;
                targetPos.Speed = 5;
            }
        }

        private Vector3 GetClosestPoint(Vector3 playerPos)
        {
            var closestPoint = Vector3.zero;
            var minDistance = float.MaxValue;
            foreach (var i in _pathPoints)
            {
                var pos = _pathPoints.Get2(i).Value;
                var distance = Vector3.Distance(pos, playerPos);
                if (distance < 0.01f)
                {
                    var playerEntity = _player.GetEntity(0);
                    if (_pathPoints.GetEntity(i).Has<RotationDirectionComponent>())
                    {
                        var playerRot = _player.Get3(0).Value;
                        ref var targetRot = ref playerEntity.Get<TargetRotationComponent>();
                        var yaw = _pathPoints.GetEntity(i).Get<RotationDirectionComponent>().Value switch {
                            ERotateDirection.Left => -90,
                            ERotateDirection.Right => 90,
                            _ => 0
                        };
                        targetRot.Value = playerRot * Quaternion.Euler(0, yaw, 0);
                        targetRot.Speed = 360  * 0.1f;
                    }
                    _pathPoints.GetEntity(i).Destroy();
                    continue;
                }
                if (!(distance < minDistance)) continue;
                minDistance = distance;
                closestPoint = pos;

            }
            return closestPoint;
        }
    }
}