using Core.Game;
using Core.Location;
using UnityEngine;

namespace Core.PlayerScripts.Weapon
{
    public abstract class Weapon : MonoBehaviour
    {
        protected int weaponID;
        protected Item currentItem;
        protected GameObject cacheHolder;
        protected WeaponOptionsItem options;
        protected new Transform camera;

        private float decalTime;
        private LayerMask decalLayer;
        protected LayerMask customMask = -1;
        protected bool isLayerMaskChanged;
        protected Vector3 pointOffcet;
        
        
        protected float cooldown;
        protected float damage;
        
        protected float time;
        
        public void Init(int shootKey, Item current, WeaponOptionsItem opt)
        {
            currentItem = current;
            weaponID = shootKey;
            options = opt;
            decalLayer = LayerMask.GetMask("Decals");
            camera = transform;

            cacheHolder = SpawnCacheHolder();
        
            if (Player.inst != null)
            {
                Player.inst.attack.OnShoot += CheckIsCurrentWeapon;
                Player.inst.attack.OnHold += OnHold;
                Player.inst.attack.OnHold += OnHoldDown;
            }

            damage = (float)currentItem.GetKeyPair(KeyPairValue.Damage);
            cooldown = (float)currentItem.GetKeyPair(KeyPairValue.Cooldown);
            
            
            InitData();
        }

        public float GetReload()
        {
            if (cooldown == -1)
            {
                return 1;
            }
            return Mathf.Clamp01(time/cooldown);
        }

        public float GetDistance()
        {
            return options.maxDistance;
        }
    
        public void SetCustomMask(LayerMask mask)
        {
            customMask = mask;
            isLayerMaskChanged = true;
        }

        public void SetOffcet(Vector3 newOffcet)
        {
            pointOffcet = newOffcet;
        }
        public void SetCustomCamera(Transform newCamera)
        {
            camera = newCamera;
        }
        protected virtual void InitData(){}
    
    
        private GameObject SpawnCacheHolder()
        {
            var holder = new GameObject("SlotData");
            holder.transform.parent = transform;
            holder.transform.localPosition = Vector3.zero;
            holder.transform.localEulerAngles = Vector3.zero;
            holder.layer = LayerMask.NameToLayer("Main");
            return holder;
        }
    
        public void CheckIsCurrentWeapon(int shootKey)
        {
            if (shootKey == weaponID)
            {
                Attack();
            }
        }

        public void OnHold(int shootKey)
        {
            decalTime += Time.deltaTime;
            if (shootKey == weaponID)
            {
                NotAttack();
            }
        } 
        public void OnHoldDown(int shootKey)
        {
            if (shootKey == weaponID)
            {
                AttackDown();
            }
        }
    

        protected virtual void NotAttack()
        {
        
        }
        protected virtual void AttackDown()
        {
        }
        protected virtual void Attack()
        {
        }

        protected virtual void Reload()
        {
        
        }

        protected virtual void ClearData()
        {
        
        }

        protected Event<RaycastHit> OnSpawnDecal = new Event<RaycastHit>();
        protected void SpawnDecal(GameObject decal, Vector3 start, Vector3 dir, RaycastHit initHit)
        {
            if (decalTime >= 1/5f)
            {
                if (initHit.transform.GetComponent<WorldDrop>() == null)
                {
                    bool addToDecal = false;
                    if (Physics.Raycast(start, dir, out RaycastHit hit, options.maxDistance, decalLayer))
                    {
                        var findedDecal = hit.collider.GetComponent<Decal>();
                        if (findedDecal)
                        {
                            findedDecal.AddToOpacity();
                            OnSpawnDecal.Run(initHit);
                            addToDecal = true;
                        }
                    }

                    if (!addToDecal)
                    {
                        var d = Instantiate(decal, initHit.point, Quaternion.identity);
                        d.transform.rotation = Quaternion.FromToRotation(Vector3.forward, initHit.normal);
                        d.transform.localRotation *= Quaternion.Euler(90, 0, 0);
                        d.transform.parent = initHit.transform;
                        OnSpawnDecal.Run(initHit);
                    }
                }

                decalTime = 0;
            }
        }
    
        private void OnDestroy()
        {
            Destroy(cacheHolder.gameObject);
            ClearData();
        }
        public Item Current()
        {
            return currentItem;
        }
        public int CurrentID()
        {
            return weaponID;
        }
    }
}
