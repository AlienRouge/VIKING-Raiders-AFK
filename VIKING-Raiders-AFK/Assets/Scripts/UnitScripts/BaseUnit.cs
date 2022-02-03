using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace UnitScripts
{
    public class BaseUnit : MonoBehaviour
    {
        public Hero heroStats { get; private set; }
        [SerializeField] private BattleManager.Team myTeam;

        [SerializeField] private int health;
        [SerializeField] private BaseUnit currentTarget;
        [SerializeField] private Vector3 spawnPosition;
        private bool isDead => health <= 0;
        private float attackDeltaTime;
        private bool canAttack = true;

        private SpriteRenderer spriteRenderer;
        private MovementController movementController;
        private HealthBar healthBar;

        private void Awake()
        {
            movementController = GetComponent<MovementController>();
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            healthBar = GetComponentInChildren<HealthBar>();
        }

        public void Init(BattleManager.Team team, Transform spawnPos, Hero hero)
        {
            heroStats = hero;
            health = heroStats.baseHealth;
            attackDeltaTime = 1 / heroStats.attackSpeed;
            myTeam = team;
            spawnPosition = spawnPos.position;
            healthBar.SetMaxHealth(heroStats.baseHealth);

            transform.position = spawnPosition;
            spriteRenderer.color = (myTeam == BattleManager.Team.Team1)
                ? new Color(53,72,231)
                : Color.red;
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
            StartCoroutine(WaitCoroutine());
        }

        private void TakeDamage(int amount)
        {
            if (amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            Debug.Log($"{heroStats.name}: Taking damage: {amount}dmg");
            health -= amount;
            healthBar.SetHealth(health);
            if (isDead)
            {
                Debug.Log($"{heroStats.name} dead.");
                BattleManager.Instance.UnitDied(myTeam, this);
            }
        }

        private IEnumerator WaitCoroutine()
        {
            canAttack = false;
            yield return null;
            yield return new WaitForSeconds(attackDeltaTime);
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