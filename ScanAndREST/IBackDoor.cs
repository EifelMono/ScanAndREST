using System;

using Xamarin.Forms;

namespace ScanAndREST
{
    public interface IBackdoor 
    {
        bool IsCameraAvailable { get;}
        void Vibrate(int time);
    }
}


