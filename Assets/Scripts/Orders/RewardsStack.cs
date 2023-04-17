using System.Collections.Generic;
using Merge;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Orders
{
    public class RewardsStack : MonoBehaviour
    {
        [SerializeField] private Image lastRewardImage;
        [SerializeField] private TMP_Text rewardsCountText;

        public static RewardsStack Instance { get; private set; }

        private Stack<MergeItemData> Rewards { get; } = new();

        public void AppendReward(MergeItemData reward)
        {
            Rewards.Push(reward);
            UpdateSprite();
            gameObject.SetActive(true);
        }

        public void ClaimReward()
        {
            var reward = Rewards.Pop();
            UpdateSprite();
        }

        private void UpdateSprite()
        {
            if (Rewards.Count > 0)
                lastRewardImage.sprite = Rewards.Peek().sprite;
        }

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            var rewardsCount = Rewards.Count;
            gameObject.SetActive(rewardsCount > 0);
            rewardsCountText.text = rewardsCount.ToString();
        }
    }
}
