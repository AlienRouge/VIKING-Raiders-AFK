using System;
using System.Linq;
using System.Threading.Tasks;
using _Scripts.Enums;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts.UnitScripts
{
    public class BaseUnit : MonoBehaviour
    {
        public BaseUnitModel HeroStats;
        [SerializeField] private Team _myTeam;

        [SerializeField] private float _health;
        [SerializeField] private BaseUnit _currentTarget;
        [SerializeField] private Vector3 _spawnPosition;
        private bool _isDead => _health <= 0;
        private float _attackDeltaTime;
        private bool _canAttack = true;
        
        private MovementController _movementController;
        private BaseUnitView _baseUnitView;
        private HealthBar _healthBar;

        public UnityAction HealthChanged;
        
        private void Awake()
        {
            _movementController = GetComponent<MovementController>();
            _baseUnitView = GetComponentInChildren<BaseUnitView>();
            _healthBar = GetComponentInChildren<HealthBar>();
        }

        public void Init(Team team, Transform spawnPos, BaseUnitModel hero)
        {
            HeroStats = hero;
            _health = HeroStats.BaseHealth;
            _attackDeltaTime = 1 / HeroStats.AttackSpeed;
            _myTeam = team;
            _spawnPosition = spawnPos.position;
            _healthBar.SetMaxHealth(HeroStats.BaseHealth);
            
            _baseUnitView.SetUnitSprite(HeroStats.viewSprite, HeroStats.spriteScale);
            // _baseUnitView.SetTeamColor(_myTeam);
          

            transform.position = _spawnPosition;
        }


        private void FindTarget()
        {
            var enemies = BattleManager.Instance.GetEnemies(_myTeam);
            float minDistance = Mathf.Infinity;
            BaseUnit supposedEnemy = null;
            _currentTarget = null;

            // TODO Find with priority
            foreach (var enemy in enemies.Where(enemy =>
                         (Vector3.Distance(enemy.transform.position, this.transform.position) <= minDistance &&
                          !enemy._isDead)))
            {
                minDistance = Vector3.Distance(enemy.transform.position, this.transform.position);
                supposedEnemy = enemy;
            }

            _currentTarget = supposedEnemy;
            Debug.Log(_currentTarget
                ? $"{HeroStats.name}: Found new target({_currentTarget.HeroStats.name})."
                : $"{HeroStats.name}: No targets.");
        }

        private void AttackCurrentTarget()
        {
            _movementController.SetTarget(_currentTarget);
            if (Vector3.Distance(transform.position, _currentTarget.transform.position) <= HeroStats.AttackRange)
            {
                _movementController.Stop();
                Attack();
            }
            else
                _movementController.Resume();
        }

        private void Attack()
        {
            if (!_canAttack)
                return;
            Debug.Log($"{HeroStats.CharacterName}: Target in range. Attack.");
            _currentTarget.TakeDamage(HeroStats.BaseDamage);
            WaitCoroutine();
        }

        private void TakeDamage(float dmg)
        {
            if (dmg < 0)
                throw new ArgumentOutOfRangeException(nameof(dmg));

            Debug.Log($"{HeroStats.CharacterName}: Taking damage: {dmg}dmg");
            _health -= dmg;
            _healthBar.SetHealth(_health);
            HealthChanged?.Invoke();
            if (_isDead)
            {
                Debug.Log($"{HeroStats.CharacterName} dead.");
                EventController.UnitDied.Invoke(_myTeam, this);
            }
        }

        private async void WaitCoroutine()
        {
            _canAttack = false;
            await Task.Delay(Mathf.RoundToInt(_attackDeltaTime * 1000));
            _canAttack = true;
        }

        private void Update()
        {
            if (_currentTarget == null || _currentTarget._isDead)
                FindTarget();

            if (_currentTarget == null)
                return;

            AttackCurrentTarget();
        }
    }
}