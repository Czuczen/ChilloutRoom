using System;
using System.Linq;
using System.Net;
using Abp.Dependency;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;

namespace CzuczenLand.ExtendingFunctionalities.BackgroundWorkers.AppReviver;

/// <summary>
/// Klasa wykonująca pracę w cyklach związaną z utrzymaniem ciągłości pracy aplikacji.
/// </summary>
public class AppReviverWorker : PeriodicBackgroundWorkerBase, ISingletonDependency
{
    /// <summary>
    /// Okres czasu (w milisekundach) między cyklami pracy.
    /// </summary>
    private const int PeriodTime = 60000; // 1min
    
    
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="timer">AbpTimer do określania czasu cyklu pracy.</param>
    public AppReviverWorker(AbpTimer timer) : base(timer)
    {
        Timer.Period = PeriodTime;
    }

    /// <summary>
    /// Służy do zapewnienia ciągłości działania aplikacji na Somee.com.
    /// Bez tego worker na Somee.com przestanie działać i przestanie wysyłać zapytania do tej aplikacji.
    /// Worker na Somee.com to kopia tego workera, z tą różnicą, że przesyła zapytania do tej aplikacji, zapewniając jej ciągłość działania.
    /// </summary>
    protected override void DoWork()
    {
        const string someComUrl = "http://reviver.somee.com/Account/Login";
        using var client = new WebClient();
        
        try
        {
            var data = client.DownloadData(someComUrl);
            if (!data.Any())
                Logger.Error("Blad! WebClient.DownloadData nie posiada danych. Url - " + someComUrl);
        }
        catch (Exception ex)
        {
            Logger.Error("Blad===========//==========", ex);
        }
    }
}
