using System;
namespace MySP2010Utilities
{
    public interface IContentOrganizerConditionalData
    {
        string ConditionFieldID { get; set; }
        string ConditionFieldInternalName { get; set; }
        string ConditionFieldTitle { get; set; }
        string ConditionOperator { get; set; }
        string ConditionValue { get; set; }
    }
}
