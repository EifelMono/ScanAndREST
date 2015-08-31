using System;

using Xamarin.Forms;

namespace ScanAndREST
{
    public class Backdoor
    {
        protected static IBackdoor m_Backdoor = null;

        public static IBackdoor Instance
        {
            get
            { 
                if (m_Backdoor == null)
                    m_Backdoor = (IBackdoor)DependencyService.Get<IBackdoor>();

                return m_Backdoor;
            }
        }

        #region IBackDoor implementation

        public static void Vibrate(int time= 500)
        {
            Instance.Vibrate(time);
        }

        public static bool IsCameraAvailable
        {
            get
            {
                return Instance.IsCameraAvailable;
            }
        }

        #endregion
    }
}


