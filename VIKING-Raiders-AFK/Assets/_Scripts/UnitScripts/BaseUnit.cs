/*using System;
using System.Linq;
using System.Threading.Tasks;
using _Scripts.Enums;
using _Scripts.Interfaces;
using _Scripts.UnitScripts.Views;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts.UnitScripts
{
    public class BaseUnit : MonoBehaviour
    {
        public IUnitModel HeroStats;
        
        private MovementController _movementController;
        private BaseUnitView _baseUnitView;
        private HealthBar _healthBar;
        private Vector3 _spawnPosition;
        private BaseUnitView _currentTarget; 
        
        
        public UnityAction HealthChanged;
        
        private void Awake()
        {
            _movementController = GetComponent<MovementController>();
            _baseUnitView = GetComponentInChildren<BaseUnitView>();
            _healthBar = GetComponentInChildren<HealthBar>();
        }

        public void Init(Team team, Transform spawnPos, BaseUnitModelModel hero)
        {
            HeroStats = hero;
            _healthBar.SetMaxHealth(HeroStats.baseHealth);
            
            /*_baseUnitView.SetUnitSprite(HeroStats.viewSprite, HeroStats.spriteScale);#1#
            // _baseUnitView.SetTeamColor(_myTeam);
          

            transform.position = _spawnPosition;
        }


        private void FindTarget()
        {
            var enemies = BattleManager.Instance.GetEnemies(_baseUnitView.myTeam);
            float minDistance = Mathf.Infinity;
            BaseUnitView supposedEnemy = null;
            _currentTarget = null;

            // TODO Find with priority
            foreach (var enemy in enemies.Where(enemy =>
                         (Vector3.Distance(enemy.transform.position, this.transform.position) <= minDistance &&
                          !enemy.isDead)))
            {
                minDistance = Vector3.Distance(enemy.transform.position, this.transform.position);
                supposedEnemy = enemy;
            }

            _currentTarget = supposedEnemy;
            Debug.Log(_currentTarget
                ? $"{HeroStats.characterName}: Found new target({_currentTarget.characterName})."
                : $"{HeroStats.characterName}: No targets.");
        }

        private void AttackCurrentTarget()
        {
            _movementController.SetTarget(_currentTarget);
            if (Vector3.Distance(transform.position, _currentTarget.transform.position) <= HeroStats.attackRange)
            {
                _movementController.Stop();
                _baseUnitView.Attack(_currentTarget);
            }
            else
                _movementController.Resume();
        }

        private void Update()
        {
            if (_currentTarget == null || _currentTarget.isDead)
                FindTarget();

            if (_currentTarget == null)
                return;

            AttackCurrentTarget();
        }
    }
}*/