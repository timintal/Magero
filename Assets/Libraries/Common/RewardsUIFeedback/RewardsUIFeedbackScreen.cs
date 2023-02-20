using System.Collections.Generic;
using UIFramework;
using UnityEngine;

public class RewardsUIFeedbackScreen : UIScreen
{
    [SerializeField] private List<RewardUIPrefab> _rewardUIPrefabs;

    Dictionary<UIFeedbackTarget, List<GameObject>> _rewardPools = new();
    Dictionary<UIFeedbackTarget, GameObject> _rewardsPrefabs = new();

    void Start()
    {
        foreach (var rewardUIPrefab in _rewardUIPrefabs)
        {
            _rewardPools.Add(rewardUIPrefab.Target, new List<GameObject>());
            _rewardsPrefabs.Add(rewardUIPrefab.Target, rewardUIPrefab.Prefab);
        }
    }
    
    public GameObject GetRewardPrefab(UIFeedbackTarget target, Vector3 position)
    {
        if (!_rewardPools.TryGetValue(target, out var prefabPool))
        {
            Debug.LogError($"Prefab pool for {target} not found");
            return null;
        }

        GameObject reward = null;

        if (prefabPool.Count > 0)
        {
            reward = prefabPool[0];
            reward.SetActive(true);
            prefabPool.RemoveAt(0);
        }
        else
        {
            if (!_rewardsPrefabs.TryGetValue(target, out var prefab))
            {
                Debug.LogError($"Prefab for {target} not found");
                return null;
            }
            reward = Instantiate(prefab, transform);
        }

        reward.transform.position = position;
        
        return reward;
    }
    
    public void ReturnRewardPrefab(UIFeedbackTarget target, GameObject reward)
    {
        if (!_rewardPools.TryGetValue(target, out var prefabPool))
        {
            Debug.LogError($"Prefab pool for {target} not found");
            return;
        }
        
        reward.SetActive(false);
        prefabPool.Add(reward);
    }
    
}