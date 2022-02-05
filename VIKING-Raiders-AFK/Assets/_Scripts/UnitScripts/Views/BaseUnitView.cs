using System;
using System.Linq;
using System.Threading.Tasks;
using _Scripts.Enums;
using _Scripts.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts.UnitScripts.Views
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class BaseUnitView : MonoBehaviour
    {
        
        private MovementController _movementController;
        private SpriteRenderer _spriteRenderer;
        private IUnitModel _model;
        private float _attackDeltaTime;
        private BaseUnitView _currentTarget;
        private HealthBar _healthBar;

        public Team myTeam { get; private set; }  
        public float health { get; private set; }
        public string characterName => _model.characterName;
        public float moveSpeed => _model.moveSpeed;
        public float attackRange => _model.attackRange;
        public bool isDead => health <= 0;

        public UnityAction<BaseUnitView> EndFight;

        public void SetTeamColor(Team team)
        {
            _spriteRenderer.color = (team == Team.Team1)
                ? new Color(53/255f,72/255f,231/255f)
                : new Color(250/255f, 52/255f, 37/255f);
        }
        
        public void Init(IUnitModel model, Team team, Vector3 startPosition)
        {
            _model = model;
            _movementController = GetComponent<MovementController>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            health = model.baseHealth;
            _attackDeltaTime = 1 / model.attackSpeed;
            myTeam = team;
            transform.position = startPosition;
            _spriteRenderer.sprite = model.viewSprite;
            _movementController.Init(moveSpeed, attackRange);
            _healthBar = GetComponentInChildren<HealthBar>();
            _healthBar.SetMaxHealth(health);
            /*transform.localScale = model.spriteScale;*/
        }
        
        public bool CanFindTarget()
        {
            if (isDead) return false;
            var enemies = BattleController.Instance.GetEnemies(myTeam);
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
                ? $"{characterName}: Found new target({_currentTarget.characterName})."
                : $"{characterName}: No targets.");
            
            return _currentTarget != null;
        }
        
        public void AttackTarget()
        {
            _movementController.SetTarget(_currentTarget);
            MoveToTarget();
            /*_movementController.SetTarget(_currentTarget);*/
        }
        
        private void TakeDamage(float dmg)
        {
            if (dmg < 0)
                throw new ArgumentOutOfRangeException(nameof(dmg));

            Debug.Log($"{_model.characterName}: Taking damage: {dmg}dmg");
            health -= dmg;
            _healthBar.SetHealth(health);
            //HealthChanged?.Invoke();
            if (isDead)
            {
                Debug.Log($"{_model.characterName} dead.");
            }
        }

        private bool CanAttack()
        {
            Debug.Log(characterName);
            if (_currentTarget == null)
            {
                return false;
            }
            return Vector3.Distance(transform.position, _currentTarget.transform.position) <= attackRange;
        }

        private async void MoveToTarget()
        {
            if (isDead) return;
            while (!CanAttack())
            {
                await Task.Delay(100);
                if (_movementController.isActiveAndEnabled)
                {
                    _movementController.Resume();
                }
            }
            _movementController.Stop();
            StartFight();
        }
        
        private async void StartFight()
        {
            while (_currentTarget != null )
            {
                if (isDead) return;
                if (CanAttack())
                {
                    _currentTarget.TakeDamage(_model.baseDamage);
                    if (!_currentTarget.isDead)
                    {
                        await Task.Delay(Mathf.RoundToInt(_attackDeltaTime * 1000));
                    }
                    else
                    {
                        Debug.Log("smert");
                        EventController.UnitDied?.Invoke(_currentTarget.myTeam, _currentTarget);
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
            
            EndFight?.Invoke(this);
        }
    }
}