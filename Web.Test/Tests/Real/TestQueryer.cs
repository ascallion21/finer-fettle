﻿using Core.Models.Exercise;

namespace Web.Test.Tests.Real;


[TestClass]
public class TestQueryer : RealDatabase
{
    [TestMethod]
    public async Task ExerciseQueryer_WithExerciseVariationType_ReturnsCorrectExerciseVariationType()
    {
        var results = await new QueryBuilder(Context)
            .WithExerciseFocus(ExerciseFocus.Strength)
            .Build()
            .Query();

        Assert.IsTrue(results.All(vm => vm.Variation.ExerciseFocus.HasFlag(ExerciseFocus.Strength)));
    }
}