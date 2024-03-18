using System;
using System.Collections.Generic;
using NAudio.Wave;
using System.Linq;

namespace MusicPlayerConsoleApp
{
    public delegate void MusicPlayerDelegate(string audioFilePath);

    class MusicPlayer
    {
        private List<string> playlist = new List<string>();

        // Display the menu options
        public void DisplayMenu()
        {
            Console.WriteLine("\n===== Music Player Menu =====");
            Console.WriteLine("1. Add song to playlist");
            Console.WriteLine("2. Delete song from playlist");
            Console.WriteLine("3. Play playlist");
            Console.WriteLine("4. Shuffle playlist");
            Console.WriteLine("5. Sort playlist");
            Console.WriteLine("6. Display playlist");
            Console.WriteLine("7. Exit");
        }

        // Add a song to the playlist
        public void AddSong(string audioFilePath)
        {
            playlist.Add(audioFilePath);
            Console.WriteLine($"{audioFilePath} added to the playlist.");
        }

        // Delete a song from the playlist
        public void DeleteSong()
        {
            if (playlist.Count == 0)
            {
                Console.WriteLine("Playlist is empty.");
                return;
            }

            Console.WriteLine("\nCurrent Playlist:");
            DisplayPlaylist();

            Console.Write("Enter the index of the song to delete: ");
            if (int.TryParse(Console.ReadLine(), out int index) && index >= 0 && index < playlist.Count)
            {
                string deletedSong = playlist[index];
                playlist.RemoveAt(index);
                Console.WriteLine($"{deletedSong} deleted from the playlist.");
            }
            else
            {
                Console.WriteLine("Invalid index.");
            }
        }

        // Play all songs in the playlist using delegate
        public void PlayPlaylist(MusicPlayerDelegate playSongDelegate)
        {
            if (playlist.Count == 0)
            {
                Console.WriteLine("Playlist is empty.");
                return;
            }

            Console.WriteLine("\nPlaying Playlist:");
            foreach (string audioFilePath in playlist)
            {
                playSongDelegate(audioFilePath);
            }
        }

        // Shuffle the songs in the playlist
        public void ShufflePlaylist()
        {
            Random random = new Random();
            playlist = playlist.OrderBy(x => random.Next()).ToList();
            Console.WriteLine("Playlist shuffled.");
        }

        // Sort the songs in the playlist
        public void SortPlaylist()
        {
            playlist.Sort();
            Console.WriteLine("Playlist sorted.");
        }

        // Display all songs in the playlist
        public void DisplayPlaylist()
        {
            if (playlist.Count == 0)
            {
                Console.WriteLine("Playlist is empty.");
            }
            else
            {
                for (int i = 0; i < playlist.Count; i++)
                {
                    Console.WriteLine($"{i}. {playlist[i]}");
                }
            }
        }

        // Main method to run the music player
        public void Run()
        {
            while (true)
            {
                DisplayMenu();
                Console.Write("\nEnter your choice (1-7): ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Enter the File path: ");
                        string audioFilePath = Console.ReadLine();
                        AddSong(audioFilePath);
                        break;
                    case "2":
                        DeleteSong();
                        break;
                    case "3":
                        // Use delegate to play playlist
                        PlayPlaylist(PlaySong);
                        break;
                    case "4":
                        ShufflePlaylist();
                        break;
                    case "5":
                        SortPlaylist();
                        break;
                    case "6":
                        DisplayPlaylist();
                        break;
                    case "7":
                        Console.WriteLine("Exiting music player. Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please enter a number between 1 and 7.");
                        break;
                }
            }
        }

        // Delegate method to play a song
        private void PlaySong(string audioFilePath)
        {
            Console.WriteLine($"Now playing: {audioFilePath}");
            using (AudioFileReader audioFileReader = new AudioFileReader(audioFilePath))
            {
                using (WaveOutEvent waveOutEvent = new WaveOutEvent())
                {
                    waveOutEvent.Init(audioFileReader);
                    waveOutEvent.Play();
                    Console.WriteLine("Press any key to stop...");
                    Console.ReadKey();
                    waveOutEvent.Stop();
                }
            }
        }
    }
}