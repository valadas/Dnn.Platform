// This file loads or creates the local settings file you can use to influence the build process
// and/or maintain a local DNN development site

using System;
using Cake.Frosting;

public class LocalSettings {
    public string WebsitePath {get; set;} = "";
    public string WebsiteUrl {get; set;} = "";
    public string SaConnectionString {get; set;} = "server=(local);Trusted_Connection=True;";
    public string DnnConnectionString {get; set;} = "";
    public string DbOwner {get; set;} = "dbo";
    public string ObjectQualifier {get; set;} = "";
    public string DnnDatabaseName {get; set;} = "Dnn_Platform";
    public string DnnSqlUsername {get; set;} = "";
    public string DatabasePath {get; set;} = "";
    public string Version {get; set;} = "auto";
}

public sealed class CreateSettings : FrostingTask<Context>
{
    public override void Run(Context context)
    {
        Console.Write("What version would you like to set (default=auto): ");
        var version = Console.ReadLine();
    }
    // Doesn't need to do anything as it's done automatically
}
