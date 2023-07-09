using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace StreamerPlusApp
{
    static class css_js
    {
        public static Dictionary<string, string> css = new Dictionary<string, string>();
        public static Dictionary<string, string> js = new Dictionary<string, string>();


        public static void INI()
        {
            LoadCSS();
            LoadJS();
        }

        private static void LoadCSS()
        {
            //chat / loginError
            #region chat
            css["chat"] = @"@import url(""https: //fonts.googleapis.com/css?family=Heebo&display=swap"");
html:not([dark]) body {
	filter: invert(94%)
}

html:not([dark]) *:is(img,
#text,
#icon,
yt-icon,
.moderator,
yt-icon,
#focused,
.owner){
	filter: invert(100%)
}

span,
b,
label,
p,
pre,
#message,
#text,
#yt-dropdown-menu,
.style-scope {
	font-weight: bolder;
	font-family: heebo;
	letter-spacing: 0.03em;
	word-spacing: 0.1em;
}

body:after {
	display: block;
	content: ""סטרימר פלוס"";
	z-index: 999;
	position: absolute;
	top: 0;
	color: #000;
	font-size: min(4vw, 30px);
	font-family: heebo;
	margin: 0;
	right: 37vw;
	text-shadow: 2px 2px 0px aqua;
	background: gray;
	border-radius: 0px 0px 10px 10px;
	box-shadow: inset 0px -15px 30px -15px #fff;
	width: 22.5vw;
	padding: 0 2vw;
	text-align: center;
}

html[dark] body:after
{
	box-shadow: inset 0px -15px 30px -15px #fff;
    color:#fff;
	text-shadow: 2px 2px 0px red;

}

html:not([dark]) *:is(#menu,
#picker-buttons,
#panel-contents,
#visible-banners) {
	filter: invert(1) !important;
}

div#visible-banners * {
	filter: invert(1);
    color: #fff;
}

html:not([dark]) .yt-live-interactivity-component-background {
	filter: hue-rotate(132deg) brightness(1.1) contrast(1.9) !important;
}
";
            #endregion
            #region loginError
            css["loginError"] = @"body {background-color: #222222;}
                        h1{color: #ffffff;font-weight: 900 !important;letter-spacing: 0.1vmin;font-size: 6vmin !important;}
                        div div:not(#content) {display: none;}
                        div#content h1::after {content: ""נסה להתחבר מחדש בהגדרות או לפתוח את התוכנה מחדש."";display: block;direction: rtl;font - size: 4vmin;font - weight: 100;opacity: 0.6;}";            ;
            #endregion

            css["switch_user"] = @"
#masthead-container, ytd-masthead,
#contents.ytd-channel-switcher-page-renderer ytd-button-renderer.ytd-channel-switcher-page-renderer,
ytd-settings-sidebar-renderer{
    display:none !important;
}
ytd-page-manager,
ytd-browse,
ytd-section-list-renderer,
ytd-channel-switcher-header-renderer
{
    margin: 0 !important;
}
ytd-browse,
{
    width:100% !important;
    max-width: 100% !important;
}
ytd-channel-switcher-header-renderer
{
    width: 100%;
    display: block;
    font-size: 0 !important;
    color: transparent;
    position: relative;
    padding-bottom: 4vmin !important;
}
ytd-channel-switcher-header-renderer::before
{
    content: ""בחר איזה באיזה ערוץ אתה עושה לייב:"";
    display: block;
    width: 100%;
    position: relative;
    margin: auto;
    font-size: 6vmin;
    font-weight: 700;
    color: black;
}

ytd-channel-switcher-header-renderer::after {
    content: ""אם אתה מוחזר לדף הזה, אז התוכנה זיהתה שלייבים לא זמינים עבור הערוץ שבחרת.."";
    width: 100%;
    position: absolute;
    margin: auto;
    font-size: 4vmin;
    font-weight: 100;
    color: red;
    line-height: 3.5vmin;
    bottom: 0;
}";
        }

        private static void LoadJS()
        {
            #region reLoginWait
            js["reLoginWait"] = @"var new_window = window.open('" + Urls.youtube["login"] + @"', '_blank', 'location=false,height=600,width=700,scrollbars=yes,status=yes');
                new_window.onbeforeunload = function(){
                    window.location.href='" + Urls.youtube["dashboard"] + @"';               
                };
                setInterval(()=>{
                    if(new_window.closed)
                         window.location.href='" + Urls.youtube["dashboard"] + @"';
                },10)";
            #endregion

            #region InsertAd
            js["InsertAd"] = @"setTimeout(()=>{document.querySelector('#insert-ad-button').click()
            setTimeout(()=>{
                window.location.href = '" + Urls.olympicangel["event_end"] + @"'},3000);
            },2000)";
            #endregion

            #region InsertMarker
            js["InsertMarker"] = @"setTimeout(()=>{document.querySelector('#insert-highlight-marker-button').click()
                setTimeout(()=>{window.location.href = '" + Urls.olympicangel["event_end"] + @"'},3000);
            },2000)";
            #endregion

            js["clickTest"] = "";

            js["invalidUser"] = @"
            function testFunc()
            {
                let continueBtn = Array.from(document.querySelectorAll(""#dialog-buttons #confirm-button dom-if + div.label.ytcp-button"")).find(btn=>{return btn.innerText == ""המשך"" || btn.innerText == ""continue""});
                if(continueBtn){
                    clearInterval(window.testInterval);
                    continueBtn.click()
                    return;
                }

                var btn = document.querySelector('#features li tp-yt-paper-icon-item[aria-disabled=true]');
                var loader = document.querySelector('tp-yt-paper-progress#app-loading[hidden]');
                if(!btn || !loader)
                    return console.log(btn,loader)

                if(btn && loader)
                {
                    if(btn.getAttribute('tabindex') == '-1')
                    {
                             location = '{url}';
                             clearInterval(window.testInterval);
                    }
                }
            }
            if(window.testInterval == undefined)
                window.testInterval = setInterval(testFunc,75);
";
        }
    }
}
