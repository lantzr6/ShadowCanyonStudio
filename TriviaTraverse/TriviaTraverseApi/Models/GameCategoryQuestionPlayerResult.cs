//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TriviaTraverseApi.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class GameCategoryQuestionPlayerResult
    {
        public int GameCategoryQuestionPlayerResultId { get; set; }
        public int GameCategoryQuestionId { get; set; }
        public int GamePlayerId { get; set; }
        public string PlayerAnswerText { get; set; }
        public Nullable<bool> IsCorrect { get; set; }
        public Nullable<int> PointsRewarded { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public System.DateTime UpdatedAt { get; set; }
        public bool Deleted { get; set; }
    }
}