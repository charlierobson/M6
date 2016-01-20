using System;
using System.Collections.Generic;
using System.Linq;

namespace M6.Processing.OnsetDetection
{
    public class OnsetDetection
    {
        private readonly FFT _fft = new FFT();

        private readonly int _sampleWindowSize;
        private readonly int _sampleRate;

        private readonly float[] _previousSpectrum;
        private readonly float[] _spectrum;

        private readonly bool _rectify;

        private readonly List<float> _fluxes;

        // Constructor
        public OnsetDetection(int sampleWindowSize, int sampleRate)
        {
            _sampleWindowSize = sampleWindowSize;
            _sampleRate = sampleRate;

            _spectrum = new float[sampleWindowSize / 2 + 1];
            _previousSpectrum = new float[_spectrum.Length];
            _fluxes = new List<float>();
            _rectify = true;
        }

        /// <summary>
        ///  Perform Spectral Flux onset detection on loaded audio file
        ///  <para>Recommended onset detection algorithm for most needs</para>  
        /// </summary>
        /// <param name="monoSampleData"></param>
        /// <param name="hamming">Apply hamming window before FFT function. 
        ///  <para>Smooths out the noise in between peaks.</para> 
        ///  <para>Small improvement but isn't too costly.</para> 
        ///  <para>Default: true</para></param>
        public void AddFlux(float[] monoSampleData, bool hamming = true)
        {
            // Perform Fast Fourier Transform on the audio monoSampleData
            _fft.RealFFT(monoSampleData, hamming);

            // Update spectrums
            Array.Copy(_spectrum, _previousSpectrum, _spectrum.Length);
            Array.Copy(_fft.GetPowerSpectrum(), _spectrum, _spectrum.Length);

            _fluxes.Add(CompareSpectrums(_spectrum, _previousSpectrum, _rectify));
        }

        /// <param name="thresholdTimeSpan">Amount of data used during threshold averaging, in seconds.
        /// <para>Default: 1</para></param>
        /// <param name="sensitivity">Sensitivivity of onset detection.
        /// <para>Lower increases the sensitivity</para>
        /// <para>Recommended: 1.3 - 1.6</para>
        /// <para>Default: 1.5</para></param>
        // Use threshold average to find the onsets from the spectral flux
        public float[] FindOnsets(float sensitivity = 1.5f, float thresholdTimeSpan = 0.5f)
        {
            var thresholdAverage = GetThresholdAverage(_fluxes, _sampleWindowSize, thresholdTimeSpan, sensitivity);
            return GetPeaks(_fluxes, thresholdAverage, _sampleWindowSize);
        }

        /// <summary>
        ///  Normalize the beats found.
        /// </summary>
        /// <param name="onsets"></param>
        /// <param name="type">Type of normaliztion.
        /// <para>0 = Normalize onsets between 0 and max onset</para>
        /// <para>1 = Normalize onsets between min onset and max onset.</para>
        /// <para>2 = clamp values less than max / 8 to 0, set others to 1.</para>
        /// </param>
        public void NormalizeOnsets(float[] onsets, int type)
        {
            var min = onsets.Min();
            var max = onsets.Max();
            var maxclip = max/8;
            var difference = max - min;

            // Normalize the onsets
            switch (type)
            {
                case 0:
                    for (var i = 0; i < onsets.Length; i++)
                    {
                        onsets[i] /= max;
                    }
                    break;
                case 1:
                    for (var i = 0; i < onsets.Length; i++)
                    {
                        if (onsets[i] == min)
                        {
                            onsets[i] = 0.01f;
                        }
                        else
                        {
                            onsets[i] -= min;
                            onsets[i] /= difference;
                        }
                    }
                    break;

                case 2:
                    for (var i = 0; i < onsets.Length; i++)
                    {
                        if (onsets[i] < maxclip)
                        {
                            onsets[i] = 0;
                        }
                        else
                        {
                            onsets[i] = 1;
                        }
                    }
                    break;

                default:
                    break;
            }
        }

        private float CompareSpectrums(float[] spectrum, float[] previousSpectrum, bool rectify)
        {
            // Find difference between each respective bins of each spectrum
            // Sum up these differences
            float flux = 0;
            for (var i = 0; i < spectrum.Length; i++)
            {
                var value = (spectrum[i] - previousSpectrum[i]);

                // If ignoreNegativeEnergy is true
                // Only interested in rise in energy, ignore negative values
                if (!rectify || value > 0)
                {
                    flux += value;
                }
            }

            return flux;
        }

        // Finds the peaks in the flux above the threshold average
        private float[] GetPeaks(List<float> data, float[] dataAverage, int sampleCount)
        {
            // Time window in which humans can not distinguish beats in seconds
            const float indistinguishableRange = 0.01f; // 10ms
            // Number of set of monoSampleData to ignore after an onset
            var immunityPeriod = (int)((float)sampleCount
                / _sampleRate
                / indistinguishableRange);

            // Results
            var peaks = new float[data.Count];

            // For each sample
            for (var i = 0; i < data.Count; i++)
            {
                // Add the peak if above the average, else 0
                if (data[i] >= dataAverage[i])
                {
                    peaks[i] = data[i] - dataAverage[i];
                }
                else
                {
                    peaks[i] = 0.0f;
                }
            }

            // Prune the peaks list
            peaks[0] = 0.0f;
            for (var i = 1; i < peaks.Length - 1; i++)
            {
                // If the next value is lower than the current value, that means it is end of the peak
                if (peaks[i] < peaks[i + 1])
                {
                    peaks[i] = 0.0f;
                    continue;
                }

                // Remove peaks too close to each other
                if (peaks[i] > 0.0f)
                {
                    for (var j = i + 1; j < i + immunityPeriod; j++)
                    {
                        if (peaks[j] > 0)
                        {
                            peaks[j] = 0.0f;
                        }
                    }
                }
            }

            return peaks;
        }

        // Find the running average of the given list
        private float[] GetThresholdAverage(List<float> data, int sampleWindow, float thresholdTimeSpan, float thresholdMultiplier)
        {
            var thresholdAverage = new List<float>();

            // How many spectral fluxes to look at, at a time (approximation is fine)
            var sourceTimeSpan = (float)(sampleWindow) / _sampleRate;
            var windowSize = (int)(thresholdTimeSpan / sourceTimeSpan / 2);

            for (var i = 0; i < data.Count; i++)
            {
                // Max/Min Prevent index out of bounds error
                // Look at values to the left and right of the current spectral flux
                var start = Math.Max(i - windowSize, 0);
                var end = Math.Min(data.Count, i + windowSize);
                // Current average
                float mean = 0;

                // Sum up the surrounding values
                for (var j = start; j < end; j++)
                {
                    mean += data[j];
                }

                // Find the average
                mean /= (end - start);

                // Multiply mean to increase the sensitivity
                thresholdAverage.Add(mean * thresholdMultiplier);
            }

            return thresholdAverage.ToArray();
        }
    }
}
