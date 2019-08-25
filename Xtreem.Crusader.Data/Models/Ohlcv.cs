namespace Xtreem.Crusader.Data.Models
{
    public class Ohlcv
    {
        public string Base { get; set; }

        public string Quote { get; set; }

        public string Resolution { get; set; }

        public long Time { get; set; }

        public float Open { get; set; }

        public float Close { get; set; }

        public float High { get; set; }

        public float Low { get; set; }

        public float VolumeFrom { get; set; }

        public float VolumeTo { get; set; }
    }
}
