using System;
using System.IO;
using Abp.Reflection.Extensions;

namespace CzuczenLand;

/// <summary>
/// Centralny punkt dla wersji aplikacji.
/// </summary>
public class AppVersionHelper
{
    /// <summary>
    /// Pobiera aktualną wersję aplikacji. 
    /// Jest ona również pokazywana na stronie internetowej.
    /// </summary>
    public const string Version = "4.6.3.1";

    /// <summary>
    /// Pobiera datę wydania (ostatniej kompilacji) aplikacji.
    /// Jest to pokazywane na stronie internetowej.
    /// </summary>
    public static DateTime ReleaseDate
    {
        get { return new FileInfo(typeof(AppVersionHelper).GetAssembly().Location).LastWriteTime; }
    }
}