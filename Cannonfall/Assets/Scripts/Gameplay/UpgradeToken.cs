using UnityEngine;

public class UpgradeToken : MonoBehaviour
{
    [SerializeField] private int upgradeIndex;

    public int getUpgrade() { return upgradeIndex; }
}