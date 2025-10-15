namespace Common.Enumerations.Notification
{
    public enum NotificationType
    {
        /// <summary>Sent when user achieves their first account type</summary>
        FirstAccountTypeAchieved,

        /// <summary>Sent when user achieves a new account type</summary>
        UpgradeAccountType,

        /// <summary>Sent when PV (Point Value) points are added to user's account</summary>
        PvPointsAdded,

        /// <summary>Sent when team PV points are added (left/right team)</summary>
        TeamPvPoints,

        /// <summary>Sent when a new direct marketer registers under the user</summary>
        NewDirectMarketerRegistered,

        /// <summary>Sent when a new indirect marketer joins the network</summary>
        NewIndirectMarketerJoined,

        /// <summary>Sent when a marketer's account becomes active</summary>
        MarketerAccountActivated,

        /// <summary>Sent for direct referral commissions</summary>
        DirectCommission,

        /// <summary>Sent for recruiter bonuses</summary>
        RecruitmentCommission,

        /// <summary>Sent for binary tree matching bonuses</summary>
        BinaryCommission,

        /// <summary>Sent when user achieves a new rank/level</summary>
        RankPromotion,

        /// <summary>Sent for rank achievement bonuses</summary>
        RankBonus,

        /// <summary>Sent for level-based commission earnings</summary>
        LevelCommission,

        /// <summary>Sent with updates about binary commission status</summary>
        BinaryCommissionStatus,

        /// <summary>Sent with updates about level commission status</summary>
        LevelCommissionStatus,

        /// <summary>Sent with updates about rank bonus status</summary>
        RankBonusStatus,

        /// <summary>Sent when a new order is placed in the system</summary>
        NewOrderCreated,

        /// <summary>Sent when an order's status changes (shipped, completed, etc.)</summary>
        OrderStatusChanged,

        /// <summary>Sent when system settings are modified by administrators</summary>
        SystemSettingsUpdated
    }
}
