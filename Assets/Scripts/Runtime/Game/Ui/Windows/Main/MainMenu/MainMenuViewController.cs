using Game.SceneLoading;
using Runtime.Managers;
using SimpleUi.Abstracts;
using SimpleUi.Interfaces;
using SimpleUi.Signals;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Utils.UiExtensions;
using Zenject;

namespace Runtime.Game.Ui.Windows.MainMenu
{
    public class MainMenuViewController : UiController<MainMenuView>, IInitializable, IDefaultSelectable
    {
        private readonly SignalBus _signalBus;
        private readonly ISceneLoadingManager _sceneLoadingManager;
        private readonly IGameDataManager _gameDataManager;

        public Selectable DefaultSelectable => View.NewGame;

        public MainMenuViewController(SignalBus signalBus, ISceneLoadingManager sceneLoadingManager,
            IGameDataManager gameDataManager)
        {
            _signalBus = signalBus;
            _sceneLoadingManager = sceneLoadingManager;
            _gameDataManager = gameDataManager;
        }

        public void Initialize()
        {
            View.NewGame.OnClickAsObservable().Subscribe(x => OnNewGame()).AddTo(View.NewGame);
        }
        private void OnNewGame() => _sceneLoadingManager.LoadScene(EScene.Game0_1);
    }
}