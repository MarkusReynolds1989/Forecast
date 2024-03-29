﻿using System.Text;
using System.Text.RegularExpressions;

namespace Forecast;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        var zipMatch = new Regex("[0-9]{5}");

        if (args.Length < 1)
        {
            await Console.Error.WriteLineAsync("Please input a zip code as an argument.");
            Environment.Exit(1);
        }

        if (!zipMatch.IsMatch(args[0]))
        {
            await Console.Error.WriteLineAsync("Please input the zip code as a 5 digit number.");
            Environment.Exit(1);
        }

        string ForecastTemp(string zip) => $"https://weather.com/weather/tenday/l/{zip}";

        string CurrentWeather(string zip) => $"https://weather.com/weather/today/l/{zip}";

        var zip = args[0];

        var tenDayTemp =
            new Regex(@"class=""DailyContent--narrative--hplRl"">(.+?\.)</p>");

        var currentWeatherTemp =
            new Regex(@"class=""CurrentConditions--tempValue--3a50n"">([0-9]{1,3})");

        var currentWeather =
            new Regex(@"class=""CurrentConditions--phraseValue--2Z18W"">(.+?)</div>");

        var request = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(10)
        };

        var forecastContent = request.GetAsync(ForecastTemp(zip)).Result.Content.ReadAsStringAsync();
        var currentContent = request.GetAsync(CurrentWeather(zip)).Result.Content.ReadAsStringAsync();

        await Task.WhenAll(forecastContent, currentContent);

        var tenDayMatches = tenDayTemp.Matches(forecastContent.Result);
        var currentTemp = currentWeatherTemp.Matches(currentContent.Result);
        var weather = currentWeather.Matches(currentContent.Result);

        if (tenDayMatches.Count > 0 && currentTemp.Count > 0)
        {
            var outputBuffer = new StringBuilder();

            outputBuffer = outputBuffer.Append(
                $"Now:\t\t{currentTemp[0].Groups[1]}F {weather[0].Groups[1]}\nToday:\t\t{tenDayMatches[0].Groups[1]}\nTonight:\t{tenDayMatches[1].Groups[1]}");
            outputBuffer = outputBuffer.Append(
                $"\nTomorrow:\t{tenDayMatches[2].Groups[1]}\nTomorrow Night:\t{tenDayMatches[3].Groups[1]}");

            Console.WriteLine(outputBuffer);
        }
    }
}