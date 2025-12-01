using System.Text.Json.Serialization;

namespace ApplicationHub.Modules.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ApplicationFormStatusEnum
{
    ApplicationReceivedStage = 0,
    UnderReviewStage = 1,
    ShortListedStage = 2,
    AssessmentStage = 3,
    InterviewStage = 4,
    InReviewStage = 5,
    ReferenceCheckStage = 6,
    OfferStage = 7,
    HiredStage = 8,
    ClosedStage = 9,
    WithdrawnStage = 10,
    OnHoldStage = 11,
    PositionFilledStage = 12
}