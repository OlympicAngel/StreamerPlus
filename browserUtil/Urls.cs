using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamerPlusApp
{
    public static class Urls
    {
        public static Dictionary<string,string> youtube = new Dictionary<string,string>() {
            { "login", "https://accounts.google.com/ServiceLogin?service=youtube&uilel=3&passive=true&continue=https%3A%2F%2Fwww.youtube.com%2Flive_dashboard%3Fnv%3D0&hl=he" },
            { "select_account", "https://www.youtube.com/channel_switcher" },
            { "select_account_path", "https://www.youtube.com/account" },
            { "logout","https://www.youtube.com/logout" },
            { "dashboard", "https://www.youtube.com/live_dashboard?nv=0" },
            { "chat","https://www.youtube.com/live_chat?v=" },
            { "olympic" , "https://www.youtube.com/OlympicAngel" },
            { "livestream_placeholder", "https://studio.youtube.com/video/{id}/livestreaming"},

            { "minLoginURL","://accounts.google.com"},
            { "youtubeUrl","https://www.youtube.com/"}
        };
        public static Dictionary<string, string> streamlabs = new Dictionary<string, string>() {
            { "login", "path" },
            { "logout","https://streamlabs.com/login?skip_splash=1&r=/dashboard&youtube=1&landing=1&force_login=1" },
            { "dashboard","https://streamlabs.com/dashboard#/" },
            { "events", "https://streamlabs.com/dashboard/recent-events" }
        };
        public static Dictionary<string, string> olympicangel = new Dictionary<string, string>(){
            { "settings","https://www.olympicangelabz.com/pages/stream-settings/full.php" },
            { "event_end","https://www.undefinedurlforevent.com/"}
        };

    }
}
