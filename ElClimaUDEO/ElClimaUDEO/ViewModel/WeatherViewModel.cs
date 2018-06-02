using ElClimaUDEO.Model;
using JetBrains.Annotations;
using System;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace ElClimaUDEO.ViewModel
{
    public class WeatherViewModel : INotifyPropertyChanged
    {

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Properties

        private string _location;
        private string _condition;
        private string _searchTerm;
        private ImageSource _weatherImage;
        public string Location
        {
            get { return _location; }
            set
            {
                _location = value;
                OnPropertyChanged();
            }
        }

        public string Condition
        {
            get { return _condition; }
            set
            {
                _condition = value;
                OnPropertyChanged();
            }
        }

        public string SearchTerm
        {
            get { return _searchTerm; }
            set
            {
                _searchTerm = value;
                OnPropertyChanged();
            }
        }

        public ImageSource WeatherImage
        {
            get { return _weatherImage; }
            set
            {
                _weatherImage = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public Command SearchCommand { get; set; }

        public WeatherViewModel()
        {
            SearchCommand = new Command(SearchWeather);
        }
        public async void SearchWeather()
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(GetUrl());
                var response =
                    await client.GetAsync(client.BaseAddress);
                response.EnsureSuccessStatusCode();
                var jsonResult = response.Content.ReadAsStringAsync().Result;
                var weather = WeatherModel.FromJson(jsonResult);
                SetValues(weather);
            }
            catch (Exception ex)
            {

                throw ex;
            }

           
        }
        private void SetValues(WeatherModel weather)
        {
            Location = $"Ciudad: {weather.Query.Results.Channel.Location.City}\n" +
                       $"País: {weather.Query.Results.Channel.Location.Country}\n" +
                       $"Región: {weather.Query.Results.Channel.Location.Region}";
            Condition = $"Última actualización: {weather.Query.Results.Channel.Item.Condition.Date}\n" +
                        $"Temperatura: {weather.Query.Results.Channel.Item.Condition.Temp}\n" +
                        $"Clima: {weather.Query.Results.Channel.Item.Condition.Text}";
            var imageLink = $"http://l.yimg.com/a/i/us/we/52/{weather.Query.Results.Channel.Item.Condition.Code}.gif";
            WeatherImage = ImageSource.FromUri(new Uri(imageLink));
        }

        private string GetUrl()
        {
            string serviceUrl =
                $"https://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20weather.forecast%20where%20woeid%20in%20(select%20woeid%20from%20geo.places(1)%20where%20text%3D%22{SearchTerm}%22)&format=json&diagnostics=true&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys&callback=";
            return serviceUrl;
        }

    }
}
