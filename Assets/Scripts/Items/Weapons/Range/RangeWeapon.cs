using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeapon : Weapon
{
    public GameObject Bullet;
    public Transform bulletSpawn;
    public float WeaponDamageMultiplier = 1;
    public int AmmoID;
    //Simple shot on x axis
    public void shoot(bool firedByEnemy = false)
    {
        if (ReadyToAttack && AmmoCheck())
        {
            float direction = Owner.GetComponent<Character>().isFacingRight ? 1f : -1f;
            var bullet = Instantiate(Bullet, new Vector3(bulletSpawn.position.x , bulletSpawn.position.y, bulletSpawn.position.z), new Quaternion(bulletSpawn.rotation.x, bulletSpawn.rotation.y, Bullet.GetComponent<Bullet>().rotation, 90));
            bullet.GetComponent<Bullet>().WeaponDamageMultiplier = WeaponDamageMultiplier;
            if (!Owner.GetComponent<Character>().isFacingRight)
            {
                Vector3 theScale = bullet.transform.localScale;
                theScale.x *= -1;
                bullet.transform.localScale = theScale;
            }
            bullet.GetComponent<Bullet>().WeaponDamageMultiplier = WeaponDamageMultiplier;
            IgnoreCollisionWithOwner(bullet);
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector3(bullet.GetComponent<Bullet>().BulletSpeed * direction, 0, 0);
            ReadyToAttack = false;
            if (firedByEnemy)
            {
                bullet.GetComponent<Bullet>().IgnoreCollisionWithEnemies();
            }
            PlaySoundEffect();
            Invoke("WaitToShoot", attackCooldown);
        }
    }
    //Targeting shot
    public void shoot(GameObject target, bool firedByEnemy = false)
    {
        if (ReadyToAttack)
        {
            float direction = Owner.GetComponent<Character>().isFacingRight ? 1f : -1f;
            float endAngle = Mathf.Atan((bulletSpawn.position.y - target.transform.position.y) / (bulletSpawn.position.x - target.transform.position.x)) / Mathf.PI * 180+180;
            if (bulletSpawn.position.x < target.transform.position.x)
            {
                endAngle -= 180;
            }
            var bullet = Instantiate(Bullet, new Vector3(bulletSpawn.position.x, bulletSpawn.position.y, bulletSpawn.position.z), Quaternion.Euler(0, 0, endAngle));
            IgnoreCollisionWithOwner(bullet);
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector3(bullet.GetComponent<Bullet>().BulletSpeed * Mathf.Sin((endAngle+90) / 180 * Mathf.PI),
                -bullet.GetComponent<Bullet>().BulletSpeed * Mathf.Cos((endAngle+90) / 180 * Mathf.PI));
            bullet.GetComponent<Bullet>().WeaponDamageMultiplier = WeaponDamageMultiplier;
            ReadyToAttack = false;
            if(firedByEnemy)
            {
                bullet.GetComponent<Bullet>().IgnoreCollisionWithEnemies();
            }
            PlaySoundEffect();
            Invoke("WaitToShoot", attackCooldown);
        }
    }
    void WaitToShoot()
    {
        ReadyToAttack = true;
    }

    bool AmmoCheck()
    {
        if (Owner.GetComponent<PlayerController>().itemEQ[AmmoID] > 0)
        {
            Owner.GetComponent<PlayerController>().itemEQ[AmmoID]--;
            return true;
        }
        return false;
    }
}
