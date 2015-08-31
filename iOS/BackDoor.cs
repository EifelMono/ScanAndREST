using System;
using AudioToolbox;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(ScanAndREST.iOS.Backdoor))]

namespace ScanAndREST.iOS
{
    public class Backdoor: IBackdoor
    {
        #region IBackdoor implementation

        /// <summary>
        /// Vibrate the specified time.
        /// but no time possible for iOS
        /// </summary>
        /// <param name="time">Time.</param>
        public void Vibrate(int time)
        {
            SystemSound.Vibrate.PlaySystemSound();
            ;
        }

        public bool IsCameraAvailable
        {
            get
            {
                return UIImagePickerController.IsCameraDeviceAvailable(UIImagePickerControllerCameraDevice.Rear)
                || UIImagePickerController.IsCameraDeviceAvailable(UIImagePickerControllerCameraDevice.Front);
            }
        }

        #endregion
    }
}

