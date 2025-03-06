using System;
using System.Collections.ObjectModel;
using PiCalculator.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace PiCalculator.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private int inputSampleSize = 10000;

        public ObservableCollection<PiMission> Missions { get; } = new();

        [RelayCommand]
        private async Task AddMissionAsync()
        {
            if (InputSampleSize <= 0) return;
            if (InputSampleSize > int.MaxValue) InputSampleSize = int.MaxValue;
            if (Missions.Any(m => m.SampleSize == InputSampleSize)) return;

            var mission = new PiMission(InputSampleSize);
            Missions.Add(mission);
            await mission.CalculatePiAsync();
        }

        public MainViewModel()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(1000);
                    foreach (var mission in Missions)
                    {
                        await mission.CalculatePiAsync();
                    }
                }
            });
        }
    }
}
