using System.Threading.Tasks;

namespace Llh.Client.Services
{
    public class SoundNotificationService
    {
        public bool IsPlayingsound { get; private set; }

        public bool IsEnabled { get; set; } = true;

        public async Task PlaySound()
        {
            if (!IsEnabled)
            {
                return;
            }

            IsPlayingsound = true;
            IsPlayingsoundChanged.Invoke();

            await Task.Delay(2000);

            IsPlayingsound = false;
            IsPlayingsoundChanged.Invoke();
        }

        public delegate void IsPlayingsoundChangedHandler();
        public event IsPlayingsoundChangedHandler IsPlayingsoundChanged;
    }
}
