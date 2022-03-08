using MediaManager;
using Mp3Player.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Mp3Player.ViewModel
{
    public class PlayerViewModel : BaseViewModel
    {

        public PlayerViewModel(Music selectedMusic, ObservableCollection<Music> musicList)
        {
            this.selectedMusic = selectedMusic;
            this.musicList = musicList;
            PlayMusic(selectedMusic);
        }

        #region Properties
        private ObservableCollection<Music> musicList;
        public ObservableCollection<Music> MusicList
        {
            get => musicList;
            set
            {
                musicList = value;
                OnPropertyChanged();
            }
        }
        private Music selectedMusic;
        public Music SelectedMusic
        {
            get => selectedMusic;
            set
            {
                selectedMusic = value;
                OnPropertyChanged();
            }
        }

        private TimeSpan duration;
        public TimeSpan Duration
        {
            get => duration;
            set
            {
                duration = value;
                OnPropertyChanged();
            }
        }
        private TimeSpan position;
        public TimeSpan Position
        {
            get => position;
            set
            {
                position = value;
                OnPropertyChanged();
            }
        }

        private double maximum = 100f;
        public double Maximum
        {
            get => maximum;
            set
            {
                if (value > 0)
                {
                    maximum = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool isPlaying;
        public bool IsPlaying
        {
            get => isPlaying;
            set
            {
                isPlaying = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PlayIcon));
            }
        }

        public string PlayIcon { get => isPlaying ? "pause.png" : "play.png"; }

        #endregion

        public ICommand PlayCommand => new Command(Play);
        public ICommand ChangeCommand => new Command(ChangeMusic);
        public ICommand BackCommand => new Command(() => Application.Current.MainPage.Navigation.PopAsync());
        public ICommand ShareCommand => new Command(() => Share.RequestAsync(selectedMusic.Url, selectedMusic.Title));

        private async void Play()
        {
            if (isPlaying)
            {
                await CrossMediaManager.Current.Pause();
                IsPlaying = false; ;
            }
            else
            {
                await CrossMediaManager.Current.Play();
                IsPlaying = true; ;
            }
        }

        private void ChangeMusic(object obj)
        {
            if ((string)obj == "P")
            {
                PreviousMusic();
            }
            else if ((string)obj == "N")
            {
                NextMusic();
            }
        }
        private void NextMusic()
        {
            int currentIndex = musicList.IndexOf(selectedMusic);
            if (currentIndex < musicList.Count - 1)
            {
                SelectedMusic = musicList[currentIndex + 1];
                PlayMusic(selectedMusic);
            }
        }
        private void PreviousMusic()
        {
            int currentIndex = musicList.IndexOf(selectedMusic);
            if (currentIndex > 0)
            {
                SelectedMusic = musicList[currentIndex - 1];
                PlayMusic(selectedMusic);
            }
        }

        private async void PlayMusic(Music music)
        {
            IMediaManager mediaInfo = CrossMediaManager.Current;
            _ = await mediaInfo.Play(music?.Url);
            IsPlaying = true;

            mediaInfo.MediaItemFinished += (sender, args) =>
            {
                IsPlaying = false;
                NextMusic();
            };

            Device.StartTimer(TimeSpan.FromMilliseconds(500), () =>
            {
                Duration = mediaInfo.Duration;
                Maximum = duration.TotalSeconds;
                Position = mediaInfo.Position;
                return true;
            });
        }

       
    }
}
