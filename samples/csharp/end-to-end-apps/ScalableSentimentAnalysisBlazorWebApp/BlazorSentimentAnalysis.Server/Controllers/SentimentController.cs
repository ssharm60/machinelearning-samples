﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorSentimentAnalysis.Server.ML.DataModels;
using Microsoft.Extensions.ML;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorSentimentAnalysis.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SentimentController : ControllerBase
    {
        private readonly PredictionEnginePool<SampleObservation, SamplePrediction> _predictionEnginePool;

        public SentimentController(PredictionEnginePool<SampleObservation, SamplePrediction> predictionEnginePool)
        {
            // Get the ML Model Engine injected, for scoring
            _predictionEnginePool = predictionEnginePool;
        }

        [HttpGet("[action]")]
        [Route("sentimentprediction")]
        public ActionResult<float> PredictSentiment([FromQuery]string sentimentText)
        {
            string sampleText = sentimentText;

            // Random simulation
            //Random random = new Random();
            //float percentage = random.Next(0, 100);

            // Predict sentiment using ML.NET model
            SampleObservation sampleData = new SampleObservation() { Col0 = sentimentText };
            //Predict sentiment
            SamplePrediction prediction = _predictionEnginePool.Predict(sampleData);
            float percentage = CalculatePercentage(prediction.Score);

            return percentage;
        }

        public float CalculatePercentage(double value)
        {
            return 100 * (1.0f / (1.0f + (float)Math.Exp(-value)));
        }
    }
}