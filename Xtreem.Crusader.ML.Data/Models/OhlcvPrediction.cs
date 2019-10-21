﻿using JetBrains.Annotations;

namespace Xtreem.Crusader.ML.Data.Models
{
    public class OhlcvPrediction : OhlcvInput
    {
        [UsedImplicitly]
        public float OpenPrediction;

        [UsedImplicitly]
        public float ClosePrediction;

        [UsedImplicitly]
        public float HighPrediction;

        [UsedImplicitly]
        public float LowPrediction;

        [UsedImplicitly]
        public float VolumeFromPrediction;

        [UsedImplicitly]
        public float VolumeToPrediction;
    }
}
