//using Microsoft.AspNetCore.Mvc;
//using vericekme.Models;
//using OpenQA.Selenium;
//using OpenQA.Selenium.Chrome;
//using System;
//using System.Threading;

//namespace vericekme.Controllers;

//public class HomeController : Controller
//{
//    public IActionResult Index() => View(new UserViewModel());

//    [HttpPost]
//    public IActionResult Analyze(string profileUrl)
//    {
//        var user = new UserViewModel { Website = profileUrl };
//        var options = new ChromeOptions();
//        options.AddArgument("--headless=new");
//        options.AddArgument("--disable-gpu");
//        options.AddArgument("--no-sandbox");

//        using (IWebDriver driver = new ChromeDriver(options))
//        {
//            try
//            {
//                driver.Navigate().GoToUrl(profileUrl);
//                Thread.Sleep(4000); // TikTok ve X bazen geç yüklenir, süreyi biraz artırdık

//                string source = driver.PageSource;
//                string title = driver.Title;

//                // --- PLATFORM: X (TWITTER) ---
//                if (profileUrl.Contains("x.com") || profileUrl.Contains("twitter.com"))
//                {
//                    // İsim temizleme: "İsim (@kullanici) / X" yapısından sadece İsim kısmını al
//                    if (title.Contains("(@"))
//                        user.Name = title.Split('(')[0].Trim();
//                    else
//                        user.Name = title.Replace("on X", "").Replace("/ X", "").Trim();

//                    // ID Çekme: Senin verdiğin "identifier":"770..." yapısını arıyoruz
//                    user.Id = ExtractLong(source, "\"identifier\":\"", "\"");
//                }

//                // --- PLATFORM: FACEBOOK ---
//                else if (profileUrl.Contains("facebook.com"))
//                {
//                    user.Name = title.Replace("| Facebook", "").Trim();
//                    user.Id = ExtractLong(source, "\"userID\":\"", "\"");
//                }

//                // --- PLATFORM: INSTAGRAM ---
//                else if (profileUrl.Contains("instagram.com"))
//                {
//                    user.Name = title.Split('(')[0].Trim();
//                    user.Id = ExtractLong(source, "\"profile_id\":\"", "\"");
//                }

//                // --- PLATFORM: TIKTOK ---
//                else if (profileUrl.Contains("tiktok.com"))
//                {
//                    // Profil Adı: Title'dan çekiyoruz (Vanlı Sako 0165 | TikTok gibi gelir)
//                    user.Name = title.Split('|')[0].Trim();

//                    // ID Çekme: Senin verdiğin userId=656... yapısını arıyoruz
//                    // TikTok'ta ID bazen userId= bazen de "userId":" şeklinde olabilir, ikisini de dener:
//                    user.Id = ExtractLong(source, "userId=", "&") ?? ExtractLong(source, "userId\":\"", "\"");
//                }
//            }
//            catch (Exception ex)
//            {
//                user.Name = "Hata oluştu: " + ex.Message;
//            }
//        }
//        return View("Index", user);
//    }

//    // Yardımcı Metot: Kaynak kodun içinden belirtilen etiketler arasındaki sayıyı çeker
//    private long? ExtractLong(string source, string startTag, string endTag)
//    {
//        try
//        {
//            if (source.Contains(startTag))
//            {
//                int start = source.IndexOf(startTag) + startTag.Length;
//                int end = source.IndexOf(endTag, start);

//                if (end > start)
//                {
//                    string value = source.Substring(start, end - start);
//                    // Sadece rakamları temizle (Bazen sonda gereksiz karakter kalabiliyor)
//                    string cleanValue = System.Text.RegularExpressions.Regex.Replace(value, "[^0-9]", "");

//                    if (long.TryParse(cleanValue, out long result))
//                        return result;
//                }
//            }
//        }
//        catch { }
//        return null;
//    }
//}




////buda oluyor sadece tiktok olmuyor
//using Microsoft.AspNetCore.Mvc;
//using vericekme.Models;
//using OpenQA.Selenium;
//using OpenQA.Selenium.Chrome;
//using System;
//using System.Threading;
//using System.Text.RegularExpressions;

//namespace vericekme.Controllers;

//public class HomeController : Controller
//{
//    public IActionResult Index() => View(new UserViewModel());

//    [HttpPost]
//    public IActionResult Analyze(string profileUrl)
//    {
//        var user = new UserViewModel { Website = profileUrl };
//        var options = new ChromeOptions();
//        options.AddArgument("--headless=new");
//        options.AddArgument("--disable-gpu");
//        options.AddArgument("--no-sandbox");

//        using (IWebDriver driver = new ChromeDriver(options))
//        {
//            try
//            {
//                driver.Navigate().GoToUrl(profileUrl);
//                Thread.Sleep(5000); // TikTok verisi için 5 saniye bekleme en sağlıklısı

//                string source = driver.PageSource;
//                string title = driver.Title;

//                // --- PLATFORM: X (TWITTER) ---
//                if (profileUrl.Contains("x.com") || profileUrl.Contains("twitter.com"))
//                {
//                    if (title.Contains("(@"))
//                        user.Name = title.Split('(')[0].Trim();
//                    else
//                        user.Name = title.Replace("on X", "").Replace("/ X", "").Trim();

//                    user.Id = ExtractLong(source, "\"identifier\":\"", "\"");
//                }

//                // --- PLATFORM: FACEBOOK ---
//                else if (profileUrl.Contains("facebook.com"))
//                {
//                    user.Name = title.Replace("| Facebook", "").Trim();
//                    user.Id = ExtractLong(source, "\"userID\":\"", "\"");
//                }

//                // --- PLATFORM: INSTAGRAM ---
//                else if (profileUrl.Contains("instagram.com"))
//                {
//                    user.Name = title.Split('(')[0].Trim();
//                    user.Id = ExtractLong(source, "\"profile_id\":\"", "\"");
//                }

//                // --- PLATFORM: TIKTOK ---
//                // --- PLATFORM: TIKTOK (H1 VE USERID ÖZEL ÇÖZÜM) ---
//                else if (profileUrl.Contains("tiktok.com"))
//                {
//                    try
//                    {
//                        // 1. Kullanıcı Adını doğrudan ekrandaki H1 etiketinden çek
//                        var h1Element = driver.FindElement(By.CssSelector("h1[data-e2e='user-title']"));
//                        user.Name = h1Element.Text.Trim();
//                    }
//                    catch
//                    {
//                        // Eğer H1 bulunamazsa başlıktan al
//                        user.Name = title.Split('|')[0].Trim();
//                    }

//                    // 2. ID Çekme: Senin verdiğin ?userId=...& kalıbını kaynak kodda ara
//                    var idMatch = Regex.Match(source, @"userId=(\d+)");
//                    if (idMatch.Success)
//                    {
//                        if (long.TryParse(idMatch.Groups[1].Value, out long tId))
//                            user.Id = tId;
//                    }

//                    // Eğer hala "Make Your Day" gibi bir şey gelirse temizle
//                    if (user.Name.Contains("Make Your Day")) user.Name = "Bilinmiyor";
//                }
//            }



//            catch (Exception ex)
//            {
//                user.Name = "Hata oluştu: " + ex.Message;
//            }
//        }
//        return View("Index", user);
//    }

//    // Mevcut ExtractLong metodun (Bozmadık, aynen duruyor)
//    private long? ExtractLong(string source, string startTag, string endTag)
//    {
//        try
//        {
//            if (source.Contains(startTag))
//            {
//                int start = source.IndexOf(startTag) + startTag.Length;
//                int end = source.IndexOf(endTag, start);

//                if (end > start)
//                {
//                    string value = source.Substring(start, end - start);
//                    string cleanValue = Regex.Replace(value, "[^0-9]", "");
//                    if (long.TryParse(cleanValue, out long result))
//                        return result;
//                }
//            }
//        }
//        catch { }
//        return null;
//    }
//}

//bu yukarıdaki sadece tiktok olmuyor



//// bu kod eksiksiz çalışıyor hepsi doğru (başlangıç)
//using Microsoft.AspNetCore.Mvc;
//using vericekme.Models;
//using OpenQA.Selenium;
//using OpenQA.Selenium.Chrome;
//using System;
//using System.Threading;
//using System.Text.RegularExpressions;
//using System.Net;

//namespace vericekme.Controllers
//{
//    public partial class HomeController : Controller
//    {
//        public IActionResult Index() => View(new UserViewModel());

//        [HttpPost]
//        public IActionResult Analyze(string profileUrl)
//        {
//            var user = new UserViewModel { Website = profileUrl };
//            var options = new ChromeOptions();
//            options.AddArgument("--headless=new");
//            options.AddArgument("--disable-gpu");
//            options.AddArgument("--no-sandbox");

//            using (IWebDriver driver = new ChromeDriver(options))
//            {
//                try
//                {
//                    driver.Navigate().GoToUrl(profileUrl);
//                    // TikTok ve Facebook dinamik içerik için bekleme süresi
//                    Thread.Sleep(5000);

//                    string source = driver.PageSource;
//                    string title = driver.Title;

//                    // --- PLATFORM: X (TWITTER) ---
//                    if (profileUrl.Contains("x.com") || profileUrl.Contains("twitter.com"))
//                    {
//                        if (title.Contains("(@"))
//                            user.Name = title.Split('(')[0].Trim();
//                        else
//                            user.Name = title.Replace("on X", "").Replace("/ X", "").Trim();

//                        user.Id = ExtractLong(source, "\"identifier\":\"", "\"");
//                    }

//                    // --- PLATFORM: FACEBOOK (DÜZELTİLDİ) ---
//                    else if (profileUrl.Contains("facebook.com"))
//                    {
//                        // Sadece "| Facebook" değil, "| Van" veya "| İstanbul" gibi ekleri de temizler
//                        if (title.Contains("|"))
//                        {
//                            user.Name = title.Split('|')[0].Trim();
//                        }
//                        else
//                        {
//                            user.Name = title.Replace("Facebook", "").Trim();
//                        }

//                        user.Id = ExtractLong(source, "\"userID\":\"", "\"") ?? ExtractLong(source, "\"entity_id\":\"", "\"");
//                    }




//                    // --- PLATFORM: INSTAGRAM ---
//                    else if (profileUrl.Contains("instagram.com"))
//                    {
//                        user.Name = title.Split('(')[0].Trim();
//                        user.Id = ExtractLong(source, "\"profile_id\":\"", "\"");
//                    }

//                    // --- PLATFORM: TIKTOK (YENİ MANTIK EKLENDİ) ---
//                    else if (profileUrl.Contains("tiktok.com"))
//                    {
//                        // 1. Regex ile ID ve Nickname çekme (Gönderdiğin çalışan mantık)
//                        var idMatch = Regex.Match(source, @"""id""\s*:\s*""(\d{15,25})""|""user""\s*:\s*\{\s*""id""\s*:\s*""(\d+)""");
//                        var nickMatch = Regex.Match(source, @"""nickname""\s*:\s*""([^""]+)""");

//                        if (idMatch.Success)
//                        {
//                            string rawId = string.IsNullOrEmpty(idMatch.Groups[1].Value) ? idMatch.Groups[2].Value : idMatch.Groups[1].Value;
//                            if (long.TryParse(rawId, out long tId)) user.Id = tId;
//                        }

//                        if (nickMatch.Success)
//                        {
//                            // Unescape ve Decode işlemiyle Türkçe karakterleri düzeltir
//                            user.Name = WebUtility.HtmlDecode(Regex.Unescape(nickMatch.Groups[1].Value));
//                        }
//                        else
//                        {
//                            // Eğer regex bulamazsa klasik yöntem
//                            user.Name = title.Split('|')[0].Trim();
//                        }

//                        // Temizlik: "Make Your Day" uyarısı için
//                        if (string.IsNullOrEmpty(user.Name) || user.Name.Contains("Make Your Day"))
//                            user.Name = "Bilinmiyor";
//                    }
//                }
//                catch (Exception ex)
//                {
//                    user.Name = "Hata oluştu: " + ex.Message;
//                }
//            }
//            return View("Index", user);
//        }

//        private long? ExtractLong(string source, string startTag, string endTag)
//        {
//            try
//            {
//                if (source.Contains(startTag))
//                {
//                    int start = source.IndexOf(startTag) + startTag.Length;
//                    int end = source.IndexOf(endTag, start);

//                    if (end > start)
//                    {
//                        string value = source.Substring(start, end - start);
//                        string cleanValue = Regex.Replace(value, "[^0-9]", "");
//                        if (long.TryParse(cleanValue, out long result))
//                            return result;
//                    }
//                }
//            }
//            catch { }
//            return null;
//        }
//    }
////}
//// bu kod eksiksiz çalışıyor hepsi doğru (bitiş)
///



//buda sorunsuz çalışıyor
using Microsoft.AspNetCore.Mvc;
using vericekme.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Text.RegularExpressions;
using System.Net;

namespace vericekme.Controllers
{
    public partial class HomeController : Controller
    {
        public IActionResult Index() => View(new UserViewModel());

        [HttpPost]
        public IActionResult Analyze(string profileUrl)
        {
            var user = new UserViewModel { Website = profileUrl };

            var options = new ChromeOptions();
            options.AddArgument("--headless=new");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--no-sandbox");

            // hız optimizasyonu (bozmayacak olanlar)
            options.AddArgument("--disable-extensions");
            options.AddArgument("--disable-dev-shm-usage");
            options.AddArgument("--log-level=3");

            var service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;

            using (IWebDriver driver = new ChromeDriver(service, options))
            {
                try
                {
                    driver.Navigate().GoToUrl(profileUrl);

                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(6));

                    // 🔥 PLATFORMA GÖRE BEKLEME
                    if (profileUrl.Contains("x.com") || profileUrl.Contains("twitter.com"))
                    {
                        // X için veri gelene kadar bekle
                        wait.Until(d => d.PageSource.Contains("identifier"));
                    }
                    else if (profileUrl.Contains("facebook.com"))
                    {
                        // Facebook ID gelene kadar bekle
                        wait.Until(d => d.PageSource.Contains("userID") || d.PageSource.Contains("entity_id"));
                    }
                    else
                    {
                        // diğerleri hızlı
                        wait.Until(d => ((IJavaScriptExecutor)d)
                            .ExecuteScript("return document.readyState").Equals("complete"));
                    }

                    string source = driver.PageSource;
                    string title = driver.Title;

                    // --- X (TWITTER) ---
                    if (profileUrl.Contains("x.com") || profileUrl.Contains("twitter.com"))
                    {
                        if (title.Contains("(@"))
                            user.Name = title.Split('(')[0].Trim();
                        else
                            user.Name = title.Replace("on X", "").Replace("/ X", "").Trim();

                        user.Id = ExtractLong(source, "\"identifier\":\"", "\"");
                    }

                    // --- FACEBOOK ---
                    else if (profileUrl.Contains("facebook.com"))
                    {
                        if (title.Contains("|"))
                            user.Name = title.Split('|')[0].Trim();
                        else
                            user.Name = title.Replace("Facebook", "").Trim();

                        user.Id = ExtractLong(source, "\"userID\":\"", "\"")
                               ?? ExtractLong(source, "\"entity_id\":\"", "\"");
                    }

                    // --- INSTAGRAM ---
                    else if (profileUrl.Contains("instagram.com"))
                    {
                        user.Name = title.Split('(')[0].Trim();
                        user.Id = ExtractLong(source, "\"profile_id\":\"", "\"");
                    }

                    // --- TIKTOK ---
                    else if (profileUrl.Contains("tiktok.com"))
                    {
                        var idMatch = Regex.Match(source, @"""id""\s*:\s*""(\d{15,25})""|""user""\s*:\s*\{\s*""id""\s*:\s*""(\d+)""");
                        var nickMatch = Regex.Match(source, @"""nickname""\s*:\s*""([^""]+)""");

                        if (idMatch.Success)
                        {
                            string rawId = string.IsNullOrEmpty(idMatch.Groups[1].Value)
                                ? idMatch.Groups[2].Value
                                : idMatch.Groups[1].Value;

                            if (long.TryParse(rawId, out long tId))
                                user.Id = tId;
                        }

                        if (nickMatch.Success)
                        {
                            user.Name = WebUtility.HtmlDecode(
                                Regex.Unescape(nickMatch.Groups[1].Value));
                        }
                        else
                        {
                            user.Name = title.Split('|')[0].Trim();
                        }

                        if (string.IsNullOrEmpty(user.Name) || user.Name.Contains("Make Your Day"))
                            user.Name = "Bilinmiyor";
                    }
                }
                catch (Exception ex)
                {
                    user.Name = "Hata oluştu: " + ex.Message;
                }
            }

            return View("Index", user);
        }

        private long? ExtractLong(string source, string startTag, string endTag)
        {
            try
            {
                if (source.Contains(startTag))
                {
                    int start = source.IndexOf(startTag) + startTag.Length;
                    int end = source.IndexOf(endTag, start);

                    if (end > start)
                    {
                        string value = source.Substring(start, end - start);
                        string cleanValue = Regex.Replace(value, "[^0-9]", "");

                        if (long.TryParse(cleanValue, out long result))
                            return result;
                    }
                }
            }
            catch { }

            return null;
        }
    }
}



