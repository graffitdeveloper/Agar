using System.Collections.Generic;
using gRaFFit.Agar.Controllers.GameScene.MainControllers;
using gRaFFit.Agar.Controllers.InputSystem;
using gRaFFit.Agar.Models.ControllerSwitcherSystem;
using gRaFFit.Agar.Views;
using gRaFFit.Agar.Views.CameraControls;
using gRaFFit.Agar.Views.Pool;
using gRaFFit.Agar.Views.UIPanelSystem;
using Models;
using UnityEditor.iOS.Xcode;
using UnityEngine;
using Character = Models.Character;

namespace Controllers {
	public class GameControllerView : AControllerView {
		[SerializeField] private PlayerView _playerView;
		[SerializeField] private MeshRenderer _bgMeshRenderer;
		[SerializeField] private int _targetCharactersCount;

		[SerializeField] private EnemyView _enemyViewPrefab;

		private Character _playerCharacter;

		private List<Character> _enemies;

		private List<EnemyView> _enemyViews;

		private const string ENEMY_POOL_KEY = "ENEMY_POOL_KEY";

		public override void Activate() {
			_playerCharacter = new Character();

			_playerView.gameObject.SetActive(true);
			_playerView.Init(_playerCharacter.ID);
			_playerView.SignalOnCharactersCollided.AddListener(OnCharactersCollided);
			
			PoolService.Instance.InitPoolWithNewObject(ENEMY_POOL_KEY, _enemyViewPrefab);
			UIManager.Instance.ShowPanel<HudPanelView>();
			RefreshPlayerWeight(true);

			_bgMeshRenderer.gameObject.SetActive(true);
			CameraView.Instance.SetToPlayer(_playerView);
			CookieSpawner.Instance.Init();
			CookieSpawner.Instance.InstantiateAllCookies();
			
			_enemies = new List<Character>();
			_enemyViews = new List<EnemyView>();

			for (int i = 0; i < _targetCharactersCount; i++) {
				var enemyModel = new Character();
				_enemies.Add(enemyModel);

				var newEnemy = PoolService.Instance.PopObject(ENEMY_POOL_KEY) as EnemyView;
				if (newEnemy != null) {
					_enemyViews.Add(newEnemy);
					newEnemy.Init(enemyModel.ID);
					newEnemy.transform.position = CookieSpawner.Instance.GetRandomPositionInLevel();
					newEnemy.FindNewTarget();
					newEnemy.SignalOnCharactersCollided.AddListener(OnCharactersCollided);
				}
			}

			AddListeners();
		}


		private void OnCharactersCollided(CharacterView characterA, CharacterView characterB) {
			var characterAModel = _enemies.Find(enemy => enemy.ID == characterA.ID);
			if (characterAModel == null && _playerCharacter.ID == characterA.ID) {
				characterAModel = _playerCharacter;
			}

			var characterBModel = _enemies.Find(enemy => enemy.ID == characterB.ID);
			if (characterBModel == null && _playerCharacter.ID == characterB.ID) {
				characterBModel = _playerCharacter;
			}

			if (characterAModel == null) {
				Debug.LogError("character A model is missing!");
			} else if (characterBModel == null) {
				Debug.LogError("character B model is missing!");
			} else {
				if (characterAModel.Weight > characterBModel.Weight) {
					var damage = characterBModel.Hit() * 0.5f;
					characterB.CookiesDropped();
					for (int i = 0; i < damage; i++) {
						CookieSpawner.Instance.SpawnCookieByHit(characterB.transform.position);
					}
				} else {
					var damage = characterAModel.Hit() * 0.5f;
					characterA.CookiesDropped();
					for (int i = 0; i < damage; i++) {
						CookieSpawner.Instance.SpawnCookieByHit(characterA.transform.position);
					}
				}
				
				characterA.SetWeight(characterAModel.Weight);
				characterB.SetWeight(characterBModel.Weight);
			}
		}

		public override void Deactivate() {
			_playerView.gameObject.SetActive(false);
			_bgMeshRenderer.gameObject.SetActive(false);

			for (int i = 0; i < _enemyViews.Count; i++) {
				_enemyViews[i].Dispose();
			}

			_enemyViews.Clear();
			_enemyViews = null;

			CookieSpawner.Instance.Clear();
			_playerCharacter = null;
			_enemies.Clear();
			_enemies = null;

			UIManager.Instance.HidePanel<HudPanelView>();

			base.Deactivate();
		}

		private void RefreshPlayerWeight(bool immediately) {
			_playerView.SetWeight(_playerCharacter.Weight);
			UIManager.Instance.GetPanel<HudPanelView>().RefreshWeight(_playerCharacter.Weight, immediately);
			CameraView.Instance.SetTargetOrthoAccordingWithWeight(_playerCharacter.Weight);
		}

		private void RefreshEnemyWeight(int id) {
			var enemyModel = _enemies.Find(enemy => enemy.ID == id);
			var enemyView = _enemyViews.Find(enemy => enemy.ID == id);
			if (enemyView != null) {
				enemyView.SetWeight(enemyModel.Weight);
			}
		}

		protected override void AddListeners() {
			base.AddListeners();

			InputController.Instance.SignalOnTouchStart.AddListener(OnTouchStart);
			InputController.Instance.SignalOnTouchEnd.AddListener(OnTouchEnd);
			InputController.Instance.SignalOnTouch.AddListener(OnTouch);

			var hud = UIManager.Instance.GetPanel<HudPanelView>();
			if (hud != null) {
				hud.SignalOnRestartButtonClicked.AddListener(OnRestartButtonClicked);
				hud.SignalOnGoToLobbyButtonClicked.AddListener(OnGoToLobbyButtonClicked);
			}

			CookieSpawner.Instance.SignalOnCookieEatenByPlayer.AddListener(OnCookieEatenByPlayer);
			CookieSpawner.Instance.SignalOnCookieEatenByEnemy.AddListener(OnCookieEatenByEnemy);
		}

		protected override void RemoveListeners() {
			InputController.Instance.SignalOnTouchStart.RemoveListener(OnTouchStart);
			InputController.Instance.SignalOnTouchEnd.RemoveListener(OnTouchEnd);
			InputController.Instance.SignalOnTouch.RemoveListener(OnTouch);

			var hud = UIManager.Instance.GetPanel<HudPanelView>();
			if (hud != null && hud.IsSignalsInited) {
				hud.SignalOnRestartButtonClicked.RemoveListener(OnRestartButtonClicked);
				hud.SignalOnGoToLobbyButtonClicked.RemoveListener(OnGoToLobbyButtonClicked);
			}

			CookieSpawner.Instance.SignalOnCookieEatenByPlayer.RemoveListener(OnCookieEatenByPlayer);
			CookieSpawner.Instance.SignalOnCookieEatenByEnemy.RemoveListener(OnCookieEatenByEnemy);

			base.RemoveListeners();
		}

		private void OnGoToLobbyButtonClicked() {
			ControllerSwitcher.Instance.SwitchController(ControllerType.Lobby);
		}

		private void OnRestartButtonClicked() {
			ControllerSwitcher.Instance.RestartCurrentController();
		}

		private void OnCookieEatenByPlayer(float cookieScale) {
			_playerCharacter.EatCookie(cookieScale);
			RefreshPlayerWeight(false);
			CookieSpawner.Instance.SpawnNewCookie();
		}

		private void OnCookieEatenByEnemy(float cookieWeight, int enemyID) {
			var targetEnemy = _enemies.Find(enemy => enemy.ID == enemyID);
			if (targetEnemy != null) {
				targetEnemy.EatCookie(cookieWeight);
				RefreshEnemyWeight(enemyID);
				CookieSpawner.Instance.SpawnNewCookie();
			} else {
				Debug.LogError($"target enemy model with {enemyID} is missing, wtf");
			}
		}

		private void OnTouchStart(Vector2 obj) {
			_playerView.PlayWalkAnimation();
		}

		private void OnTouchEnd(Vector2 obj) {
			_playerView.Stop();
		}

		private void OnTouch(Vector2 obj) {
			_playerView.FaceToTouch();
			_playerView.MoveByControls();

			_bgMeshRenderer.material.mainTextureOffset = _playerView.GetOffsetForBG();
			_bgMeshRenderer.transform.position = new Vector3(
				_playerView.transform.position.x,
				_playerView.transform.position.y,
				_bgMeshRenderer.transform.position.z);
		}

		private void Update() {
			if (_enemyViews != null) {
				for (int i = 0; i < _enemyViews.Count; i++) {
					_enemyViews[i].MoveToTarget();
				}
			}

			if (Input.GetKeyDown(KeyCode.Space)) {
				_playerCharacter.EatCookie(20);
				RefreshPlayerWeight(false);
			}
		}

		public override ControllerType Type => ControllerType.Game;
	}
}