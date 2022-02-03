using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using _Scripts.Enums;
using UnityEngine;
using UnityEngine.Events;

namespace UnitScripts
{
    public class BaseUnit : MonoBehaviour
    {
        public Hero heroStats { get; private set; }
        [SerializeField] private Team myTeam;

        [SerializeField] private int health;
        [SerializeField] private BaseUnit currentTarget;
        [SerializeField] private Vector3 spawnPosition;
        private bool isDead => health <= 0;
        private float attackDeltaTime;
        private bool canAttack = true;

        private SpriteRenderer spriteRenderer;
        private MovementController movementController;
        private HealthBar healthBar;

        public UnityAction healthChanged;
        
        private void Awake()
        {
            movementController = GetComponent<MovementController>();
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            healthBar = GetComponentInChildren<HealthBar>();
        }

        public void Init(Team team, Transform spawnPos, Hero hero)
        {
            heroStats = hero;
            health = heroStats.baseHealth;
            attackDeltaTime = 1 / heroStats.attackSpeed;
            myTeam = team;
            spawnPosition = spawnPos.position;
            healthBar.SetMaxHealth(heroStats.baseHealth);

            transform.position = spawnPosition;
            spriteRenderer.color = (myTeam == Team.Team1)
                ? new Color(53/255f,72/255f,231/255f)
                : new Color(250/255f, 52/255f, 37/255f);
        }


        private void FindTarget()
        {
            var enemies = BattleManager.Instance.GetEnemies(myTeam);
            float minDistance = Mathf.Infinity;
            BaseUnit supposedEnemy = null;
            currentTarget = null;

            // TODO Find with priority
            foreach (var enemy in enemies.Where(enemy =>
                         (Vector3.Distance(enemy.transform.position, this.transform.position) <= minDistance &&
                          !enemy.isDead)))
            {
                minDistance = Vector3.Distance(enemy.transform.position, this.transform.position);
                supposedEnemy = enemy;
            }

            currentTarget = supposedEnemy;
            Debug.Log(currentTarget
                ? $"{heroStats.name}: Found new target({currentTarget.heroStats.name})."
                : $"{heroStats.name}: No targets.");
        }

        private void AttackCurrentTarget()
        {
            movementController.SetTarget(currentTarget);
            if (Vector3.Distance(transform.position, currentTarget.transform.position) <= heroStats.attackRange)
            {
                movementController.Stop();
                Attack();
            }
            else
                movementController.Resume();
        }

        private void Attack()
        {
            if (!canAttack)
                return;
            Debug.Log($"{heroStats.name}: Target in range. Attack.");
            currentTarget.TakeDamage(heroStats.baseDamage);
            WaitCoroutine();
        }

        private void TakeDamage(int amount)
        {
            if (amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            Debug.Log($"{heroStats.name}: Taking damage: {amount}dmg");
            health -= amount;
            healthBar.SetHealth(health);
            healthChanged?.Invoke();
            if (isDead)
            {
                Debug.Log($"{heroStats.name} dead.");
                EventController.UnitDied.Invoke(myTeam, this);
            }
        }

        private async void WaitCoroutine()
        {
            canAttack = false;
            await Task.Delay(Mathf.RoundToInt(attackDeltaTime * 1000));
            canAttack = true;
        }

        private void Update()
        {
            if (currentTarget == null || currentTarget.isDead)
                FindTarget();

            if (currentTarget == null)
                return;

            AttackCurrentTarget();
        }
    }
}