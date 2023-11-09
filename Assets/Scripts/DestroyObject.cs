using NaughtyAttributes;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    [BoxGroup("Visual")][SerializeField] private Transform _fxDestruction;
    [BoxGroup("Visual")][SerializeField] private Transform _debris;
    [BoxGroup("Visual")][SerializeField] private bool _destroyItself = true;
    [BoxGroup("Visual")][SerializeField] private bool _addLoot = false;
    [BoxGroup("Visual")][SerializeField][ShowIf("_addLoot")] private CollectionObject _lootPrefab;
    [BoxGroup("Visual")][SerializeField][ShowIf("_addLoot")] private bool _customLootCount;
    [BoxGroup("Visual")][SerializeField][ShowIf("_customLootCount")] private int _lootCount;
    [BoxGroup("Visual")][SerializeField] private CollectionObject[] _loot;


    private void Start()
    {
        if (_addLoot)
        {
            int lootCount = (_customLootCount || !TryGetComponent<HPObject>(out HPObject hpobj)) ? _lootCount : hpobj.GetHP();
            for (int i = 0; i < lootCount; i++)
            {
                CollectionObject lootObj = Instantiate(_lootPrefab, transform.position, Quaternion.identity, transform);
                lootObj.gameObject.SetActive(false);
            }
        }
        _loot = GetComponentsInChildren<CollectionObject>(true);
    }

    public void Destroying()
    {
        if (_fxDestruction != null)
        {
            _fxDestruction.transform.SetParent(null);
            _fxDestruction.gameObject.SetActive(true);
        }
        if (_debris != null)
        {
            _debris.transform.SetParent(null);
            _debris.gameObject.SetActive(true);
        }
        if (_loot.Length > 0)
        {
            for (int i = 0; i < _loot.Length; i++)
            {
                if (_loot[i].gameObject)
                {
                    _loot[i].transform.SetParent(null);
                    _loot[i].gameObject.SetActive(true);
                    _loot[i].Collect();
                }
            }
        }
        if (_destroyItself)
        {
            Destroy(gameObject, 0.05f);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying)
            _loot = GetComponentsInChildren<CollectionObject>(true);
    }
#endif

}
