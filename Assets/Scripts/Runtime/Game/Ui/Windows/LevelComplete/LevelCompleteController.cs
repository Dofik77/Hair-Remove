using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using DG.Tweening;
using Game.SceneLoading;
using Runtime.Services.AnalyticsService;
using Runtime.Services.CommonPlayerData;
using Runtime.Services.CommonPlayerData.Data;
using SimpleUi.Abstracts;
using UniRx;
using UnityEngine;
using Utils.UiExtensions;
using Zenject;
using Random = UnityEngine.Random;

namespace Runtime.Game.Ui.Windows.LevelComplete 
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class LevelCompleteController : UiController<LevelCompleteView>, IInitializable
    {
        [Inject] private IAnalyticsService _analyticsService;
        [Inject] private readonly ICommonPlayerDataService<CommonPlayerData> _commonPlayerData;
        [Inject] private readonly SignalBus _signalBus;
        
        private readonly ISceneLoadingManager _sceneLoadingManager;
        //private EScene loopedLevel = EScene.Level_01;
        
        public LevelCompleteController(ISceneLoadingManager sceneLoadingManager)
        {
            _sceneLoadingManager = sceneLoadingManager;
        }
        
        public void Initialize()
        {
            View.NextLevel.OnClickAsObservable().Subscribe(x => OnNextLevel()).AddTo(View.NextLevel);
        }
        
        private void OnNextLevel()
        {
            _sceneLoadingManager.LoadScene(_commonPlayerData.GetData().Level);
        }

        public override void OnShow()
        {
            _analyticsService.SendRequest("level_complete");
            OnFinish();
        }

        private void OnFinish()
        {
            // var data = _commonPlayerData.GetData();
            // View.Show(data.Level);
            // if (data.Level >= Enum.GetValues(typeof(EScene)).Cast<EScene>().Last())
            // {
            //     data.Level = loopedLevel;
            //     _analyticsService.SendRequest("last_level_complete");
            // }
            // else
            //     data.Level++;
            // var rand = Random.Range(300, 600);
            // data.Money += rand;
            // _commonPlayerData.Save(data);
            // View.Currency.text = (data.Money - rand).ToString();
            // View.RewardValue.text = new StringBuilder("+").Append(rand).ToString();
            // View.transform.DOMove(Vector3.zero, 0.7f).SetRelative(true).OnComplete(() => View.GetReward(data.Money));
        }
    }
}