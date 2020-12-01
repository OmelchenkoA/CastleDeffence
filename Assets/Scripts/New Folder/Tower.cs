using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Spawnable
{
    private Animator animator;

    public TowerData towerData;

    public int currentLevel;
    private TowerLevel towerLevel;
    private PlacementTile placementTile;

	public PlacementTile PlacementTile { get => placementTile; set => placementTile = value; }

	private void Awake()
    {
        type = Spawnable.Type.Tower;

        //find references to components
        animator = GetComponent<Animator>();
        
    }



    public void Init(TowerData tData)
    {
        state = States.Idle;
        towerData = tData;
        faction = tData.faction;
        targetType = tData.targetType;


        SetLevel(0);
    }


    private void SetLevel(int level)
    {

        if (level >= 0 && level < towerData.towerLevels.Length)
        {

            if (!CurrencyManager.instance.IsEnoughMoneyFor(towerData.towerLevels[currentLevel].upgradeCost))
            {
                Debug.Log("Not enough money to upgrade tower!");
                return;
            }

            health = towerData.towerLevels[currentLevel].hitPoints;
            attackRange = towerData.towerLevels[currentLevel].attackRange;
            attackRatio = towerData.towerLevels[currentLevel].attackRatio;
            damage = towerData.towerLevels[currentLevel].damagePerAttack;

            currentLevel = level;
            if (towerLevel != null)
                GameObject.Destroy(towerLevel.gameObject);
            towerLevel = Instantiate(towerData.towerLevels[currentLevel].prefab, transform).GetComponent<TowerLevel>();
            towerLevel.ParentTower = this;

            projectileSpawnPoints = towerLevel.projectileSpawnPoints;
            gun = towerLevel.gun;

            CurrencyManager.instance.DecreaseCurrency(towerData.towerLevels[currentLevel].upgradeCost);
        }
    }

    public void UpgradeTower()
    {
        SetLevel(currentLevel + 1);
    }




    public override void Seek()
    {
        if (target == null)
            return;

        base.Seek();

 
    }

    //Unit has gotten to its target. This function puts it in "attack mode", but doesn't delive any damage (see DealBlow)
    public override void StartAttack()
    {
        base.StartAttack();
    }

    //Starts the attack animation, and is repeated according to the Unit's attackRatio
    public override void DealBlow()
    {
        base.DealBlow();

        //animator.SetTrigger("Attack");
        //turn towards the target
        FireProjectile();
    }

    public override void Stop()
    {
        base.Stop();

    }

    protected override void Die()
    {
        base.Die();
    }




}
