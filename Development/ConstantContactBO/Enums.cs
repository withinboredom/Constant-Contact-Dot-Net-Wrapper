namespace ConstantContactBO
{
    /// <summary>
    /// Email Campaign Status
    /// </summary>
    public enum CampaignState
    {
        ///<summary>
        ///Draft State
        ///</summary>
        Draft,
        ///<summary>
        ///Running State
        ///</summary>
        Running,
        ///<summary>
        ///Scheduled State
        ///</summary>
        Scheduled,
        ///<summary>
        ///ArchivePending State
        ///</summary>
        ArchivePending,
        ///<summary>
        ///Archived State
        ///</summary>
        Archived,
        ///<summary>
        ///ClosePending State
        ///</summary>
        ClosePending,
        ///<summary>
        ///Closed State
        ///</summary>
        Closed,
        ///<summary>
        ///Sent State
        ///</summary>
        Sent
    } ;

    /// <summary>
    /// Email Campaign Type
    /// </summary>
    public enum CampaignType
    {
        ///<summary>
        ///STOCK type
        ///</summary>
        STOCK,
        ///<summary>
        ///CUSTOM type
        ///</summary>
        CUSTOM
    } ;
}