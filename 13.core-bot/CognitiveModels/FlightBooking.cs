// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;
using Microsoft.Bot.Builder;
using Microsoft.BotBuilderSamples.Clu;
using Newtonsoft.Json;

namespace Microsoft.BotBuilderSamples
{
    /// <summary>
    /// An <see cref="IRecognizerConvert"/> implementation that provides helper methods and properties to interact with
    /// the CLU recognizer results.
    /// </summary>
    public class FlightBooking : IRecognizerConvert
    {
        public enum Intent
        {
            BookFlight,
            None
        }

        public string Text { get; set; }

        public string AlteredText { get; set; }

        public Dictionary<Intent, IntentScore> Intents { get; set; }

        public CluEntities Entities { get; set; }

        public IDictionary<string, object> Properties { get; set; }

        public void Convert(dynamic result)
        {
            //var jsonResult = JsonConvert.SerializeObject(result, new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore});
            //var app = JsonConvert.DeserializeObject<FlightBooking>(jsonResult);
            var app = JsonConvert.DeserializeObject<FlightBooking>(JsonConvert.SerializeObject(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));

            Text = app.Text;
            AlteredText = app.AlteredText;
            Intents = app.Intents;
            Entities = app.Entities;
            Properties = app.Properties;
        }

        public (Intent intent, double score) GetTopIntent()
        {
            var maxIntent = Intent.None;
            var max = 0.0;
            foreach (var entry in Intents)
            {
                if (entry.Value.Score > max)
                {
                    maxIntent = entry.Key;
                    max = entry.Value.Score.Value;
                }
            }

            return (maxIntent, max);
        }

        public class CluEntities
        {
            public CluEntity[] Entities;

            public CluEntity[] GetDst_cityList() => Entities.Where(e => e.Category == "dst_city").ToArray();

            public CluEntity[] GetOr_cityList() => Entities.Where(e => e.Category == "or_city").ToArray();

            public CluEntity[] GetStr_dateList() => Entities.Where(e => e.Category == "str_date").ToArray();
			
            public CluEntity[] GetEnd_dateList() => Entities.Where(e => e.Category == "end_date").ToArray();
			
			public CluEntity[] GetBudgetList() => Entities.Where(e => e.Category == "budget").ToArray();


            public string GetDst_city() => GetDst_cityList().FirstOrDefault()?.Text;

            public string GetOr_city() => GetOr_cityList().FirstOrDefault()?.Text;

            public string GetStr_date() => GetStr_dateList().FirstOrDefault()?.Text;
			
			public string GetEnd_date() => GetEnd_dateList().FirstOrDefault()?.Text;

            public string GetBudget() => GetBudgetList().FirstOrDefault()?.Text;
        }
    }
}
