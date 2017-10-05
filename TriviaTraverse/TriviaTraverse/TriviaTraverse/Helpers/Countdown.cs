//
//  Countdown.cs
//  Created by Alexey Kinev on 11 Jan 2015.
//
//    Licensed under The MIT License (MIT)
//    http://opensource.org/licenses/MIT
//
//    Copyright (c) 2015 Alexey Kinev <alexey.rudy@gmail.com>
//
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
//using System.Timers;

namespace TriviaTraverse.Helpers
{
    /// <summary>
    /// Countdown timer with periodical ticks.
    /// </summary>
    public class Countdown : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the start date time.
        /// </summary>
        public DateTime StartDateTime { get; private set; }

        /// <summary>
        /// Gets the remain time in seconds.
        /// </summary>
        public double RemainTime
        {
            get { return remainTime; }

            private set
            {
                remainTime = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Occurs when completed.
        /// </summary>
        public event Action Completed;

        /// <summary>
        /// Occurs when ticked.
        /// </summary>
        public event Action Ticked;

        /// <summary>
        /// The timer.
        /// </summary>
        //Timer timer;

        bool timerActive;

        /// <summary>
        /// The remain time.
        /// </summary>
        double remainTime;

        /// <summary>
        /// The remain time total.
        /// </summary>
        double remainTimeTotal;

        /// <summary>
        /// Starts the updating with specified period, total time and period are specified in seconds.
        /// </summary>
        public void StartUpdating(double total, double period = 1.0)
        {
            //if (timer != null)
            //{
            //    ResetUpdating();
            //}

            timerActive = true;
            remainTimeTotal = total;
            RemainTime = total;

            StartDateTime = DateTime.Now;

            //timer = new Timer(period * 1000);
            //timer.Elapsed += (sender, e) => Tick();
            //timer.Enabled = true;

            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                return Tick();

                //return true; // True = Repeat again, False = Stop the timer
            });

        }

        /// <summary>
        /// Stops the updating.
        /// </summary>
        public void StopUpdating()
        {
            //if (timer != null)
            //{
            //    timer.Enabled = false;
            //    timer = null;
            //}
            timerActive = false;
        }

        /// <summary>
        /// Stops the updating.
        /// </summary>
        public void ResetUpdating()
        {
            RemainTime = 0;
            remainTimeTotal = 0;

            timerActive = false;
        }

        /// <summary>
        /// Updates the time remain.
        /// </summary>
        public bool Tick()
        {
            if (timerActive)
            {
                var delta = (DateTime.Now - StartDateTime).TotalSeconds;

                if (delta < remainTimeTotal)
                {
                    RemainTime = remainTimeTotal - delta;

                    var ticked = Ticked;
                    if (ticked != null)
                    {
                        ticked();
                    }
                    return true;
                }
                else
                {
                    RemainTime = 0;

                    var completed = Completed;
                    if (completed != null)
                    {
                        completed();
                    }
                    return false;  //stops the timer
                }
            }
            else
            {
                return false;
            }
        }

        #region INotifyPropertyChanged implementation

        /// <summary>
        /// Occurs when property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the property changed event.
        /// </summary>
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}