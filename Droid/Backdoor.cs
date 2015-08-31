using System;

[assembly: Xamarin.Forms.Dependency (typeof (ScanAndREST.Droid.Backdoor))]
namespace ScanAndREST.Droid
{
    public class Backdoor: IBackdoor
    {
        #region IBackdoor implementation

        public void Vibrate(int time)
        {
            throw new NotImplementedException();
        }

        public bool IsCameraAvailable
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
