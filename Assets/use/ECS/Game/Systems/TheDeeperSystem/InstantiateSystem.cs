using ECS.Core.Utils.ReactiveSystem;
using ECS.Core.Utils.ReactiveSystem.Components;
using ECS.Game.Components;
using ECS.Utils;
using ECS.Views;
using Leopotam.Ecs;
using UnityEngine;
using Zenject;

namespace ECS.Game.Systems
{
    public class InstantiateSystem : ReactiveSystem<EventAddComponent<PrefabComponent>>
    {
        [Inject] private readonly ISpawnService<EcsEntity, ILinkable> _spawnService; //inject and Nedo-Singletone
        protected override EcsFilter<EventAddComponent<PrefabComponent>> ReactiveFilter { get; } //рефлексия 
        protected override void Execute(EcsEntity entity)
        {
            //after trohw here entity
            //мы оправляем entity в метод Spawn и получаем вьюшку ( родитель всех интерфейсов ) 
            
            var linkable = _spawnService.Spawn(entity);
            linkable?.Link(entity); // после отправления в линк наша вьюшка знает о сущности
            entity.Get<LinkComponent>().View = linkable; // а здесь сущность уже знает о вьюшке
            
            //таким образом Ilinkeble нам нужен для того что бы вьюшка узнала о сущности
            //а LinkComponent - что бы сущность имея компонент Link.comp.View (Ilinkable) узнала о вьюшке 
        }
    }
}