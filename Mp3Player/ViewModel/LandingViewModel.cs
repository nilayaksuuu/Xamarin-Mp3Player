using Mp3Player.Model;
using Mp3Player.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Mp3Player.ViewModel
{
    public class LandingViewModel : BaseViewModel
    {
        public LandingViewModel()
        {
            musicList = GetMusics();
            recentMusic = musicList.FirstOrDefault(x => x.IsRecent);
        }

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

        private Music recentMusic;

        public Music RecentMusic
        {
            get => recentMusic;
            set
            {
                recentMusic = value;
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

        public ICommand SelectionCommand => new Command(PlayMusic);

        private void PlayMusic()
        {
            if (selectedMusic != null)
            {
                PlayerViewModel viewModel = new PlayerViewModel(selectedMusic, musicList);
                PlayerPage playerPage = new PlayerPage { BindingContext = viewModel };

                NavigationPage navigation = Application.Current.MainPage as NavigationPage;
                _ = navigation.PushAsync(playerPage, true);
            }
        }

        private ObservableCollection<Music> GetMusics()
        {
            return new ObservableCollection<Music>
            {
                new Music { Title = "Crazy Love", Artist = "Aron Neville", Url = "https://ia800605.us.archive.org/32/items/Mp3Playlist_555/AaronNeville-CrazyLove.mp3", CoverImage = "https://static.wikia.nocookie.net/aesthetics/images/5/59/7F907D80-0841-46D0-9585-C93BBF16CB2F.jpeg/revision/latest?cb=20201020194420", IsRecent = true},
                new Music { Title = "House At Pooh Corner", Artist = "Kenny Loggins", Url = "https://ia800605.us.archive.org/32/items/Mp3Playlist_555/KennyLoggins-04HouseAtPoohCorner.mp3", CoverImage = "https://static.wikia.nocookie.net/aesthetics/images/b/b3/6BCB3EE3-E11B-4A87-B9A8-53B1915C1472.jpeg/revision/latest?cb=20201020193901", IsRecent = true},
                new Music { Title = "Home Acoustic", Artist = "Density & Time", Url = "https://ia800605.us.archive.org/32/items/Mp3Playlist_555/Daughtry-Homeacoustic.mp3"},
                new Music { Title = "Change In My Life", Artist = "John Pagano", Url = "https://ia800605.us.archive.org/32/items/Mp3Playlist_555/JohnPagano-changeInMyLife.mp3"},
                new Music { Title = "Walang Hanggang Paalam", Artist = "Joey Ayala", Url = "https://ia800605.us.archive.org/32/items/Mp3Playlist_555/JoeyAyala-WalangHanggangPaalam.mp3"},
                new Music { Title = "I Will Be Here", Artist = "Kris Aquino", Url = "https://ia800605.us.archive.org/32/items/Mp3Playlist_555/KrisAquino-SongsOfLoveAndHealing2007-11-IWillBeHere.mp3"}
            };
        }
    }
}
