using Web.Models.Newsletter;
using Web.Models.User;
using Web.Models.Exercise;
using Web.Extensions;

namespace Web.Test.Tests.Real;

[TestClass]
public class TestNewsletterTypeGroups : RealDatabase
{
    private static readonly MovementPattern AllMovementPatterns
        = (MovementPattern)(1 << 0)
        | (MovementPattern)(1 << 1)
        | (MovementPattern)(1 << 2)
        | (MovementPattern)(1 << 3)
        | (MovementPattern)(1 << 4)
        | (MovementPattern)(1 << 5)
        | (MovementPattern)(1 << 6)
        | (MovementPattern)(1 << 7)
        | (MovementPattern)(1 << 8);

    #region Movement Patterns

    [TestMethod]
    public void NewsletterTypeGroups_MaintainFullBody_HasAllMovementPatterns()
    {
        var groups = new NewsletterTypeGroups(StrengtheningPreference.Maintain, Frequency.FullBody2Day);
        // Works all major movement patterns each week
        Assert.IsTrue(groups.Aggregate((MovementPattern)0, (curr, n) => curr | n.MovementPatterns).HasFlag(AllMovementPatterns));
    }

    [TestMethod]
    public void NewsletterTypeGroups_MaintainUpperLower2Day_HasAllMovementPatterns()
    {
        var groups = new NewsletterTypeGroups(StrengtheningPreference.Maintain, Frequency.UpperLowerBodySplit2Day);
        // Works all major movement patterns each week
        Assert.IsTrue(groups.Aggregate((MovementPattern)0, (curr, n) => curr | n.MovementPatterns).HasFlag(AllMovementPatterns));
    }

    [TestMethod]
    public void NewsletterTypeGroups_MaintainUpperLower4Day_HasAllMovementPatterns()
    {
        var groups = new NewsletterTypeGroups(StrengtheningPreference.Maintain, Frequency.UpperLowerBodySplit4Day);
        // Works all major movement patterns each week
        Assert.IsTrue(groups.Aggregate((MovementPattern)0, (curr, n) => curr | n.MovementPatterns).HasFlag(AllMovementPatterns));
    }

    [TestMethod]
    public void NewsletterTypeGroups_MaintainUpperPushPullLower_HasAllMovementPatterns()
    {
        var groups = new NewsletterTypeGroups(StrengtheningPreference.Maintain, Frequency.PushPullLeg3Day);
        // Works all major movement patterns each week
        Assert.IsTrue(groups.Aggregate((MovementPattern)0, (curr, n) => curr | n.MovementPatterns).HasFlag(AllMovementPatterns));
    }

    [TestMethod]
    public void NewsletterTypeGroups_MaintainUpperPushPullLower6_HasAllMovementPatterns()
    {
        var groups = new NewsletterTypeGroups(StrengtheningPreference.Maintain, Frequency.PushPullLeg6Day);
        // Works all major movement patterns each week
        Assert.IsTrue(groups.Aggregate((MovementPattern)0, (curr, n) => curr | n.MovementPatterns).HasFlag(AllMovementPatterns));
    }

    #endregion
    
    #region Muscles

    [TestMethod]
    public void NewsletterTypeGroups_MaintainFullBody_HasAllMuscles()
    {
        var groups = new NewsletterTypeGroups(StrengtheningPreference.Maintain, Frequency.FullBody2Day);
        // Works every muscle group except core each week
        Assert.IsTrue(groups.Aggregate((MuscleGroups)0, (curr, n) => curr | n.MuscleGroups).HasFlag(MuscleGroups.All.UnsetFlag32(MuscleGroups.Core)));
        // Does not work core, core is included in every workout regardless of the frequency
        Assert.IsFalse(groups.Aggregate((MuscleGroups)0, (curr, n) => curr | n.MuscleGroups).HasAnyFlag32(MuscleGroups.Core));
    }

    [TestMethod]
    public void NewsletterTypeGroups_MaintainUpperLower2Day_HasAllMuscles()
    {
        var groups = new NewsletterTypeGroups(StrengtheningPreference.Maintain, Frequency.UpperLowerBodySplit2Day);
        // Works every muscle group except core each week
        Assert.IsTrue(groups.Aggregate((MuscleGroups)0, (curr, n) => curr | n.MuscleGroups).HasFlag(MuscleGroups.All.UnsetFlag32(MuscleGroups.Core)));
        // Does not work core, core is included in every workout regardless of the frequency
        Assert.IsFalse(groups.Aggregate((MuscleGroups)0, (curr, n) => curr | n.MuscleGroups).HasAnyFlag32(MuscleGroups.Core));
    }

    [TestMethod]
    public void NewsletterTypeGroups_MaintainUpperLower4Day_HasAllMuscles()
    {
        var groups = new NewsletterTypeGroups(StrengtheningPreference.Maintain, Frequency.UpperLowerBodySplit4Day);
        // Works every muscle group except core each week
        Assert.IsTrue(groups.Aggregate((MuscleGroups)0, (curr, n) => curr | n.MuscleGroups).HasFlag(MuscleGroups.All.UnsetFlag32(MuscleGroups.Core)));
        // Does not work core, core is included in every workout regardless of the frequency
        Assert.IsFalse(groups.Aggregate((MuscleGroups)0, (curr, n) => curr | n.MuscleGroups).HasAnyFlag32(MuscleGroups.Core));
    }

    [TestMethod]
    public void NewsletterTypeGroups_MaintainUpperPushPullLower_HasAllMuscles()
    {
        var groups = new NewsletterTypeGroups(StrengtheningPreference.Maintain, Frequency.PushPullLeg3Day);
        // Works every muscle group except core each week
        Assert.IsTrue(groups.Aggregate((MuscleGroups)0, (curr, n) => curr | n.MuscleGroups).HasFlag(MuscleGroups.All.UnsetFlag32(MuscleGroups.Core)));
        // Does not work core, core is included in every workout regardless of the frequency
        Assert.IsFalse(groups.Aggregate((MuscleGroups)0, (curr, n) => curr | n.MuscleGroups).HasAnyFlag32(MuscleGroups.Core));
    }

    [TestMethod]
    public void NewsletterTypeGroups_MaintainUpperPushPullLower6_HasAllMuscles()
    {
        var groups = new NewsletterTypeGroups(StrengtheningPreference.Maintain, Frequency.PushPullLeg6Day);
        // Works every muscle group except core each week
        Assert.IsTrue(groups.Aggregate((MuscleGroups)0, (curr, n) => curr | n.MuscleGroups).HasFlag(MuscleGroups.All.UnsetFlag32(MuscleGroups.Core)));
        // Does not work core, core is included in every workout regardless of the frequency
        Assert.IsFalse(groups.Aggregate((MuscleGroups)0, (curr, n) => curr | n.MuscleGroups).HasAnyFlag32(MuscleGroups.Core));
    }

    #endregion
}