﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinerFettle.Web.Models.Exercise
{
    [Table(nameof(Exercise)), Comment("Exercises listed on the website")]
    public class Exercise
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Code { get; set; } = null!;

        [Required]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Primary muscles worked by the exercise
        /// </summary>
        [Required]
        public MuscleGroups Muscles { get; set; }

        [Required]
        public ExerciseType ExerciseType { get; set; }

        public IList<Variation> Variations { get; set; } = default!;
    }
}
