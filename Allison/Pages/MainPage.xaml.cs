using Microsoft.EntityFrameworkCore;
using System;
using Allison.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using System.Threading.Tasks;
using System.Linq;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Composition;
using Windows.UI.Xaml.Shapes;
using Allison.CustomControls;
using Windows.UI.ViewManagement;
using Windows.Foundation;
using System.Runtime.CompilerServices;
using Windows.UI.Core;
using Windows.System;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.Media.Capture;
using Windows.Media.SpeechRecognition;
using System.Net.Http;
using System.Text.RegularExpressions;
using MyToolkit.Multimedia;
using System.Web;
using Windows.UI.Xaml.Media.Imaging;
using System.IO;
using System.Net;
using System.Text;
using System.Data;
using HtmlAgilityPack;
using Windows.ApplicationModel.Resources.Core;
using Windows.Globalization;
using Windows.Media.SpeechSynthesis;

namespace Allison
{
    public partial class MainPage : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(T currentValue, T newValue, Action DoSet, [CallerMemberName] String property = null)
        {
            if (EqualityComparer<T>.Default.Equals(currentValue, newValue)) return false;
            DoSet.Invoke();
            RaisePropertyChanged(property);
            return true;
        }

        private async void RaisePropertyChanged(string name)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs(name));
                    }
                });
        }

        private void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        //private void OnPropertyChanged(string propertyName) =>
        //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private ObservableCollection<MessageBubble> _cache;

        public ObservableCollection<MessageBubble> Cache1
        {
            get { return _cache; }
            set
            {
                _cache = value;
                OnPropertyChanged(nameof(Cache1));
            }
        }

        public static string GetMessage;

        private bool mictosendicon = false;
        public bool MicToSendIcon
        {
            get => this.mictosendicon;
            set
            {
                SetProperty(this.mictosendicon, value, () => this.mictosendicon = value);
            }
        }

        private bool backtobottom = false;
        public bool BackToBottom
        {
            get => this.backtobottom;
            set
            {
                SetProperty(this.backtobottom, value, () => this.backtobottom = value);
            }
        }

        public static ListView GetMainListView;

        public static Popup GetPopup;

        public static Grid GetOverlayGrid;

        public static MainPage Current;

        public ScrollViewer ScrollViewer;

        private SpeechRecognizer speechRecognizer;

        private ResourceMap speechResourceMap;

        public string videoimage;

        public string newsimage;

        public string speechtext;

        public bool getbool;

        public bool IsVoiceActive;

        public int get1;

        public int get2 = 1;

        public int set1;

        public int set2 = 1;

        public MainPage()
        {
            this.InitializeComponent();
            //if (Cache1 != null) { if (Cache1.Count < 0) { backtobottom = false; } }
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
            Current = this;
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(410, 410));
            Cache1 = new ObservableCollection<MessageBubble>();
            GetMainListView = this.MainListView;
            GetPopup = ClearAllPopup;
            GetOverlayGrid = ClearAllPopupOverlay;
            SpeechTextBox.IsEnabled = false;
            IsVoiceActive = false;
            getbool = false;
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            bool permissionGained = await AudioCapturePermissions.RequestMicrophonePermission();

            if (permissionGained)
            {
                speechResourceMap = ResourceManager.Current.MainResourceMap.GetSubtree("LocalizationSpeechResources");

                await InitializeRecognizer(SpeechRecognizer.SystemSpeechLanguage);
            }
        }

        protected async override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (speechRecognizer != null)
            {
                await this.speechRecognizer.ContinuousRecognitionSession.CancelAsync();

                speechRecognizer.StateChanged -= SpeechRecognizer_StateChanged;
                speechRecognizer.ContinuousRecognitionSession.Completed -= ContinuousRecognitionSession_Completed;
                speechRecognizer.ContinuousRecognitionSession.ResultGenerated -= ContinuousRecognitionSession_ResultGenerated;
                speechRecognizer.HypothesisGenerated -= SpeechRecognizer_HypothesisGenerated;

                this.speechRecognizer.Dispose();
                this.speechRecognizer = null;
            }
        }

        private void OnScrollViewerViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            try
            {
                ScrollViewer sv = (ScrollViewer)sender;
                var verticaloffset = sv.VerticalOffset;
                var scrollableheight = sv.ScrollableHeight - 30;
                if (verticaloffset >= scrollableheight)
                {
                    BackToBottom = false;
                }
                else
                {
                    BackToBottom = true;
                }
            }
            catch (Exception) { }
        }

        private void MainListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                var scrollViewer = MainListView.GetFirstDescendantOfType<ScrollViewer>();
                var verticalOffset = scrollViewer.VerticalOffset;
                var extentHeight = scrollViewer.ExtentHeight;
                var viewportHeight = scrollViewer.ViewportHeight;
                var scrollbarheight = scrollViewer.ActualHeight - scrollViewer.ScrollableHeight;
                var minus = scrollbarheight + verticalOffset;
                var downoffset = scrollViewer.ActualHeight - minus;
                var total = verticalOffset - downoffset;
                if (viewportHeight + verticalOffset == extentHeight)
                {
                    BackToBottom = false;
                }
            }
            catch (Exception) { }
        }

        private void MainTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            e.Handled = true;
            if (e.Key == VirtualKey.Enter)
            {
                UserSendMessage(MainTextBox.Text);
            }
            if (e.Key == VirtualKey.Up)
            {
                getbool = true;
                GetLastFromListUp();
                MainTextBox.SelectionStart = MainTextBox.Text.Length;
            }
            if (e.Key == VirtualKey.Down)
            {
                getbool = true;
                GetLastFromListDown();
                MainTextBox.SelectionStart = MainTextBox.Text.Length;
            }
        }

        private void GetLastFromListUp()
        {
            try
            {
                if (getbool == true)
                {
                    using (var db = new AllisonContext())
                    {
                        var message = db.MessageBubble.Where(x => x.To == true).ToListAsync();
                        get1 = message.Result[message.Result.Count - get2].MessageBubbleId;
                        MainTextBox.Text = db.MessageBubble.Find(get1).Message;
                        get2 = get2 + 1;
                        set2 = get2;
                    }
                    getbool = false;
                }
            }
            catch (Exception) { }
        }

        private void GetLastFromListDown()
        {
            try
            {
                if (getbool == true)
                {
                    using (var db = new AllisonContext())
                    {
                        var message = db.MessageBubble.Where(x => x.To == true).ToListAsync();
                        set1 = message.Result[set2 + 1].MessageBubbleId;
                        MainTextBox.Text = db.MessageBubble.Find(set1).Message;
                        set2 = set2 + 1;
                        get2 = set2;
                    }
                    getbool = false;
                }
            }
            catch (Exception) { }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ClearAllButtonShadow(ClearAllShadowHost, ClearAllShadow);
            if (Cache1.Count < 0) { backtobottom = false; }
            try
            {
                Cache1.Clear();
                MicToSendIcon = true;
                var cache1 = await GetAsync();
                cache1.ForEach(c => Cache1.Add(c));
                GetOption();
                var scrollViewer = MainListView.GetFirstDescendantOfType<ScrollViewer>();
                ScrollViewer = scrollViewer;
                if (scrollViewer != null)
                    scrollViewer.ViewChanged += OnScrollViewerViewChanged;
                BackToBottom = false;
                MainTextBox.Focus(FocusState.Programmatic);
            }
            catch (Exception)
            {

            }
        }

        private async Task<bool> AddAsync(MessageBubble messageBubble)
        {
            try
            {
                using (var db = new AllisonContext())
                {
                    db.MessageBubble.Add(messageBubble);
                    return (await db.SaveChangesAsync()) > 0;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<List<MessageBubble>> GetAsync()
        {
            try
            {
                using (var db = new AllisonContext())
                {
                    return await db.MessageBubble.ToListAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async void UserSendMessage(string message)
        {
            if (!string.IsNullOrEmpty(MainTextBox.Text) || !string.IsNullOrWhiteSpace(MainTextBox.Text))
            {
                try
                {
                    var toMsg = new MessageBubble()
                    {
                        To = true,
                        News = false,
                        Yo = false,
                        Youtube = false,
                        Select = false,
                        LiveTime = DateTime.Now.ToString("h:mm tt"),
                        Message = message,
                    };

                    if (await AddAsync(toMsg))
                    {
                        Cache1.Add(toMsg);
                    }
                    MainTextBox.Text = string.Empty;

                    if (BackToBottom != false)
                    {
                        object lastItem = MainListView.Items[MainListView.Items.Count - 1];
                        await MainListViewScroll.ScrollToItem(MainListView, lastItem);
                        BackToBottom = false;
                    }
                    SendReply(message);
                }
                catch (Exception)
                {

                }
            }
        }

        private async void UserSendMessageByButton(string message)
        {
            try
            {
                var toMsg = new MessageBubble()
                {
                    To = true,
                    News = false,
                    Yo = false,
                    Youtube = false,
                    Select = false,
                    LiveTime = DateTime.Now.ToString("h:mm tt"),
                    Message = message,
                };

                if (await AddAsync(toMsg))
                {
                    Cache1.Add(toMsg);
                }
                MainTextBox.Text = string.Empty;

                if (BackToBottom != false)
                {
                    object lastItem = MainListView.Items[MainListView.Items.Count - 1];
                    await MainListViewScroll.ScrollToItem(MainListView, lastItem);
                    BackToBottom = false;
                }
                SendReply(message);

            }
            catch (Exception)
            {

            }
        }

        private async void UserSendMessageByMic(string message)
        {
            if (!string.IsNullOrWhiteSpace(SpeechTextBox.Text) || !string.IsNullOrEmpty(SpeechTextBox.Text))
            {
                try
                {
                    var toMsg = new MessageBubble()
                    {
                        To = true,
                        News = false,
                        Yo = false,
                        Youtube = false,
                        Select = false,
                        LiveTime = DateTime.Now.ToString("h:mm tt"),
                        Message = message,
                    };

                    if (await AddAsync(toMsg))
                    {
                        Cache1.Add(toMsg);
                    }
                    MainTextBox.Text = string.Empty;

                    if (BackToBottom != false)
                    {
                        object lastItem = MainListView.Items[MainListView.Items.Count - 1];
                        await MainListViewScroll.ScrollToItem(MainListView, lastItem);
                        BackToBottom = false;
                    }
                    SendReply(message);

                }
                catch (Exception)
                {

                }
            }
        }

        private async void AllisonVoice(string text)
        {
            if (IsVoiceActive == true)
            {
                try
                {
                    using (SpeechSynthesizer synthesizer = new SpeechSynthesizer())
                    {
                        VoiceInformation voiceInfo =
                            (
                                from voice in SpeechSynthesizer.AllVoices
                                where voice.Gender == VoiceGender.Female
                                select voice
                            ).FirstOrDefault() ?? SpeechSynthesizer.DefaultVoice;

                        synthesizer.Voice = voiceInfo;
                        SpeechSynthesisStream stream = await synthesizer.SynthesizeTextToStreamAsync(text);
                        media.SetSource(stream, stream.ContentType);
                        media.Play();
                    }
                    IsVoiceActive = false;
                }
                catch { }
            }
        }

        /// <summary>
        /// ////////////////////////////////////----------------Allison Reply ------------/////////
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private async void AllisonReplyMessage1(string message)
        {
            try
            {
                var toMsg = new MessageBubble()
                {
                    To = false,
                    Yo = true,
                    News = false,
                    Youtube = false,
                    LiveTime = DateTime.Now.ToString("h:mm tt"),
                    Select = false,
                    Message = message
                };

                if (await AddAsync(toMsg))
                {
                    Cache1.Add(toMsg);
                }


                if (BackToBottom != false)
                {
                    object lastItem = MainListView.Items[MainListView.Items.Count - 1];
                    await MainListViewScroll.ScrollToItem(MainListView, lastItem);
                    BackToBottom = false;
                }

                AllisonVoice(message);
            }
            catch (Exception)
            {

            }
        }

        private async void AllisonReplyMessage2(string message)
        {
            try
            {
                var toMsg = new MessageBubble()
                {
                    To = false,
                    Yo = false,
                    Youtube = false,
                    Select = true,
                    News = false,
                    LiveTime = DateTime.Now.ToString("h:mm tt"),
                    Message = message,
                };

                if (await AddAsync(toMsg))
                {
                    Cache1.Add(toMsg);
                }

                AllisonVoice(message);
            }
            catch (Exception)
            {

            }
        }

        private async void AllisonReplyFromYoutube(string message)
        {
            try
            {
                string newsearch = message.Replace(' ', '+');
                HttpClient http = new HttpClient();
                var html = http.GetStringAsync("https://www.youtube.com/results?search_query=" + newsearch);
                string get = @"watch\?v=(\S{11})";
                Regex regex = new Regex(get);
                string gethtml = html.Result.ToString();
                MatchCollection matches = regex.Matches(gethtml);
                string GetResult = matches[0].Value;
                string VideoUrl = "https://www.youtube.com/" + GetResult;
                string GetName = GetResult.Substring(GetResult.Length - 11);
                string titlename = await YouTube.GetVideoTitleAsync(GetName);
                string gettitlename = HttpUtility.HtmlDecode(titlename);
                string videoTitle = Convert.ToString(gettitlename);
                Uri videoimageurl = YouTube.GetThumbnailUri(GetName, YouTubeThumbnailSize.Large);
                string getvideoimage = videoimageurl.ToString();

                var toMsg = new MessageBubble()
                {
                    To = false,
                    Yo = false,
                    Youtube = true,
                    Select = false,
                    News = false,
                    LiveTime = DateTime.Now.ToString("h:mm tt"),
                    VideoTitle = videoTitle.ToLower().ToString(),
                    VideoUrl = Convert.ToString(VideoUrl),
                    VideoImage = videoimage = getvideoimage
                };

                if (await AddAsync(toMsg))
                {
                    Cache1.Add(toMsg);
                    await Launcher.LaunchUriAsync(new Uri(VideoUrl));
                    MainListView.UpdateLayout();
                }

                if (BackToBottom != false)
                {
                    object lastItem = MainListView.Items[MainListView.Items.Count - 1];
                    await MainListViewScroll.ScrollToItem(MainListView, lastItem);
                    BackToBottom = false;
                }
            }
            catch (Exception) { }
        }

        public ImageSource VideoImage => new BitmapImage(new Uri(videoimage));

        private async void AllisonReplyFromNews1(string message)
        {
            try
            {
                var result = GetNewsContent(message);
                string get1 = result[0].title.ToString();
                string get2 = result[0].PubDate.ToString();
                string get3 = get2.Substring(get2.IndexOf(",") + 2);
                string newstitle = get1.Substring(0, get1.IndexOf("-"));
                string newsurl = result[0].link.ToString();
                string time1 = get2.Substring(0, get2.IndexOf(","));
                string time2 = get3.Substring(0, 6);
                string newssource = get1.Substring(get1.IndexOf("-") + 2);

                HttpClient http = new HttpClient();
                var html = http.GetStringAsync("http://news.google.com/news?q=" + message).Result;
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                var NewsImages = document.DocumentNode.Descendants("img").ToList();
                var Images = NewsImages[0].Attributes["src"].Value;

                var toMsg = new MessageBubble()
                {
                    To = false,
                    Yo = false,
                    Youtube = false,
                    Select = false,
                    News = true,
                    LiveTime = DateTime.Now.ToString("h:mm tt"),
                    NewsTitle = newstitle,
                    NewsUrl = newsurl,
                    NewsTime = newssource + " . " + time2 + " . " + time1,
                    NewsImage = newsimage = Images.ToString()
                };

                if (await AddAsync(toMsg))
                {
                    Cache1.Add(toMsg);
                    MainListView.UpdateLayout();
                }

                if (BackToBottom != false)
                {
                    object lastItem = MainListView.Items[MainListView.Items.Count - 1];
                    await MainListViewScroll.ScrollToItem(MainListView, lastItem);
                    BackToBottom = false;
                }
            }
            catch (Exception) { }
        }

        private async void AllisonReplyFromNews2(string message)
        {
            try
            {
                var result = GetNewsContent(message);
                string get1 = result[1].title.ToString();
                string get2 = result[1].PubDate.ToString();
                string get3 = get2.Substring(get2.IndexOf(",") + 2);
                string newstitle = get1.Substring(0, get1.IndexOf("-"));
                string newsurl = result[1].link.ToString();
                string time1 = get2.Substring(0, get2.IndexOf(","));
                string time2 = get3.Substring(0, 6);
                string newssource = get1.Substring(get1.IndexOf("-") + 2);

                HttpClient http = new HttpClient();
                var html = http.GetStringAsync("http://news.google.com/news?q=" + message).Result;
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                var NewsImages = document.DocumentNode.Descendants("img").ToList();
                string GetImage = null;
                var Images = NewsImages[1].Attributes["src"].Value;
                string NotRequired1 = "https://lh3.googleusercontent.com/JDFOyo903E9WGstK0YhI2ZFOKR3h4qDxBngX5M8XJVBZFKzOBoxLmk3OVlgNw9SOE-HfkNgb=w32";
                string NotRequired2 = NewsImages[0].Attributes["src"].Value;
                if (Images == NotRequired1 || Images == NotRequired2)
                {
                    GetImage = NewsImages[2].Attributes["src"].Value;
                }
                else
                {
                    GetImage = Images;
                }

                var toMsg = new MessageBubble()
                {
                    To = false,
                    Yo = false,
                    Youtube = false,
                    Select = false,
                    News = true,
                    LiveTime = DateTime.Now.ToString("h:mm tt"),
                    NewsTitle = newstitle,
                    NewsUrl = newsurl,
                    NewsTime = newssource + " . " + time2 + " . " + time1,
                    NewsImage = newsimage = GetImage.ToString()
                };

                if (await AddAsync(toMsg))
                {
                    Cache1.Add(toMsg);
                    MainListView.UpdateLayout();
                }

                if (BackToBottom != false)
                {
                    object lastItem = MainListView.Items[MainListView.Items.Count - 1];
                    await MainListViewScroll.ScrollToItem(MainListView, lastItem);
                    BackToBottom = false;
                }
            }
            catch (Exception) { }
        }

        private async void AllisonReplyFromNews3(string message)
        {
            try
            {
                var result = GetNewsContent(message);
                string get1 = result[2].title.ToString();
                string get2 = result[2].PubDate.ToString();
                string get3 = get2.Substring(get2.IndexOf(",") + 2);
                string newstitle = get1.Substring(0, get1.IndexOf("-"));
                string newsurl = result[2].link.ToString();
                string time1 = get2.Substring(0, get2.IndexOf(","));
                string time2 = get3.Substring(0, 6);
                string newssource = get1.Substring(get1.IndexOf("-") + 2);

                HttpClient http = new HttpClient();
                var html = http.GetStringAsync("http://news.google.com/news?q=" + message).Result;
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                var NewsImages = document.DocumentNode.Descendants("img").ToList();
                string GetImage = null;
                var Images = NewsImages[2].Attributes["src"].Value;
                string NotRequired1 = "https://lh3.googleusercontent.com/JDFOyo903E9WGstK0YhI2ZFOKR3h4qDxBngX5M8XJVBZFKzOBoxLmk3OVlgNw9SOE-HfkNgb=w32";
                string NotRequired2 = NewsImages[0].Attributes["src"].Value;
                string NotRequired3 = NewsImages[1].Attributes["src"].Value;
                if (Images == NotRequired1 || Images == NotRequired2 || Images == NotRequired3)
                {
                    GetImage = NewsImages[3].Attributes["src"].Value;
                }
                else
                {
                    GetImage = Images;
                }

                var toMsg = new MessageBubble()
                {
                    To = false,
                    Yo = false,
                    Youtube = false,
                    Select = false,
                    News = true,
                    LiveTime = DateTime.Now.ToString("h:mm tt"),
                    NewsTitle = newstitle,
                    NewsUrl = newsurl,
                    NewsTime = newssource + " . " + time2 + " . " + time1,
                    NewsImage = newsimage = GetImage.ToString()
                };

                if (await AddAsync(toMsg))
                {
                    Cache1.Add(toMsg);
                    MainListView.UpdateLayout();
                }

                if (BackToBottom != false)
                {
                    object lastItem = MainListView.Items[MainListView.Items.Count - 1];
                    await MainListViewScroll.ScrollToItem(MainListView, lastItem);
                    BackToBottom = false;
                }
            }
            catch (Exception) { }
        }

        private async void AllisonReplyFromNews4(string message)
        {
            try
            {
                var result = GetNewsContent(message);
                string get1 = result[3].title.ToString();
                string get2 = result[3].PubDate.ToString();
                string get3 = get2.Substring(get2.IndexOf(",") + 2);
                string newstitle = get1.Substring(0, get1.IndexOf("-"));
                string newsurl = result[3].link.ToString();
                string time1 = get2.Substring(0, get2.IndexOf(","));
                string time2 = get3.Substring(0, 6);
                string newssource = get1.Substring(get1.IndexOf("-") + 2);

                HttpClient http = new HttpClient();
                var html = http.GetStringAsync("http://news.google.com/news?q=" + message).Result;
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                var NewsImages = document.DocumentNode.Descendants("img").ToList();
                string GetImage = null;
                var Images = NewsImages[3].Attributes["src"].Value;
                string NotRequired1 = "https://lh3.googleusercontent.com/JDFOyo903E9WGstK0YhI2ZFOKR3h4qDxBngX5M8XJVBZFKzOBoxLmk3OVlgNw9SOE-HfkNgb=w32";
                string NotRequired2 = NewsImages[0].Attributes["src"].Value;
                string NotRequired3 = NewsImages[1].Attributes["src"].Value;
                string NotRequired4 = NewsImages[2].Attributes["src"].Value;
                if (Images == NotRequired1 || Images == NotRequired2 || Images == NotRequired3 || Images == NotRequired4)
                {
                    GetImage = NewsImages[4].Attributes["src"].Value;
                }
                else
                {
                    GetImage = Images;
                }

                var toMsg = new MessageBubble()
                {
                    To = false,
                    Yo = false,
                    Youtube = false,
                    Select = false,
                    News = true,
                    LiveTime = DateTime.Now.ToString("h:mm tt"),
                    NewsTitle = newstitle,
                    NewsUrl = newsurl,
                    NewsTime = newssource + " . " + time2 + " . " + time1,
                    NewsImage = newsimage = GetImage.ToString()
                };

                if (await AddAsync(toMsg))
                {
                    Cache1.Add(toMsg);
                    MainListView.UpdateLayout();
                }

                if (BackToBottom != false)
                {
                    object lastItem = MainListView.Items[MainListView.Items.Count - 1];
                    await MainListViewScroll.ScrollToItem(MainListView, lastItem);
                    BackToBottom = false;
                }
            }
            catch (Exception) { }
        }

        private async void AllisonReplyFromNews5(string message)
        {
            try
            {
                var result = GetNewsContent(message);
                string get1 = result[4].title.ToString();
                string get2 = result[4].PubDate.ToString();
                string get3 = get2.Substring(get2.IndexOf(",") + 2);
                string newstitle = get1.Substring(0, get1.IndexOf("-"));
                string newsurl = result[4].link.ToString();
                string time1 = get2.Substring(0, get2.IndexOf(","));
                string time2 = get3.Substring(0, 6);
                string newssource = get1.Substring(get1.IndexOf("-") + 2);

                HttpClient http = new HttpClient();
                var html = http.GetStringAsync("http://news.google.com/news?q=" + message).Result;
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                var NewsImages = document.DocumentNode.Descendants("img").ToList();
                string GetImage = null;
                var Images = NewsImages[4].Attributes["src"].Value;
                string NotRequired1 = "https://lh3.googleusercontent.com/JDFOyo903E9WGstK0YhI2ZFOKR3h4qDxBngX5M8XJVBZFKzOBoxLmk3OVlgNw9SOE-HfkNgb=w32";
                string NotRequired2 = NewsImages[0].Attributes["src"].Value;
                string NotRequired3 = NewsImages[1].Attributes["src"].Value;
                string NotRequired4 = NewsImages[2].Attributes["src"].Value;
                string NotRequired5 = NewsImages[3].Attributes["src"].Value;
                if (Images == NotRequired1 || Images == NotRequired2 || Images == NotRequired3 || Images == NotRequired4 || Images == NotRequired5)
                {
                    GetImage = NewsImages[5].Attributes["src"].Value;
                }
                else
                {
                    GetImage = Images;
                }

                var toMsg = new MessageBubble()
                {
                    To = false,
                    Yo = false,
                    Youtube = false,
                    Select = false,
                    News = true,
                    LiveTime = DateTime.Now.ToString("h:mm tt"),
                    NewsTitle = newstitle,
                    NewsUrl = newsurl,
                    NewsTime = newssource + " . " + time2 + " . " + time1,
                    NewsImage = newsimage = GetImage.ToString()
                };

                if (await AddAsync(toMsg))
                {
                    Cache1.Add(toMsg);
                    MainListView.UpdateLayout();
                }

                if (BackToBottom != false)
                {
                    object lastItem = MainListView.Items[MainListView.Items.Count - 1];
                    await MainListViewScroll.ScrollToItem(MainListView, lastItem);
                    BackToBottom = false;
                }
            }
            catch (Exception) { }
        }

        public ImageSource NewsImage => new BitmapImage(new Uri(newsimage));

        public ItemNews[] GetNewsContent(string NewsParameters)
        {
            List<ItemNews> Details = new List<ItemNews>();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create
            ("http://news.google.com/news?q=" + NewsParameters + "&output=rss");

            request.Method = "GET";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (response.CharacterSet == "")
                    readStream = new StreamReader(receiveStream);
                else
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));

                string data = readStream.ReadToEnd();

                DataSet ds = new DataSet();
                StringReader reader = new StringReader(data);
                ds.ReadXml(reader);
                DataTable dtGetNews = new DataTable();

                if (ds.Tables.Count > 3)
                {
                    dtGetNews = ds.Tables["item"];

                    foreach (DataRow dtRow in dtGetNews.Rows)
                    {
                        ItemNews DataObj = new ItemNews();
                        DataObj.title = dtRow["title"].ToString();
                        DataObj.link = dtRow["link"].ToString();
                        DataObj.PubDate = dtRow["pubDate"].ToString();
                        DataObj.Description = dtRow["description"].ToString();
                        Details.Add(DataObj);
                    }
                }
            }
            return Details.ToArray();
        }

        public class ItemNews
        {
            public string title { get; set; }
            public string link { get; set; }
            public string PubDate { get; set; }
            public string Description { get; set; }
        }

        /// <summary>
        /// ////////////////////////////////////------------Allison Reply Over------------/////////
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            ClearAllPopup.IsOpen = true;
            ClearAllPopupOverlay.Visibility = Visibility.Visible;
        }

        private void Grid_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            Grid g = (Grid)sender;
            DeleteMenu.ShowAt(g, e.GetPosition(g));
        }

        private void TextBoxGrid_Loaded(object sender, RoutedEventArgs e)
        {
            //MainTextBoxShadow(MainTextBoxShadowHost, TextBoxShadow);
            MicButtonShadow(MicShadowHost, MicShadow);
            BackToBottomButtonShadow(BackToBottomButtonHost, BackToBottomRectangle);
            CustomDropShadowForTextBox(shadowHost, TextBoxRectangleShadow);
            ClearTextButtonShadow(ClearTextShadowHost, ClearTextShadow);
        }

        private void CustomDropShadowForTextBox(UIElement shadowHost, Shape shadowTarget)
        {
            Visual hostVisual = ElementCompositionPreview.GetElementVisual(shadowHost);
            Compositor compositor = hostVisual.Compositor;
            var dropShadow = compositor.CreateDropShadow();
            dropShadow.Color = Colors.DarkGray;
            dropShadow.BlurRadius = 20;
            dropShadow.Opacity = 10.0f;
            dropShadow.Mask = shadowTarget.GetAlphaMask();
            var shadowVisual = compositor.CreateSpriteVisual();
            shadowVisual.Shadow = dropShadow;
            ElementCompositionPreview.SetElementChildVisual(shadowHost, shadowVisual);
            var bindSizeAnimation = compositor.CreateExpressionAnimation("hostVisual.Size");
            bindSizeAnimation.SetReferenceParameter("hostVisual", hostVisual);
            shadowVisual.StartAnimation("Size", bindSizeAnimation);
        }

        private void BackToBottomButtonShadow(UIElement shadowHost, Shape shadowTarget)
        {
            Visual hostVisual = ElementCompositionPreview.GetElementVisual(shadowHost);
            Compositor compositor = hostVisual.Compositor;
            var dropShadow = compositor.CreateDropShadow();
            dropShadow.Color = Colors.LightGray;
            dropShadow.BlurRadius = 20;
            dropShadow.Opacity = 10.0f;
            dropShadow.Mask = shadowTarget.GetAlphaMask();
            var shadowVisual = compositor.CreateSpriteVisual();
            shadowVisual.Shadow = dropShadow;
            ElementCompositionPreview.SetElementChildVisual(shadowHost, shadowVisual);
            var bindSizeAnimation = compositor.CreateExpressionAnimation("hostVisual.Size");
            bindSizeAnimation.SetReferenceParameter("hostVisual", hostVisual);
            shadowVisual.StartAnimation("Size", bindSizeAnimation);
        }

        private void MicButtonShadow(UIElement shadowHost, Shape shadowTarget)
        {
            Visual hostVisual = ElementCompositionPreview.GetElementVisual(shadowHost);
            Compositor compositor = hostVisual.Compositor;
            var dropShadow = compositor.CreateDropShadow();
            dropShadow.Color = Colors.LightGray;
            dropShadow.BlurRadius = 10;
            dropShadow.Opacity = 10.0f;
            dropShadow.Mask = shadowTarget.GetAlphaMask();
            var shadowVisual = compositor.CreateSpriteVisual();
            shadowVisual.Shadow = dropShadow;
            ElementCompositionPreview.SetElementChildVisual(shadowHost, shadowVisual);
            var bindSizeAnimation = compositor.CreateExpressionAnimation("hostVisual.Size");
            bindSizeAnimation.SetReferenceParameter("hostVisual", hostVisual);
            shadowVisual.StartAnimation("Size", bindSizeAnimation);
        }

        private void ClearAllButtonShadow(UIElement shadowHost, Shape shadowTarget)
        {
            Visual hostVisual = ElementCompositionPreview.GetElementVisual(shadowHost);
            Compositor compositor = hostVisual.Compositor;
            var dropShadow = compositor.CreateDropShadow();
            dropShadow.Color = Colors.DarkGray;
            dropShadow.BlurRadius = 20;
            dropShadow.Opacity = 10.0f;
            dropShadow.Mask = shadowTarget.GetAlphaMask();
            var shadowVisual = compositor.CreateSpriteVisual();
            shadowVisual.Shadow = dropShadow;
            ElementCompositionPreview.SetElementChildVisual(shadowHost, shadowVisual);
            var bindSizeAnimation = compositor.CreateExpressionAnimation("hostVisual.Size");
            bindSizeAnimation.SetReferenceParameter("hostVisual", hostVisual);
            shadowVisual.StartAnimation("Size", bindSizeAnimation);
        }

        private void ClearTextButtonShadow(UIElement shadowHost, Shape shadowTarget)
        {
            Visual hostVisual = ElementCompositionPreview.GetElementVisual(shadowHost);
            Compositor compositor = hostVisual.Compositor;
            var dropShadow = compositor.CreateDropShadow();
            dropShadow.Color = Colors.LightGray;
            dropShadow.BlurRadius = 10;
            dropShadow.Opacity = 10.0f;
            dropShadow.Mask = shadowTarget.GetAlphaMask();
            var shadowVisual = compositor.CreateSpriteVisual();
            shadowVisual.Shadow = dropShadow;
            ElementCompositionPreview.SetElementChildVisual(shadowHost, shadowVisual);
            var bindSizeAnimation = compositor.CreateExpressionAnimation("hostVisual.Size");
            bindSizeAnimation.SetReferenceParameter("hostVisual", hostVisual);
            shadowVisual.StartAnimation("Size", bindSizeAnimation);
        }

        private async void BackToBottomButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                object lastItem = MainListView.Items[MainListView.Items.Count - 1];
                await MainListViewScroll.ScrollToItem(MainListView, lastItem);
            }
            catch (Exception)
            {

            }
        }

        private void MicAndSendButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(MainTextBox.Text) && MainTextBox.Text.Length > 0)
            {
                UserSendMessage(MainTextBox.Text);
            }
            else
            {
                MicSound_1();
                SpeechTextBox.IsEnabled = true;
                SpeechTextBox.Text = string.Empty;
                StartSpeechRecognition();
                MicOn();
                IsVoiceActive = true;
            }
        }

        private void MicOn()
        {
            try
            {
                TextGrid.Visibility = Visibility.Collapsed;
                MicGrid.Visibility = Visibility.Collapsed;
                SpeechGrid.Visibility = Visibility.Visible;
            }
            catch (Exception)
            {

            }
        }

        private void MicOff()
        {
            try
            {
                SpeechGrid.Visibility = Visibility.Collapsed;
                TextGrid.Visibility = Visibility.Visible;
                MicGrid.Visibility = Visibility.Visible;
            }
            catch (Exception)
            {

            }
        }

        private void BackToBottomButton_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            BackToBottomButton.Foreground = new SolidColorBrush(Colors.DarkGray);
        }

        private void MainTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(MainTextBox.Text) && MainTextBox.Text.Length > 0)
            {
                ClearTextButtonGrid.Visibility = Visibility.Visible;
                MicToSendIcon = false;
                ToolTip toolTip = new ToolTip();
                toolTip.Content = "Send";
                ToolTipService.SetToolTip(MicAndSendButton, toolTip);
                return;
            }
            else
            {
                ClearTextButtonGrid.Visibility = Visibility.Collapsed;
                MicToSendIcon = true;
                ToolTip toolTip = new ToolTip();
                toolTip.Content = "Speak to Allison";
                ToolTipService.SetToolTip(MicAndSendButton, toolTip);
                return;
            }
        }

        private void ClearTextButton_Click(object sender, RoutedEventArgs e)
        {
            MainTextBox.Text = string.Empty;
        }

        private void ClearTextButton_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            ClearTextShadow.Visibility = Visibility.Visible;
        }

        private void ClearTextButton_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            ClearTextShadow.Visibility = Visibility.Collapsed;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ClearAllPopup.IsOpen = false;
            ClearAllPopupOverlay.Visibility = Visibility.Collapsed;
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            Cache1.Clear();
            BackToBottom = false;
            ClearAllPopup.IsOpen = false;
            ClearAllPopupOverlay.Visibility = Visibility.Collapsed;
            try
            {
                using (var db = new AllisonContext())
                {
                    foreach (var ClearAllMessage in db.MessageBubble)
                    {
                        db.MessageBubble.Remove(ClearAllMessage);
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// ////////////////////////////////////---------------- Reply ------------/////////
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void MainTextBoxLostFocus()
        {
            MainTextBox.IsEnabled = false;
            MainTextBox.IsEnabled = true;
        }

        private async void SendReply(string text)
        {
            MainTextBoxLostFocus();
            TextBoxGridFunction.Visibility = Visibility.Visible;
            if (text.ToLower().Contains("hello"))
            {
                await Task.Delay(TimeSpan.FromSeconds(2));
                Random random = new Random();
                int r = random.Next(1, 4);
                if (r == 1)
                    AllisonReplyMessage1("Hola, that's 'hello' in spanish, to see your schedule just say 'what's my day look like'");
                if (r == 2)
                    AllisonReplyMessage1("Bonjour, that's 'hello' in franch, to find out your next task just say 'what's next'");
                if (r == 3)
                    AllisonReplyMessage1("Ciao, that's 'hello' in italian, to check your task list just say 'show my schedule'");
            }
            else if (text.ToLower().Contains("how are you"))
            {
                await Task.Delay(TimeSpan.FromSeconds(2));
                AllisonReplyMessage1("I am fine as like as you, thanks for asking.");
                await Task.Delay(TimeSpan.FromSeconds(2));
                AllisonReplyMessage2("Do you want me to help you ?");
            }
            else if (text.ToLower().Contains("reminders"))
            {
                await Task.Delay(TimeSpan.FromSeconds(3));
                Random random = new Random();
                int r = random.Next(1, 4);
                if (r == 1)
                    AllisonReplyMessage1("It's seems like you don't have anyhthing schedule.");
                if (r == 2)
                    AllisonReplyMessage1("You have no schedule.");
                if (r == 3)
                    AllisonReplyMessage1("You don't have events coming up.");
            }
            else if (text.ToLower().Contains("play") || text.ToLower().StartsWith("play"))
            {
                await Task.Delay(TimeSpan.FromSeconds(2));
                Random random = new Random();
                string S1 = text.ToLower().Replace("play", "");
                int r = random.Next(1, 4);
                if (r == 1)
                    AllisonReplyMessage1("Asking youtube to play" + S1 + ".");
                if (r == 2)
                    AllisonReplyMessage1("Requesting youtube to play" + S1 + ".");
                if (r == 3)
                    AllisonReplyMessage1("playing" + S1 + " on youtube.");
                await Task.Delay(TimeSpan.FromSeconds(1));
                AllisonReplyFromYoutube(text);
            }
            else if (text.ToLower().Contains("news") || text.ToLower().EndsWith("news"))
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                Random random = new Random();
                string S1 = text.ToLower()
                                .Replace("news", "")
                                .Replace("show", "")
                                .Replace("some", "")
                                .Replace("me", "")
                                .Replace("could", "")
                                .Replace("you", "")
                                .Replace("latest", "")
                                .Replace("from", "")
                                .Replace("please", "");
                string S = Regex.Replace(S1, @"\s", "");
                int r = random.Next(1, 5);
                if (r == 1)
                    AllisonReplyMessage1("I have got latest headlines for " + S + ".");
                if (r == 2)
                    AllisonReplyMessage1("I found latest headlines for " + S + ".");
                if (r == 3)
                    AllisonReplyMessage1("Here are some latest headlines for " + S + ".");
                if (r == 4)
                    AllisonReplyMessage1("I found this headlines for you.");
                await Task.Delay(TimeSpan.FromSeconds(0.1));
                AllisonReplyFromNews1(text);
                await Task.Delay(TimeSpan.FromSeconds(0.1));
                AllisonReplyFromNews2(text);
                await Task.Delay(TimeSpan.FromSeconds(0.1));
                AllisonReplyFromNews3(text);
                await Task.Delay(TimeSpan.FromSeconds(0.1));
                AllisonReplyFromNews4(text);
                await Task.Delay(TimeSpan.FromSeconds(0.1));
                AllisonReplyFromNews5(text);
            }
            else
            {
                await Task.Delay(TimeSpan.FromSeconds(3));
                Random r = new Random();
                int a = r.Next(1, 4);
                if (a == 1)
                    AllisonReplyMessage1("Couldn't know the answer to this one.");
                if (a == 2)
                    AllisonReplyMessage1("I did'n found anything matching this one.");
                if (a == 3)
                    AllisonReplyMessage1("Sorry, I don't know the answer.");
            }
            GetOption();
            TextBoxGridFunction.Visibility = Visibility.Collapsed;
            MainTextBox.Focus(FocusState.Programmatic);
        }

        /// <summary>
        /// ////////////////////////////////////---------------- Mic Sound ------------/////////
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void MicSound_1()
        {
            media.Source = null;
            media.Source = new Uri("ms-appx:Assets/Sound/mic_1.mp3");
            media.Play();
        }

        private void MicSound_2()
        {
            media.Source = null;
            media.Source = new Uri("ms-appx:Assets/Sound/mic_2.mp3");
            media.Play();
        }

        /// <summary>
        /// ////////////////////////////////////---------------- Speech Recognition ------------/////////
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private async Task InitializeRecognizer(Language recognizerLanguage)
        {
            if (speechRecognizer != null)
            {
                speechRecognizer.StateChanged -= SpeechRecognizer_StateChanged;
                speechRecognizer.HypothesisGenerated -= SpeechRecognizer_HypothesisGenerated;
                speechRecognizer.ContinuousRecognitionSession.ResultGenerated -= ContinuousRecognitionSession_ResultGenerated;
                speechRecognizer.ContinuousRecognitionSession.Completed -= ContinuousRecognitionSession_Completed;


                this.speechRecognizer.Dispose();
                this.speechRecognizer = null;
            }

            this.speechRecognizer = new SpeechRecognizer(recognizerLanguage);

            speechRecognizer.StateChanged += SpeechRecognizer_StateChanged;

            var dictationConstraint = new SpeechRecognitionTopicConstraint(SpeechRecognitionScenario.Dictation, "dictation");
            speechRecognizer.Constraints.Add(dictationConstraint);
            SpeechRecognitionCompilationResult result = await speechRecognizer.CompileConstraintsAsync();
            if (result.Status != SpeechRecognitionResultStatus.Success)
            {
                SpeechTextBox.Text = string.Empty;
            }

            speechRecognizer.ContinuousRecognitionSession.Completed += ContinuousRecognitionSession_Completed;
            speechRecognizer.ContinuousRecognitionSession.ResultGenerated += ContinuousRecognitionSession_ResultGenerated;
            speechRecognizer.HypothesisGenerated += SpeechRecognizer_HypothesisGenerated;
        }

        private async void ContinuousRecognitionSession_ResultGenerated(SpeechContinuousRecognitionSession sender, SpeechContinuousRecognitionResultGeneratedEventArgs args)
        {
            if (args.Result.Confidence == SpeechRecognitionConfidence.Medium || args.Result.Confidence == SpeechRecognitionConfidence.High)
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    SpeechTextBox.Text = args.Result.Text.ToString();
                });
            }
        }

        private async void ContinuousRecognitionSession_Completed(SpeechContinuousRecognitionSession sender, SpeechContinuousRecognitionCompletedEventArgs args)
        {
            if (args.Status != SpeechRecognitionResultStatus.Success)
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => 
                {
                    MicSound_2();
                    MicOff();
                    UserSendMessageByMic(speechtext);
                });
            }
        }

        private async void StartSpeechRecognition()
        {
            if (speechRecognizer.State == SpeechRecognizerState.Idle)
            {
                try
                {
                    ClearSpeechTextBox();
                    speechRecognizer.ContinuousRecognitionSession.AutoStopSilenceTimeout = TimeSpan.FromSeconds(3);
                    await speechRecognizer.ContinuousRecognitionSession.StartAsync();
                }
                catch { }
            }
        }

        private void SpeechRecognizer_StateChanged(SpeechRecognizer sender, SpeechRecognizerStateChangedEventArgs args)
        {
            try
            {
                if (speechRecognizer.State == SpeechRecognizerState.Idle)
                {
                    MicOff();
                }
                if (speechRecognizer.State == SpeechRecognizerState.Capturing)
                {
                    MicOn();
                }
                if (speechRecognizer.State == SpeechRecognizerState.SpeechDetected)
                {
                    MicOn();
                }
                if (speechRecognizer.State == SpeechRecognizerState.SoundStarted)
                {
                    MicOn();
                }
                if (speechRecognizer.State == SpeechRecognizerState.SoundEnded)
                {
                    MicOff();
                }
                if (speechRecognizer.State == SpeechRecognizerState.Paused)
                {
                    MicOff();
                }
                if (speechRecognizer.State == SpeechRecognizerState.Processing)
                {
                    MicOn();
                }
            }
            catch (Exception) { }
        }

        private async void SpeechRecognizer_HypothesisGenerated(SpeechRecognizer sender, SpeechRecognitionHypothesisGeneratedEventArgs args)
        {
            string hypothesis = args.Hypothesis.Text;
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                SpeechTextBox.Text = speechtext = hypothesis;
            });
        }

        private void ClearSpeechTextBox()
        {
            SpeechTextBox.IsEnabled = true;
            SpeechTextBox.Text = string.Empty;
            speechtext = string.Empty;
        }

        /// <summary>
        /// ////////////////////////////////////---------------- All Message DB ------------/////////
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void GetOption()
        {
            Random random = new Random();
            int a = random.Next(1, 11);

            if (a == 1)
            {
                MessageButton1Text.Text = "Play me a romantic song";
                MessageButton2Text.Text = "Show me some business news";
                MessageButton3Text.Text = "Remind me to make my breakfast in next morning";
                MessageButton4Text.Text = "Who are you?";
                MessageButton5Text.Text = "Latest from Covid news";
                MessageButton6Text.Text = "Play latest party song on youtube";
                MessageButton7Text.Text = "Open youtube";
                MessageButton8Text.Text = "Play technical guruji";
                MessageButton9Text.Text = "Open spotify";
                MessageButton10Text.Text = "Play tere hone laga hoon on youtube";
            }
            if (a == 2)
            {
                MessageButton10Text.Text = "Play me a romantic song";
                MessageButton9Text.Text = "Show me some business news";
                MessageButton8Text.Text = "Remind me to make my breakfast in next morning";
                MessageButton7Text.Text = "Who are you?";
                MessageButton6Text.Text = "Latest from Covid news";
                MessageButton5Text.Text = "Play latest party song on youtube";
                MessageButton4Text.Text = "Open youtube";
                MessageButton3Text.Text = "Play technical guruji";
                MessageButton2Text.Text = "Open spotify";
                MessageButton1Text.Text = "Play tere hone laga hoon on youtube";
            }
            if (a == 3)
            {
                MessageButton5Text.Text = "Play me a romantic song";
                MessageButton3Text.Text = "Show me some business news";
                MessageButton6Text.Text = "Remind me to make my breakfast in next morning";
                MessageButton1Text.Text = "Who are you?";
                MessageButton2Text.Text = "Latest from Covid news";
                MessageButton4Text.Text = "Play latest party song on youtube";
                MessageButton7Text.Text = "Open youtube";
                MessageButton9Text.Text = "Play technical guruji";
                MessageButton10Text.Text = "Open spotify";
                MessageButton8Text.Text = "Play tere hone laga hoon on youtube";
            }
            if (a == 4)
            {
                MessageButton2Text.Text = "Play me a romantic song";
                MessageButton4Text.Text = "Show me some business news";
                MessageButton6Text.Text = "Remind me to make my breakfast in next morning";
                MessageButton8Text.Text = "Who are you?";
                MessageButton10Text.Text = "Latest from Covid news";
                MessageButton1Text.Text = "Play latest party song on youtube";
                MessageButton3Text.Text = "Open youtube";
                MessageButton5Text.Text = "Play technical guruji";
                MessageButton7Text.Text = "Open spotify";
                MessageButton9Text.Text = "Play tere hone laga hoon on youtube";
            }
            if (a == 5)
            {
                MessageButton9Text.Text = "Play me a romantic song";
                MessageButton7Text.Text = "Show me some business news";
                MessageButton5Text.Text = "Remind me to make my breakfast in next morning";
                MessageButton3Text.Text = "Who are you?";
                MessageButton1Text.Text = "Latest from Covid news";
                MessageButton2Text.Text = "Play latest party song on youtube";
                MessageButton4Text.Text = "Open youtube";
                MessageButton6Text.Text = "Play technical guruji";
                MessageButton8Text.Text = "Open spotify";
                MessageButton10Text.Text = "Play tere hone laga hoon on youtube";
            }
            if (a == 6)
            {
                MessageButton1Text.Text = "Play me a romantic song";
                MessageButton10Text.Text = "Show me some business news";
                MessageButton2Text.Text = "Remind me to make my breakfast in next morning";
                MessageButton9Text.Text = "Who are you?";
                MessageButton3Text.Text = "Latest from Covid news";
                MessageButton8Text.Text = "Play latest party song on youtube";
                MessageButton4Text.Text = "Open youtube";
                MessageButton7Text.Text = "Play technical guruji";
                MessageButton5Text.Text = "Open spotify";
                MessageButton6Text.Text = "Play tere hone laga hoon on youtube";
            }
            if (a == 7)
            {
                MessageButton6Text.Text = "Play me a romantic song";
                MessageButton5Text.Text = "Show me some business news";
                MessageButton7Text.Text = "Remind me to make my breakfast in next morning";
                MessageButton4Text.Text = "Who are you?";
                MessageButton8Text.Text = "Latest from Covid news";
                MessageButton3Text.Text = "Play latest party song on youtube";
                MessageButton9Text.Text = "Open youtube";
                MessageButton2Text.Text = "Play technical guruji";
                MessageButton10Text.Text = "Open spotify";
                MessageButton1Text.Text = "Play tere hone laga hoon on youtube";
            }
            if (a == 8)
            {
                MessageButton5Text.Text = "Play me a romantic song";
                MessageButton10Text.Text = "Show me some business news";
                MessageButton4Text.Text = "Remind me to make my breakfast in next morning";
                MessageButton9Text.Text = "Who are you?";
                MessageButton3Text.Text = "Latest from Covid news";
                MessageButton8Text.Text = "Play latest party song on youtube";
                MessageButton2Text.Text = "Open youtube";
                MessageButton7Text.Text = "Play technical guruji";
                MessageButton1Text.Text = "Open spotify";
                MessageButton6Text.Text = "Play tere hone laga hoon on youtube";
            }
            if (a == 9)
            {
                MessageButton6Text.Text = "Play me a romantic song";
                MessageButton1Text.Text = "Show me some business news";
                MessageButton7Text.Text = "Remind me to make my breakfast in next morning";
                MessageButton2Text.Text = "Who are you?";
                MessageButton8Text.Text = "Latest from Covid news";
                MessageButton3Text.Text = "Play latest party song on youtube";
                MessageButton9Text.Text = "Open youtube";
                MessageButton4Text.Text = "Play technical guruji";
                MessageButton10Text.Text = "Open spotify";
                MessageButton5Text.Text = "Play tere hone laga hoon on youtube";
            }
            if (a == 10)
            {
                MessageButton9Text.Text = "Play me a romantic song";
                MessageButton8Text.Text = "Show me some business news";
                MessageButton6Text.Text = "Remind me to make my breakfast in next morning";
                MessageButton5Text.Text = "Who are you?";
                MessageButton3Text.Text = "Latest from Covid news";
                MessageButton1Text.Text = "Play latest party song on youtube";
                MessageButton2Text.Text = "Open youtube";
                MessageButton4Text.Text = "Play technical guruji";
                MessageButton10Text.Text = "Open spotify";
                MessageButton7Text.Text = "Play tere hone laga hoon on youtube";
            }
            else
            {
                MessageButton1Text.Text = "Play me a romantic song";
                MessageButton2Text.Text = "Show me some business news";
                MessageButton3Text.Text = "Remind me to make my breakfast in next morning";
                MessageButton4Text.Text = "Who are you?";
                MessageButton5Text.Text = "Latest from Covid news";
                MessageButton6Text.Text = "Play latest party song on youtube";
                MessageButton7Text.Text = "Open youtube";
                MessageButton8Text.Text = "Play technical guruji";
                MessageButton9Text.Text = "Open spotify";
                MessageButton10Text.Text = "Play tere hone laga hoon on youtube";
            }

            #pragma warning disable CS0618 
            MessageScrollBar.ScrollToHorizontalOffset(0);
            #pragma warning restore CS0618
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new AllisonContext())
                {
                    var messages = db.MessageBubble.Where(x => x.To == true).ToListAsync();
                    var get = messages.Result.LastOrDefault();
                    if (get.Message != null)
                        await Launcher.LaunchUriAsync(new Uri("https://www.google.com/search?q=" + get.Message.ToString()));
                }
            }
            catch (Exception) { }
        }

        private void MessageButton1_Click(object sender, RoutedEventArgs e)
        {
            UserSendMessageByButton(MessageButton1Text.Text.ToString());
        }

        private void MessageButton2_Click(object sender, RoutedEventArgs e)
        {
            UserSendMessageByButton(MessageButton2Text.Text.ToString());
        }

        private void MessageButton3_Click(object sender, RoutedEventArgs e)
        {
            UserSendMessageByButton(MessageButton3Text.Text.ToString());
        }

        private void MessageButton4_Click(object sender, RoutedEventArgs e)
        {
            UserSendMessageByButton(MessageButton4Text.Text.ToString());
        }

        private void MessageButton5_Click(object sender, RoutedEventArgs e)
        {
            UserSendMessageByButton(MessageButton5Text.Text.ToString());
        }

        private void MessageButton6_Click(object sender, RoutedEventArgs e)
        {
            UserSendMessageByButton(MessageButton6Text.Text.ToString());
        }

        private void MessageButton7_Click(object sender, RoutedEventArgs e)
        {
            UserSendMessageByButton(MessageButton7Text.Text.ToString());
        }

        private void MessageButton8_Click(object sender, RoutedEventArgs e)
        {
            UserSendMessageByButton(MessageButton8Text.Text.ToString());
        }

        private void MessageButton9_Click(object sender, RoutedEventArgs e)
        {
            UserSendMessageByButton(MessageButton9Text.Text.ToString());
        }

        private void MessageButton10_Click(object sender, RoutedEventArgs e)
        {
            UserSendMessageByButton(MessageButton10Text.Text.ToString());
        }
    }

    public class CustomDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate LeftMessageTemplate { get; set; }
        public DataTemplate RightMessageTemplate { get; set; }
        public DataTemplate SecondLeftMessageTemplate { get; set; }
        public DataTemplate YoutubeTemplate { get; set; }
        public DataTemplate NewsTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            var message = item as MessageBubble;
            if (message.To)
            {
                return this.RightMessageTemplate;
            }
            if (message.Yo)
            {
                return this.LeftMessageTemplate;
            }
            if (message.Select)
            {
                return this.SecondLeftMessageTemplate;
            }
            if (message.Youtube)
            {
                return this.YoutubeTemplate;
            }
            if (message.News)
            {
                return this.NewsTemplate;
            }
            return null;
        }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            return SelectTemplateCore(item);
        }
    }

    public class AudioCapturePermissions
    {
        private static int NoCaptureDevicesHResult = -1072845856;
        private static uint HResultPrivacyStatementDeclined = 0x80045509;
        public async static Task<bool> RequestMicrophonePermission()
        {
            try
            {
                MediaCaptureInitializationSettings settings = new MediaCaptureInitializationSettings();
                settings.StreamingCaptureMode = StreamingCaptureMode.Audio;
                settings.MediaCategory = MediaCategory.Speech;
                MediaCapture capture = new MediaCapture();

                await capture.InitializeAsync(settings);
            }
            catch (TypeLoadException)
            {
                var messageDialog = new Windows.UI.Popups.MessageDialog("Media player components are unavailable.");
                await messageDialog.ShowAsync();
                return false;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
            catch (Exception exception)
            {
                if (exception.HResult == NoCaptureDevicesHResult)
                {
                    var messageDialog = new Windows.UI.Popups.MessageDialog("No Audio Capture devices are present on this system.");
                    await messageDialog.ShowAsync();
                    return false;
                }
                if ((uint)exception.HResult == HResultPrivacyStatementDeclined)
                {
                    var messageDialog = new Windows.UI.Popups.MessageDialog("The privacy statement was declined. Go to Settings -> Privacy -> Speech, inking and typing, and ensure you have viewed the privacy policy, and 'Get To Know You' is enabled.");
                    await messageDialog.ShowAsync();
                    await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings:privacy-accounts"));
                    return false;
                }
                else
                {
                    var messageDialog = new Windows.UI.Popups.MessageDialog(exception.Message, "Exception");
                    await messageDialog.ShowAsync();
                    throw;
                }
            }
            return true;
        }
    }
}