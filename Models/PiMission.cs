using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace PiCalculator.Models
{
    public class PiMission : INotifyPropertyChanged
    {
        private long sampleSize;
        private string lastCalculatedTime;
        private double piValue;

        public long SampleSize
        {
            get => sampleSize;
            set => SetProperty(ref sampleSize, value);
        }

        public string LastCalculatedTime
        {
            get => lastCalculatedTime;
            private set => SetProperty(ref lastCalculatedTime, value);
        }

        public double PiValue
        {
            get => piValue;
            set => SetProperty(ref piValue, value);
        }

        private static readonly ThreadLocal<Random> random = new(() => new Random());

        public PiMission(long sampleSize)
        {
            this.SampleSize = sampleSize;
        }

        public async Task CalculatePiAsync()
        {
            if (PiValue > 0) return;

            Stopwatch sw = Stopwatch.StartNew();
            int insideCircle = 0;

            await Task.Run(() =>
            {
                Parallel.For(0, SampleSize, () => 0, (i, state, local) =>
                {
                    double x = random.Value.NextDouble();
                    double y = random.Value.NextDouble();
                    if (x * x + y * y <= 1) local++;
                    return local;
                },
                local => { lock (random) insideCircle += local; });
            });

            PiValue = (double)insideCircle / SampleSize * 4;
            LastCalculatedTime = DateTime.Now.ToString("HH:mm:ss");
            sw.Stop();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
