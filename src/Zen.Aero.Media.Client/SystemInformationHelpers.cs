using Windows.System.Profile;

namespace Zen.Aero.Media.Client
{
    public class SystemInformationHelpers
    {
        // For now, the 10-foot experience is enabled only on Xbox.
        public static bool IsTenFootExperience { get; } =
            AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Xbox";
    }
}